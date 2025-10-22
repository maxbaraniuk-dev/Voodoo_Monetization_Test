using System.Collections.Generic;
using Events;
using Game.Level;
using Infrastructure;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace UI
{
    public class DifficultySelectDialog : BaseView
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button unlockButton;
        [SerializeField] private Toggle easyToggle;
        [SerializeField] private Toggle mediumToggle;
        [SerializeField] private Toggle hardToggle;

        private readonly Dictionary<Toggle, DifficultyLevel> _difficultyToggles = new();
        
        private DifficultyLevel _difficultyLevel;
        private List<LevelState> _openedDifficulties;
        
        public override void Show()
        {
            _openedDifficulties = Context.GetSystem<IUserSystem>().GetUserData().openedLevels;
            _difficultyToggles.Add(easyToggle, DifficultyLevel.Easy);
            _difficultyToggles.Add(mediumToggle, DifficultyLevel.Medium);
            _difficultyToggles.Add(hardToggle, DifficultyLevel.Hard);
            
            foreach (var activeToggle in _difficultyToggles.Keys)
                activeToggle.onValueChanged.AddListener(_ => OnToggleValueChanged(activeToggle));
            
            easyToggle.isOn = true;
            unlockButton.interactable = false;
            playButton.onClick.AddListener(OnStartGame);
            unlockButton.onClick.AddListener(OnUnlockLevelClick);
            
            Voodoo.Monetization.LoadAds(()=> unlockButton.interactable = true, null);
        }

        private void OnToggleValueChanged(Toggle toggle)
        {
            _difficultyLevel = _difficultyToggles[toggle];
            playButton.gameObject.SetActive(_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
            unlockButton.gameObject.SetActive(!_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
        }
        
        private void OnStartGame()
        {
            EventsMap.Dispatch(UIEvents.OnStartNewGame, _difficultyLevel);
            Close();
        }

        private void OnUnlockLevelClick()
        {
            Voodoo.Monetization.ShowRewardedAds(()=> UnlockLevel(_difficultyLevel), null, null);
        } 

        private void UnlockLevel(DifficultyLevel difficultyLevel)
        {
            _openedDifficulties.Find(state => state.difficultyLevel == difficultyLevel).isOpened = true;
            playButton.gameObject.SetActive(_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
            unlockButton.gameObject.SetActive(!_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
            Context.GetSystem<IUserSystem>().OpenDifficultyLevel(difficultyLevel);
        }
    }
}