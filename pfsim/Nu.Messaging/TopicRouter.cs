using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nu.Messaging
{
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
            Subscribe(t, callback, key);
        }

        private void Subscribe(Type t, object callback, string key)
        {
            if (!subscriptions.ContainsKey(t))
            {
                subscriptions[t] = new List<TopicSubscription>();
            }
            subscriptions[t].Add(new TopicSubscription(callback, key));
        }

        public void SubscribeClass(object obj)
        {
            var methods = obj.GetType().GetMethods();
            foreach (var m in methods)
            {
                var att = m.GetCustomAttributes(typeof(SubscriptionAttribute), false);
                if (!att.Any())
                {
                    continue;
                }
                var subAtt = (SubscriptionAttribute)att.First();
                var actionType = typeof(Action<>).MakeGenericType(new Type[] { subAtt.Type });

                var del = m.CreateDelegate(Expression.GetDelegateType(m.GetParameters().Select(x => x.ParameterType).Concat(new[] { m.ReturnType }).ToArray()), obj);
                Subscribe(subAtt.Type, del, subAtt.RoutingKey);
            }
        }
    }
}
