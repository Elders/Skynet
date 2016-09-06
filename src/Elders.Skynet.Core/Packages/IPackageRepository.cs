using System.Collections.Generic;

namespace Elders.Skynet.Core.Packages
{
    public interface IPackageRepository
    {
        void RegisterPackage(string location);
        void RegisterPackage(string location, string packageName);
        void RegisterPackage(byte[] packageBytes, string filename, string packageName);
        void DeletePackage(string name);
        PackageMeta GetPackage(string name);
        IEnumerable<PackageMeta> GetPackages();
    }
}
