using Game;
using Infrastructure;
using Logs;
using Offers;
using SaveLoad;
using Store;
using UI;
using User;
using Zenject;

public class AppInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LogEditor>().AsSingle().NonLazy();
        Container.Bind<ILog>().To<LogEditor>().FromResolve();
        Container.Bind<ISystem>().To<LogEditor>().FromResolve();
            
        Container.Bind<AppContext>().AsSingle().NonLazy();
        Container.Bind<IAppContext>().To<AppContext>().FromResolve();
        Container.Bind<ISystem>().To<AppContext>().FromResolve();
            
        Container.Bind<SaveSystem>().AsSingle().NonLazy();
        Container.Bind<ISaveSystem>().To<SaveSystem>().FromResolve();
        Container.Bind<ISystem>().To<SaveSystem>().FromResolve();
            
        Container.Bind<UserSystem>().AsSingle().NonLazy();
        Container.Bind<IUserSystem>().To<UserSystem>().FromResolve();
        Container.Bind<ISystem>().To<UserSystem>().FromResolve();
            
        Container.Bind<UISystem>().AsSingle().NonLazy();
        Container.Bind<IUISystem>().To<UISystem>().FromResolve();
        Container.Bind<ISystem>().To<UISystem>().FromResolve();
            
        Container.Bind<GameSystem>().AsSingle().NonLazy();
        Container.Bind<IGameSystem>().To<GameSystem>().FromResolve();
        Container.Bind<ISystem>().To<GameSystem>().FromResolve();
            
        Container.Bind<OffersSystem>().AsSingle().NonLazy();
        Container.Bind<IOffersSystem>().To<OffersSystem>().FromResolve();
        Container.Bind<ISystem>().To<OffersSystem>().FromResolve();
        
        Container.Bind<StoreProvider>().AsSingle().NonLazy();
        Container.Bind<IStore>().To<StoreProvider>().FromResolve();
        Container.Bind<ISystem>().To<StoreProvider>().FromResolve();
            
    }
}