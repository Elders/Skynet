using System.Collections.Generic;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Contracts.Processes
{
    public class GetAllProcesses : IMessage<AllProcessesList>
    {

    }

    public class AllProcessesList : IMessage
    {
        public AllProcessesList() { }

        public AllProcessesList(List<SkynetProcessInfo> packages)
        {
            Packages = packages;
        }
        public List<SkynetProcessInfo> Packages { get; set; }
    }
}