using System;

namespace Nu.Messaging
{
    public class SubscriptionAttribute : Attribute
    {
        public SubscriptionAttribute(string routingKey)
        {
            RoutingKey = routingKey;
        }

        public string RoutingKey { get; }
    }
}
