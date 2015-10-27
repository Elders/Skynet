using System.Linq;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class ShutdownHandler : IMessageHandler<Shutdown>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ShutdownHandler));

        public SkynetProcessRegistry Registry { get; set; }

        public SkynetHost Skynet { get; set; }

        public void Handle(Message<Shutdown> message)
        {
            var process = Registry.GetAllProcesses().Where(x => x.Name == message.Payload.ProcessName).FirstOrDefault();
            if (process != null)
                Skynet.Send(process.HeartbeatOrigin, new Teardown());
        }
    }
}
