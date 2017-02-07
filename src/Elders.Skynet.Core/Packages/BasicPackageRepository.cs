using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elders.Skynet.Core.Packages
{
    public class BasicFilePackageRepository : IPackageRepository
    {
        private const string BaseDirectory = "packages";

        public BasicFilePackageRepository()
        {
            if (!Directory.Exists(BaseDirectory))
                Directory.CreateDirectory(BaseDirectory);
        }

        public void DeletePackage(string name)
        {
            Directory.Delete(Path.Combine(BaseDirectory, name), true);
        }

        public PackageMeta GetPackage(string name)
        {
            var files = Directory.GetFiles(Path.Combine(BaseDirectory, name));
            return new PackageMeta(Path.Combine(BaseDirectory, name), name, files.ToList());
        }

        public IEnumerable<PackageMeta> GetPackages()
        {
            foreach (var dir in Directory.GetDirectories(BaseDirectory))
            {
                yield return GetPackage(new DirectoryInfo(dir).Name);
            }
        }

        public void RegisterPackage(string location)
        {
            RegisterPackage(location, location.GetHashCode().ToString());
        }

        public void RegisterPackage(string location, string packageName)
        {
            var packageDir = Path.Combine(BaseDirectory, packageName);
            if (!Directory.Exists(packageDir))
                Directory.CreateDirectory(packageDir);

            if (File.Exists(location))
            {
                var fileInfo = new FileInfo(location);
                File.Copy(location, new FileInfo(Path.Combine(packageDir, fileInfo.Name)).FullName, true);
            }
            else if (Directory.Exists(location))
            {
                foreach (var file in Directory.GetFiles(location))
                {
                    var fileInfo = new FileInfo(file);
                    File.Copy(fileInfo.FullName, Path.Combine(packageDir, fileInfo.Name), true);
                }
            }

        }

        public void RegisterPackage(byte[] packageBytes, string filename, string packageName)
        {
            var packageDir = Path.Combine(BaseDirectory, packageName);
            if (!Directory.Exists(packageDir))
                Directory.CreateDirectory(packageDir);

            File.WriteAllBytes(Path.Combine(packageDir, filename), packageBytes);
        }
    }
}
