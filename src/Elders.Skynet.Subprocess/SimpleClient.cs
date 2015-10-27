using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Output;
using Elders.Skynet.Transport.Tcp;

namespace Elders.Skynet.Subprocess
{

    [Verb("simple-client")]
    public class SimpleClient : ICliCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SimpleClient));

        public void Execute()
        {
            //   throw new Exception("aasd");

            var tcpClient = new TcpClient(ServerLocation, ServerPort);
            var skynet = new SkynetClient("Client", tcpClient, null);
            skynet.Connect(true, false, TimeSpan.FromSeconds(60));
            try
            {
                var assembly = Assembly.LoadFrom(File);

                assembly.EntryPoint.Invoke(null, new object[] { new string[] { "args" } });
                new ManualResetEvent(false).WaitOne(1000);
            }
            catch (Exception ex)
            {
                log.Error("Failed to start process", ex);

                Environment.Exit(0);
            }

            //  runner.Start("");

            Environment.Exit(1);

        }

        [Value(0)]
        public string File { get; set; }

        [Value(1)]
        public string ServerLocation { get; set; }

        [Value(2)]
        public int ServerPort { get; set; }

    }
}