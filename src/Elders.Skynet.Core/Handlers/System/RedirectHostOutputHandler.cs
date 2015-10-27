using Elders.Skynet.Core.Contracts.System;
using Elders.Skynet.Core.Output;

namespace Elders.Skynet.Core.Handlers.System
{
    public class RedirectHostOutputHandler : IMessageHandler<RedirectHostOutput>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetHost));

        public IOutput Output { get; set; }

        public void Handle(Message<RedirectHostOutput> message)
        {
            Output.Subscribe(new StringObserver(x => message.Context.Respond(new WriteToOutput(x))));
        }
    }
}
