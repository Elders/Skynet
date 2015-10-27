using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;

namespace Elders.Skynet.Cli.Host
{
    [Verb("register-package", HelpText = "Lists all registered packages.")]
    public class RegisterPackage : IHostCommand
    {
        public void Execute(SkynetClient client)
        {
            client.Send(new Core.Contracts.Packages.RegisterPackage(PackageLocation, PackageName));
        }

        [Option('p', "package", Required = true, HelpText = "Specifies the package name.")]
        public string PackageName { get; set; }

        [Option('l', "location", Required = true, HelpText = "Specifies the package location")]
        public string PackageLocation { get; set; }
    }
}