using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Host
{
    public class PackageExplorer : IPackageExplorer
    {
        private List<IExecutableLocator> locators;

        public PackageExplorer(params IExecutableLocator[] locators)
        {
            this.locators = locators.ToList();
        }

        public IExectuable GetExecutable(PackageMeta package)
        {
            var location = string.Empty;
            if (!string.IsNullOrEmpty(package.Exectuable))
                location = package.Exectuable;
            else
                location = package.Location;
            foreach (var item in locators)
            {
                var result = item.Locate(location);
                if (result != null)
                    return result;
            }
            throw new InvalidOperationException("Could not locate executable");
        }

    }
}