using Infrastructure.Core;
using UnityEngine;

namespace Logs
{
    [CreateAssetMenu(fileName = "LogsConfig", menuName = "Scriptable Objects/LogsConfig")]
    public class LogsConfig : ScriptableObject
    {
        public LogLevel defaultLogLevel;

        public LogLevel GetLogLevel(object sender)
        {
            return defaultLogLevel;
        }
    }
}
