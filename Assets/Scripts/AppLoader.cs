using System.Collections.Generic;
using AppStates;
using Infrastructure;
using UnityEngine;
using Zenject;

public class AppLoader : MonoBehaviour
{
    [Inject] DiContainer _diContainer;
    [Inject] IAppContext _appContext;
    List<ISystem>  _systems;
    private void Start()
    {
        _systems = _diContainer.ResolveAll<ISystem>();
        LoadApp();
    }

    private void LoadApp()
    {
        
        _systems.ForEach(system => system.Initialize());
        _appContext.AppStateMachine.Enter<LobbyState>();
    }

    private void OnDestroy()
    {
        _systems.ForEach(system => system.Dispose());
    }
}
