using System.Linq;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class GetAllProcessesHandler : IMessageHandler<GetAllProcesses, AllProcessesList>
    {
        public SkynetProcessRegistry Registry { get; set; }

        public AllProcessesList Handle(Message<GetAllProcesses> message)
        {
            return new AllProcessesList(Registry.GetAllProcesses().ToList());
        }
    }
}
