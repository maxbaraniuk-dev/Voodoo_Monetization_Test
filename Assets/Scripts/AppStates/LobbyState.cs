using Events;
using Game.Level;
using Infrastructure;
using SaveLoad;
using UI;
using User;

namespace AppStates
{
    public class LobbyState : IState
    {
        public void Enter()
        {
            Context.GetSystem<IUISystem>().ShowView<LobbyBackground>();
            Context.GetSystem<IUISystem>().ShowView<LobbyMainDialog>();
            
            EventsMap.Subscribe(UIEvents.OnPrepareNewGame, OnPrepareNewGame);
            EventsMap.Subscribe(UIEvents.OnShowResults, OnShowResults);
            
            EventsMap.Subscribe<DifficultyLevel>(UIEvents.OnStartNewGame, OnStartNewGame);
        }

        public void Exit()
        {
            Context.GetSystem<IUISystem>().CloseView<LobbyBackground>();
            EventsMap.Unsubscribe(UIEvents.OnPrepareNewGame, OnPrepareNewGame);
            EventsMap.Unsubscribe(UIEvents.OnShowResults, OnShowResults);
            
            EventsMap.Unsubscribe<DifficultyLevel>(UIEvents.OnStartNewGame, OnStartNewGame);
        }
        
        private void OnPrepareNewGame()
        {
            Context.GetSystem<IUISystem>().CloseView<LobbyMainDialog>();
            Context.GetSystem<IUISystem>().ShowView<DifficultySelectDialog>();
        }
        
        private void OnShowResults()
        {
            var achievements = Context.GetSystem<ISaveSystem>().LoadLevelsStat();
            Context.GetSystem<IUISystem>().ShowView<AchievementsDialog, AchievementsData>(achievements);
        }
        
        
        private void OnStartNewGame(DifficultyLevel difficultyLevel)
        {
            Context.AppStateMachine.Enter<GameState, DifficultyLevel>(difficultyLevel);
        }
    }
}