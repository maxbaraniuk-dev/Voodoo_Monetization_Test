using System;
using Infrastructure.Core;
using UnityEngine;

namespace Logs
{
    public class LogEditor : MonoBehaviour, ILog
    {
        [SerializeField] private LogsConfig logsConfig;

        public void Initialize()
        {
            UnityEngine.Debug.Log("LogEditor initialized");
        }
        
        public void Dispose() { }
        
        public void Debug(Func<string> callback, object sender = null)
        {
            if (logsConfig.GetLogLevel(sender) > LogLevel.Debug)
                return;
            
            UnityEngine.Debug.Log(callback.Invoke());
        }

        public void Error(Func<string> callback)
        {
            UnityEngine.Debug.LogError(callback.Invoke());
        }

        public void Warning(Func<string> callback, object sender = null)
        {
            if (logsConfig.GetLogLevel(sender) > LogLevel.Warning)
                return;
            
            UnityEngine.Debug.LogWarning(callback.Invoke());
        }
    }
}