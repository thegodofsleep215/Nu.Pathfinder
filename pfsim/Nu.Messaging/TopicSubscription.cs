namespace Nu.Messaging
{
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
}
