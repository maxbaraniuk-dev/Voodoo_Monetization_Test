using System.Collections.Generic;
using AppStates;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Offers;
using SaveLoad;
using Store;
using UnityEngine;
using User;
using VoodooSDK;
using Zenject;

public class AppLoader : MonoBehaviour
{
    [Inject] DiContainer _diContainer;
    [Inject] IAppContext _appContext;
    [Inject] IUserSystem _userSystem;
    [Inject] ISaveSystem _saveSystem;
    [Inject] IOffersSystem _offersSystem;
    [Inject] IStore _store;
    
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
        await MonetizationSDK.AuthenticateUser(userId);
        await _userSystem.LoadUserData();
        await _store.LoadAllProducts();
        await _offersSystem.LoadOffers();
        
        _appContext.AppStateMachine.Enter<LobbyState>();
    }

    private void OnDestroy()
    {
        _systems.ForEach(system => system.Dispose());
    }
}
