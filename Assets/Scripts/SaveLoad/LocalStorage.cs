using Infrastructure;
using Logs;
using UnityEngine;

namespace SaveLoad
{
    public static class LocalStorage
    {
        public static void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }
        
        public static T Load<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Context.GetSystem<ILog>().Debug(() => $"Key {key} not found");
                return default;
            }

            var json = PlayerPrefs.GetString(key);
            
            var data = JsonUtility.FromJson<T>(json);
            if (data == null)
                Context.GetSystem<ILog>().Error(() => $"Failed to load {key}");
            
            return JsonUtility.FromJson<T>(json);
        }
    }
}