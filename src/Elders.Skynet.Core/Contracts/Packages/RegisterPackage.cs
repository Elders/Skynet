namespace Elders.Skynet.Core.Contracts.Packages
{
    public class RegisterPackage : IMessage
    {
        public RegisterPackage() { }

        public RegisterPackage(string location, string name)
        {
            Location = location;
            Name = name;
        }

        public string Location { get; set; }

        public string Name { get; set; }
    }
}