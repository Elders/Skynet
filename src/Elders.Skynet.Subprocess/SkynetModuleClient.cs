using System;
using System.Reflection;
using System.Threading;
using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Transport.Tcp;
using System.Linq;
using Elders.Skynet.Core.Processes;
using Elders.Skynet.Models;

namespace Elders.Skynet.Subprocess
{
    [Verb("skynet-client")]
    public class SkynetModuleClient : ICliCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SimpleClient));

        public void Execute()
        {
            try
            {
                var assembly = Assembly.LoadFrom(File);
                var moduleType = typeof(Models.T800);
                var module = assembly.GetTypes().SingleOrDefault(x => moduleType.IsAssignableFrom(x));
                var terminator = (Activator.CreateInstance(module) as Models.T800);
                var tcpClient = new TcpClient(ServerLocation, ServerPort);
                var skynet = new SkynetClient("Client", tcpClient, terminator);
                skynet.Connect(true, false, TimeSpan.FromSeconds(60));
                terminator.PowerUp("nqkuv film");
                new ManualResetEvent(false).WaitOne(1000);
            }
            catch (Exception ex)
            {
                log.Error("Failed to start process", ex);

                Environment.Exit(0);
            }

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