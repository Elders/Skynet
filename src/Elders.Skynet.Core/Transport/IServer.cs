using System;

namespace Elders.Skynet.Core.Transport
{
    public interface IServer : IObservable<IConnection>
    {
        void Start();

        void Stop();
    }
}