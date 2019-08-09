using System;

namespace Nu.Messaging
{
    public interface IMessageRouter
    {
        void Publish<T>(T message, string key);

        TR RemoteCall<T, TR>(T message);

        void Subscribe<T>(Action<T> callback, string key);

        void RegisterRemoteCall<T, TR>(Func<T, TR> callback);

        void SubscribeClass(object obj);
    }
}
