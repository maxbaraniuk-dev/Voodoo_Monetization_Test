using Zenject;

namespace Infrastructure
{
    public class AppContext : IAppContext, ISystem
    {
        [Inject] DiContainer _diContainer;
        public AppStateMachine AppStateMachine { get; private set; }
        private static bool _initialized;

        public void Initialize()
        {
            AppStateMachine = new AppStateMachine(_diContainer);
        }

        public void Dispose()
        {
            
        }
    }
}
