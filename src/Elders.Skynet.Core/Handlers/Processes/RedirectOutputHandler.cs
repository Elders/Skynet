using System.Linq;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Contracts.System;
using Elders.Skynet.Core.Output;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class RedirectOutputHandler : IMessageHandler<RedirectOutput>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetHost));

        public SkynetProcessRegistry Registry { get; set; }

        public void Handle(Message<RedirectOutput> message)
        {
            var process = Registry.GetAllProcesses().SingleOrDefault(x => x.Name == message.Payload.ProcessName);
            if (process != null)
                Registry.SubscrbeForOutput(process, new StringObserver(x => message.Context.Respond(new WriteToOutput(x))));
            else
                log.ErrorFormat("Can not find process with name '{0}'", message.Payload.ProcessName);
        }
    }
}
