using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

namespace Infrastructure
{
    public class Context : MonoBehaviour
    {
        public static AppStateMachine AppStateMachine { get; private set; }
        private static List<ISystem> _systems = new();
        private static bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _systems = GetComponents<ISystem>().ToList();
            AppStateMachine = new AppStateMachine();
            _initialized = true;
        }
        
        public static List<ISystem> GetAllSystems() => _systems;
        
        public static T GetSystem<T>() where T : ISystem
        {
            if (!_initialized)
                throw new System.Exception("Context is not initialized yet"); 
                    
            var system = (T) _systems.FirstOrDefault(system => system is T);
            
            if (system == null)
                throw new System.Exception($"There is no system of type {typeof(T)}");
            
            return system;
        }
    }
}
