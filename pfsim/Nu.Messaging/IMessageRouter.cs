using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.Messaging
{
    public interface IMessageRouter
    {
        void Publish<T>(T message, string key);

        void Subscribe<T>(Action<T> callback, string key);
    }
    public class TopicSubscription
    {
        public string[] RoutingKey { get; private set; }

        public object Callback { get; private set; }

        public TopicSubscription(object callback, string routingKey)
        {
            RoutingKey = routingKey.Split('.');
            Callback = callback;
        }

        public bool TestRoutingKey(string key)
        {
            var keyParts = key.Split('.');
            var i = 0;
            for (int t = 0; t < keyParts.Length; t++)
            {
                if (keyParts[t] != RoutingKey[i] && RoutingKey[i] != "*" && RoutingKey[i] != "#")
                {
                    return false;
                }
                if (RoutingKey[i] != "#" || keyParts[t] == RoutingKey[i])
                {
                    i++;
                }
            }
            return true;
        }
    }



    public class TopicRouter : IMessageRouter
    {
        Dictionary<Type, List<TopicSubscription>> subscriptions = new Dictionary<Type, List<TopicSubscription>>();

        public void Publish<T>(T message, string key)
        {
            var t = typeof(T);
            if (!subscriptions.ContainsKey(t))
            {
                return;
            }
            subscriptions[t].Where(x => x.TestRoutingKey(key)).ToList().ForEach(x => (x.Callback as Action<T>)(message));
        }

        public void Subscribe<T>(Action<T> callback, string key)
        {
            var t = typeof(T);
            if (!subscriptions.ContainsKey(t))
            {
                subscriptions[t] = new List<TopicSubscription>();
            }
            subscriptions[t].Add(new TopicSubscription(callback, key));
        }
    }
}
