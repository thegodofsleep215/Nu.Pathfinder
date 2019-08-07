using System;

namespace Nu.Messaging
{
    public class SubscriptionAttribute : Attribute
    {
        public SubscriptionAttribute(Type type, string routingKey)
        {
            Type = type;
            RoutingKey = routingKey;
        }

        public Type Type { get; }
        public string RoutingKey { get; }
    }
}
