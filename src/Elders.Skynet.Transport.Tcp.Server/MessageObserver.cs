using System;
using System.Text;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Server
{
    public class MessageObserver : IObserver<BasicMessage>
    {
        public void OnCompleted()
        {
            //Console.WriteLine("Connection closed");
        }

        public void OnError(Exception error)
        {
            //Console.WriteLine(error);
        }

        public void OnNext(BasicMessage value)
        {
            Console.WriteLine(Encoding.UTF8.GetString(value.Body));
        }
    }
}