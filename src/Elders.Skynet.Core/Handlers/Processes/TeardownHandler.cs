using System;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Processes;
using Elders.Skynet.Models;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class TeardownHandler : IMessageHandler<Teardown>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TeardownHandler));

        public T800 Terminator { get; set; }

        public SkynetClient Client { get; set; }

        public void Handle(Message<Teardown> message)
        {
            Terminator.Shutdown();
            log.Info("Stoppping.");
            log.Info("Exiting...");
            Client.Disconnect();
            Environment.Exit(0);
        }
    }
}