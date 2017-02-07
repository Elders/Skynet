using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Host
{
    public class PackageExplorer : IPackageExplorer
    {
        private List<IPackageLocator> locators;

        public PackageExplorer(params IPackageLocator[] locators)
        {
            this.locators = locators.ToList();
        }

        public IExectuable GetExecutable(PackageMeta package)
        {
            foreach (var item in locators)
            {
                var result = item.GetExecutable(package);
                if (result != null)
                    return result;
            }
            throw new InvalidOperationException("Could not locate executable");
        }

        public Dictionary<string, string> GetMetadata(PackageMeta package)
        {
            foreach (var item in locators.Where(x => x.Recognise(package)))
            {
                var result = item.GetMetadata(package);
                if (result != null)
                    return result;
            }
            return new Dictionary<string, string>();
        }
    }
}