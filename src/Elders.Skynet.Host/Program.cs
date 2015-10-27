using System.Net;
using System.Threading;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Transport.Tcp;

namespace Elders.Skynet.Host
{
    class Program
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {

            log4net.Config.XmlConfigurator.Configure();
            IServer server = new TcpServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25672));
            var skynet = new SkynetHost("Skynet", server, new PackageExplorer(new SimpleExecutableLocator("127.0.0.1", 25672)));
            skynet.Start();
            new ManualResetEvent(false).WaitOne(Timeout.Infinite);
        }
    }
}
