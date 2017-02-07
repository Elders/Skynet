using System.Collections.Generic;

namespace Elders.Skynet.Core.Packages
{
    public class PackageMeta
    {
        public PackageMeta(string location, string name, List<string> files)
        {
            Location = location;
            Name = name;
            Files = files;
        }

        public string Location { get; private set; }

        public string Name { get; private set; }

        public List<string> Files { get; private set; }
    }
}