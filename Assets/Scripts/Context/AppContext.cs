using Events;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Context
{
    public class AppContext : IAppContext, ISystem
    {
        [Inject] DiContainer _diContainer;
        public AppStateMachine AppStateMachine { get; private set; }
        private static bool _initialized;

        public void Initialize()
        {
            AppStateMachine = new AppStateMachine(_diContainer);
            EventsMap.Subscribe(UIEvents.OnErrorConfirm, AppQuit);
        }

        private void AppQuit()
        {
            Application.Quit();
        }

        public void Dispose()
        {
            EventsMap.Unsubscribe(UIEvents.OnErrorConfirm, AppQuit);
        }
    }
}
