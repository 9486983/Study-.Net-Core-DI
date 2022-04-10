using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Study.Core
{
    public static class MessageManager
    {
        private static readonly ConcurrentDictionary<string, Action<object>> MessageEvents = new ConcurrentDictionary<string, Action<object>>();
        public static void Subscribe(string subject, Action<object> action)
        {
            MessageEvents.TryAdd(subject, action);
        }

        public static void Unsubscribe(string subject)
        {
            MessageEvents.TryRemove(subject, out Action<object> value);
        }

        public static void Publish(string key, object value)
        {
            if (MessageEvents.TryGetValue(key, out Action<object> action) && action != null)
            {
                action?.Invoke(value);
            }
        }
    }
}
