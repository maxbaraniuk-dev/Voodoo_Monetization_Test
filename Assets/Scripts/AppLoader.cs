using System.Collections.Generic;
using AppStates;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Logs;
using OffersSystem;
using SaveLoad;
using Store;
using UI;
using UnityEngine;
using User;
using VoodooSDK;
using Zenject;

public class AppLoader : MonoBehaviour
{
    [Inject] private DiContainer _diContainer;
    [Inject] private IAppContext _appContext;
    [Inject] private IUserSystem _userSystem;
    [Inject] private ISaveSystem _saveSystem;
    [Inject] private IOffersSystem _offersSystem;
    [Inject] private IStore _store;
    [Inject] private IUISystem _uiSystem;
    [Inject] private ILog _log;
    
    List<ISystem>  _systems;
    private void Start()
    {
        _systems = _diContainer.ResolveAll<ISystem>();
        LoadApp().Forget();
    }

    private async UniTask LoadApp()
    {
        _systems.ForEach(system => system.Initialize());

        var userId = _saveSystem.LoadUserId();
        var initResult = await MonetizationServer.AuthenticateUser(userId);
        if (!initResult.Success)
        {
            _log.Error(() => initResult.Message);
            _uiSystem.ShowView<ErrorView, string>(initResult.Message);
            return;
        }
        _log.Debug(() => "Auth complete");
        var userLoadResult = await _userSystem.LoadUserData();
        if (!userLoadResult.Success)
        {
            _log.Error(() => userLoadResult.Message);
            _uiSystem.ShowView<ErrorView, string>(userLoadResult.Message);
            return;
        }
        _log.Debug(() => "Load user complete");
        var productsLoadResult = await _store.LoadAllProducts();
        
        if (!productsLoadResult.Success)
        {
            _log.Error(() => productsLoadResult.Message);
            _uiSystem.ShowView<ErrorView, string>(productsLoadResult.Message);
            return;
        }
        _log.Debug(() => "Products load complete");
        
        var offersLoadResult = await _offersSystem.LoadOffers();
        if (!offersLoadResult.Success)
        {
            _log.Error(() => offersLoadResult.Message);
            _uiSystem.ShowView<ErrorView, string>(offersLoadResult.Message);
            return;
        }
        
        _log.Debug(() => "App loading complete");
        _appContext.AppStateMachine.Enter<LobbyState>();
    }

    private void OnDestroy()
    {
        _systems.ForEach(system => system.Dispose());
    }
}
