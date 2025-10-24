using System.Collections.Generic;
using Events;
using Game.Level;
using Infrastructure;
using SaveLoad;
using UI;
using User;
using Zenject;

namespace AppStates
{
    public class LobbyState : IState
    {
        [Inject] IUISystem _uiSystem;
        [Inject] ISaveSystem _saveSystem;
        [Inject] IUserSystem  _userSystem;
        [Inject] IAppContext _appContext;
        public void Enter()
        {
            _uiSystem.ShowView<LobbyBackground>();
            _uiSystem.ShowView<LobbyMainDialog>();
            
            EventsMap.Subscribe(UIEvents.OnPrepareNewGame, OnPrepareNewGame);
            EventsMap.Subscribe(UIEvents.OnShowResults, OnShowResults);
            
            EventsMap.Subscribe<DifficultyLevel>(UIEvents.OnStartNewGame, OnStartNewGame);
        }

        public void Exit()
        {
            _uiSystem.CloseView<LobbyBackground>();
            EventsMap.Unsubscribe(UIEvents.OnPrepareNewGame, OnPrepareNewGame);
            EventsMap.Unsubscribe(UIEvents.OnShowResults, OnShowResults);
            
            EventsMap.Unsubscribe<DifficultyLevel>(UIEvents.OnStartNewGame, OnStartNewGame);
        }
        
        private void OnPrepareNewGame()
        {
            _uiSystem.CloseView<LobbyMainDialog>();
            _uiSystem.ShowView<DifficultySelectDialog, List<LevelState>>(_userSystem.GetUserData().openedLevels);
        }
        
        private void OnShowResults()
        {
            var achievements = _saveSystem.LoadLevelsStat();
            _uiSystem.ShowView<AchievementsDialog, AchievementsData>(achievements);
        }
        
        
        private void OnStartNewGame(DifficultyLevel difficultyLevel)
        {
            _appContext.AppStateMachine.Enter<GameState, DifficultyLevel>(difficultyLevel);
        }
    }
}