using System;
using System.Collections.Generic;

namespace Events
{
    public static class EventsMap
    {
        private static readonly Dictionary<string, Delegate> Events = new();

        public static void Subscribe(string eventName, Action listener)
        {
            Events.TryAdd(eventName, null);
            Events[eventName] = (Action)Events[eventName] + listener;
        }

        public static void Unsubscribe(string eventName, Action listener)
        {
            if (!Events.TryGetValue(eventName, out var del)) 
                return;
            
            del = (Action)del - listener;
            if (del == null)
                Events.Remove(eventName);
            else
                Events[eventName] = del;
        }

        public static void Dispatch(string eventName)
        {
            if (Events.TryGetValue(eventName, out var del))
                (del as Action)?.Invoke();
        }
        
        public static void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (Events.TryGetValue(eventName, out var existing))
            {
                if (existing is Action<T> typed)
                {
                    Events[eventName] = typed + listener;
                }
                else
                {
                    throw new InvalidOperationException($"Event '{eventName}' already exists with a different signature.");
                }
            }
            else
            {
                Events[eventName] = listener;
            }
        }

        public static void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (!Events.TryGetValue(eventName, out var del)) 
                return;

            if (del is not Action<T> typed) 
                return;
            
            var updated = typed - listener;
            if (updated == null)
                Events.Remove(eventName);
            else
                Events[eventName] = updated;
        }

        public static void Dispatch<T>(string eventName, T arg)
        {
            if (Events.TryGetValue(eventName, out var del) && del is Action<T> typed)
                typed.Invoke(arg);
        }

        public static void UnsubscribeAll(string eventName)
        {
            Events.Remove(eventName);
        }
    }
}