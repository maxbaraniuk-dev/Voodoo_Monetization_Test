using Game;
using Logs;
using UI.Core;
using UnityEngine;
using Zenject;

namespace Core
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
    {
        [SerializeField] private UIConfig uiConfig;
        [SerializeField] private LogsConfig logsConfig;
        [SerializeField] private GameConfig gameConfig;
        public override void InstallBindings()
        {
            Container.BindInstance(uiConfig);
            Container.BindInstance(logsConfig);
            Container.BindInstance(gameConfig);
        }
    }
}