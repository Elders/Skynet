using System;

namespace Elders.Skynet.Core.Transport
{
    public interface IConnection : IObservable<BasicMessage>
    {
        bool Connected { get; }

        void Close();

        void SendMessage(BasicMessage message);

        string ToString();
    }
}
