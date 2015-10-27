using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;

namespace Elders.Skynet.Cli.Host
{
    [Verb("run", HelpText = "Starts new instance of the specified package.")]
    public class Run : IHostCommand
    {
        public Run()
        {
        }

        public void Execute(SkynetClient client)
        {
            client.Send(new Core.Contracts.Processes.Run(PackageName, ProcessName));
        }

        [Option('p', "package", Required = true, HelpText = "Specifies the package name.")]
        public string PackageName { get; set; }

        [Option('n', "name", Required = true, HelpText = "Specifies the process name.")]
        public string ProcessName { get; set; }
    }
}