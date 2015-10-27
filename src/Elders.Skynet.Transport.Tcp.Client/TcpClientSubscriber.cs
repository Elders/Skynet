using System;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Client
{
    public class TcpClientSubscriber : IObserver<IConnection>
    {
        IConnection connection;

        public void OnCompleted()
        {
            //Console.WriteLine("Client disconected");
        }

        public void OnError(Exception error)
        {
            //Console.WriteLine(error);
        }

        public void Send(BasicMessage message)
        {
            if (connection != null)
                connection.SendMessage(message);
            else
                Console.WriteLine("Connection not established");
        }

        public void OnNext(IConnection connection)
        {
            this.connection = connection;
            connection.Subscribe(new MessageObserver());
        }
    }
}