using System;
using System.Collections.Generic;
using AppStates;
using Zenject;

namespace Infrastructure
{
    public class AppStateMachine
    {
        DiContainer _diContainer;
        readonly Dictionary<Type, IState> _states;
        IState _activeState;

        public AppStateMachine(DiContainer  diContainer)
        {
            _diContainer = diContainer;
            _states = new()
            {
                [typeof(LobbyState)] = new LobbyState(),
                [typeof(GameState)] = new GameState()
            };
            foreach (var state in _states.Values)
                _diContainer.Inject(state);
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IPayLoadedState<TPayLoad>
        {
            TState state = ChangeState<TState>();
            state.SetPayload(payload);
            state.Enter();
        }

        public IState CurrentState() => _activeState;

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }
        TState GetState<TState>() where TState : class, IState => 
            _states[typeof(TState)] as TState;
        }
}