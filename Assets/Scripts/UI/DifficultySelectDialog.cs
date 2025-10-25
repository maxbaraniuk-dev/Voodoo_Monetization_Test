using System.Collections.Generic;
using Events;
using Game.Level;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace UI
{
    public class DifficultySelectDialog : BaseDataView<List<LevelState>>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button unlockButton;
        [SerializeField] private Toggle easyToggle;
        [SerializeField] private Toggle mediumToggle;
        [SerializeField] private Toggle hardToggle;

        private readonly Dictionary<Toggle, DifficultyLevel> _difficultyToggles = new();
        
        private DifficultyLevel _difficultyLevel;
        private List<LevelState> _openedDifficulties;
        
        public override void Show(List<LevelState> difficulties)
        {
            _openedDifficulties = difficulties;
            _difficultyToggles.Add(easyToggle, DifficultyLevel.Easy);
            _difficultyToggles.Add(mediumToggle, DifficultyLevel.Medium);
            _difficultyToggles.Add(hardToggle, DifficultyLevel.Hard);
            
            foreach (var activeToggle in _difficultyToggles.Keys)
                activeToggle.onValueChanged.AddListener(_ => OnToggleValueChanged(activeToggle));
            
            easyToggle.isOn = true;
            playButton.onClick.AddListener(OnStartGame);
            unlockButton.onClick.AddListener(OnUnlockLevelClick);
            UpdateButtons();
        }

        public override void UpdateView(List<LevelState> difficulties)
        {
            _openedDifficulties = difficulties;
            UpdateButtons();
        }

        private void OnToggleValueChanged(Toggle toggle)
        {
            _difficultyLevel = _difficultyToggles[toggle];
            UpdateButtons();
        }
        
        private void OnStartGame()
        {
            EventsMap.Dispatch(UIEvents.OnStartNewGame, _difficultyLevel);
            Close();
        }

        private void OnUnlockLevelClick()
        {
            EventsMap.Dispatch(GameEvents.TryUnlockDifficultyLevel, _difficultyLevel);
        } 

        private void UnlockLevel(DifficultyLevel difficultyLevel)
        {
            _openedDifficulties.Find(state => state.difficultyLevel == difficultyLevel).isOpened = true;
            UpdateButtons();
        }
        
        private void UpdateButtons()
        {
            playButton.gameObject.SetActive(_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
            unlockButton.gameObject.SetActive(!_openedDifficulties.Find(state => state.difficultyLevel == _difficultyLevel).isOpened);
        }
    }
}