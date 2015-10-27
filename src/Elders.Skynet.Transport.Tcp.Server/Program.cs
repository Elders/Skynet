using System;
using System.Net;
using System.Text;
using System.Threading;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Server
{
    class Program
    {
        static Thread trd;
        static TcpServerSubscriber subscriber;
        static TcpServer server;
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            server = new TcpServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 15672));
            server.Start();
            subscriber = new TcpServerSubscriber();
            server.Subscribe(subscriber);
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
