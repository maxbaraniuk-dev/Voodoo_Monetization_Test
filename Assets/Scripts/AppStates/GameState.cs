using Events;
using Game;
using Game.Level;
using Infrastructure;
using SaveLoad;
using UI;
using Zenject;

namespace AppStates
{
    public class GameState : IPayLoadedState<DifficultyLevel>
    {
        [Inject] IGameSystem _gameSystem;
        [Inject] IUISystem _uiSystem;
        [Inject] IAppContext _appContext;
        
        private DifficultyLevel _difficultyLevel;
        public void SetPayload(DifficultyLevel payload)
        {
            _difficultyLevel = payload;
        }

        public void Enter()
        {
            _uiSystem.ShowView<GameUI>();
            _gameSystem.StartNewGame(_difficultyLevel);
            EventsMap.Subscribe(UIEvents.OnBackToMenu, OnBackToMenu);
        }

        private void OnBackToMenu()
        {
            _appContext.AppStateMachine.Enter<LobbyState>();
        }

        public void Exit()
        {
            _gameSystem.ExitGame();
            EventsMap.Unsubscribe(UIEvents.OnBackToMenu, OnBackToMenu);
        }
    }
}