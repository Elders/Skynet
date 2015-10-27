using System;

namespace Elders.Skynet.Core.Transport
{
    public interface IClient : IObservable<IConnection>
    {
        void Connect(bool autoReconnect);

        void Connect(bool autoreconnect, TimeSpan autoRecconectTimespan);

        void Disconnect();
    }
}