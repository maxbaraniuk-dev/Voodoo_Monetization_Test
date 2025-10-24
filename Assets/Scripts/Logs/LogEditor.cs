using System;
using Infrastructure;
using Infrastructure.Core;
using Zenject;

namespace Logs
{
    public class LogEditor : ILog, ISystem
    {
        [Inject] private LogsConfig _logsConfig;

        public void Initialize()
        {
            UnityEngine.Debug.Log("LogEditor initialized");
        }
        
        public void Dispose() { }
        
        public void Debug(Func<string> callback, object sender = null)
        {
            if (_logsConfig.GetLogLevel(sender) > LogLevel.Debug)
                return;
            
            UnityEngine.Debug.Log(callback.Invoke());
        }

        public void Error(Func<string> callback)
        {
            UnityEngine.Debug.LogError(callback.Invoke());
        }

        public void Warning(Func<string> callback, object sender = null)
        {
            if (_logsConfig.GetLogLevel(sender) > LogLevel.Warning)
                return;
            
            UnityEngine.Debug.LogWarning(callback.Invoke());
        }
    }
}