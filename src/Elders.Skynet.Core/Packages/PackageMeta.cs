namespace Elders.Skynet.Core.Packages
{
    public class PackageMeta
    {
        public PackageMeta(string location, string name)
        {
            Location = location;
            Name = name;
        }

        public string Location { get; private set; }

        public string Name { get; private set; }

        public string Exectuable { get; private set; }
    }
}