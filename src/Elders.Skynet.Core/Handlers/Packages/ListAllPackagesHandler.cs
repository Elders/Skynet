using System.Linq;
using Elders.Skynet.Core.Contracts.Packages;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Core.Handlers.Packages
{
    public class GetAllPackagesHandler : IMessageHandler<GetAllPackages, AllPackagesList>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetHost));

        public IPackageRepository PackagesRepository { get; set; }

        public AllPackagesList Handle(Message<GetAllPackages> message)
        {
            return new AllPackagesList(PackagesRepository.GetPackages().ToList());
        }
    }
}
