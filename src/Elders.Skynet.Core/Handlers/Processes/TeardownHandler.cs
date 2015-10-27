using System;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class TeardownHandler : IMessageHandler<Teardown>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TeardownHandler));

        public ISkynetRunner Runner { get; set; }

        public SkynetClient Client { get; set; }

        public void Handle(Message<Teardown> message)
        {
            Runner.Stop();
            log.Info("Stoppping.");
            log.Info("Exiting...");
            Client.Disconnect();
            Environment.Exit(1);
        }
    }
}