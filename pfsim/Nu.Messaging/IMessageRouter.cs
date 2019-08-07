using System;

namespace Nu.Messaging
{
    public interface IMessageRouter
    {
        void Publish<T>(T message, string key);

        void Subscribe<T>(Action<T> callback, string key);

        void SubscribeClass(object obj);
    }
}
