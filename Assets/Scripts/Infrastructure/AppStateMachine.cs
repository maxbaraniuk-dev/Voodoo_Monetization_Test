using System;
using System.Collections.Generic;
using AppStates;

namespace Infrastructure
{
  public class AppStateMachine
  {
    readonly Dictionary<Type, IState> _states = new()
    {
        [typeof(LobbyState)] = new LobbyState(),
        [typeof(GameState)] = new GameState()
    };
    
    IState _activeState;

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