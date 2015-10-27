using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class HeartBeatHandler : IMessageHandler<Heartbeat>
    {
        public SkynetProcessRegistry Registry { get; set; }

        public void Handle(Message<Heartbeat> message)
        {
            Registry.Heartbeat(message.Context.ConnectionId, message.Payload.ProcessId);
        }
    }
}
