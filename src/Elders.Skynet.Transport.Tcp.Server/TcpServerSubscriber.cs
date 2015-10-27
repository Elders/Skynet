using System;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Server
{
    public class TcpServerSubscriber : IObserver<IConnection>
    {
        IConnection connection;

        public void OnCompleted()
        {
            //Console.WriteLine("Server stopped");
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
            Console.WriteLine("New connection established");
            connection.Subscribe(new MessageObserver());
        }
    }
}