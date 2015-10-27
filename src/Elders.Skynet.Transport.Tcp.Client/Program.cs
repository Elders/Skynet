using System;
using System.Text;
using System.Threading;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Client
{
    class Program
    {
        static Thread trd;
        static TcpClientSubscriber subscriber;
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var client = new TcpClient("127.0.0.1", 15672);
            subscriber = new TcpClientSubscriber();
            client.Subscribe(subscriber);
            client.Connect(true);

            Console.WriteLine("Client started");
            ReadLine();
            new ManualResetEvent(false).WaitOne(Timeout.Infinite);
        }

        static void ReadLine()
        {
            trd = new Thread(() =>
            {
                while (true)
                {
                    var input = Console.ReadLine();
                    if (subscriber != null)
                    {
                        subscriber.Send(new BasicMessage(Encoding.UTF8.GetBytes(input)));
                    }
                }
            });
            trd.Start();
        }
    }
}
