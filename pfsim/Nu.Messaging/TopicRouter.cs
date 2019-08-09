using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nu.Messaging
{
    public class TopicRouter : IMessageRouter
    {
        Dictionary<Type, List<TopicSubscription>> subscriptions = new Dictionary<Type, List<TopicSubscription>>();
        Dictionary<Type, Dictionary<Type, object>> remoteCalls = new Dictionary<Type, Dictionary<Type, object>>();

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
            foreach (var mInfo in methods)
            {
                if (!TrySubscription(mInfo))
                {
                    TryRemoteCall(mInfo);
                }
            }

            bool TrySubscription(MethodInfo m)
            {
                var att = m.GetCustomAttributes(typeof(SubscriptionAttribute), false);
                var p = m.GetParameters();
                if (!att.Any() || p.Length != 1)
                {
                    return false;
                }
                var subAtt = (SubscriptionAttribute)att.First();
                var pType = p[0].ParameterType;
                var actionType = typeof(Action<>).MakeGenericType(new Type[] { pType });

                var del = m.CreateDelegate(Expression.GetDelegateType(m.GetParameters().Select(x => x.ParameterType).Concat(new[] { m.ReturnType }).ToArray()), obj);
                Subscribe(pType, del, subAtt.RoutingKey);
                return true;
            }

            void TryRemoteCall(MethodInfo m)
            {
                var att = m.GetCustomAttributes(typeof(RemoteCallAttribute), false);
                var p = m.GetParameters();
                if (!att.Any() || p.Length != 1 || m.ReturnType == typeof(void))
                {
                    return;
                }
                var rcAtt = (RemoteCallAttribute)att.First();
                var pType = p[0].ParameterType;
                var actionType = typeof(Func<>).MakeGenericType(new Type[] { pType });

                var del = m.CreateDelegate(Expression.GetDelegateType(m.GetParameters().Select(x => x.ParameterType).Concat(new[] { m.ReturnType }).ToArray()), obj);
                RegisterRemoteCall(pType, m.ReturnType, del);
            }
        }

        private void RegisterRemoteCall(Type parameter, Type ret, object callback)
        {
            if (!remoteCalls.ContainsKey(parameter))
            {
                remoteCalls[parameter] = new Dictionary<Type, object>();
            }
            if (remoteCalls[parameter].ContainsKey(ret))
            {
                throw new Exception($"A remote call with parameter type {parameter} and the return type of {ret} has already been registered");
            }
            remoteCalls[parameter][ret] = callback;
        }

        public void RegisterRemoteCall<T, TR>(Func<T, TR> callback)
        {
            RegisterRemoteCall(typeof(T), typeof(TR), callback);
        }

        public TR RemoteCall<T, TR>(T message)
        {
            if (!remoteCalls.ContainsKey(typeof(T)) || !remoteCalls[typeof(T)].ContainsKey(typeof(TR)))
            {
                throw new Exception($"No callback for parameter type {typeof(T)} and return type {typeof(TR)} has been registered");
            }
            var c = (Func<T, TR>)remoteCalls[typeof(T)][typeof(TR)];
            return c(message);
        }

    }
}
