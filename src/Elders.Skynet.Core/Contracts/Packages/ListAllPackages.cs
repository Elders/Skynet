using System.Collections.Generic;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Core.Contracts.Packages
{
    public class AllPackagesList : IMessage
    {
        public AllPackagesList() { }

        public AllPackagesList(List<PackageMeta> packages)
        {
            Packages = packages;
        }
        public List<PackageMeta> Packages { get; set; }
    }


    public class GetAllPackages : IMessage<AllPackagesList>
    {

    }
}