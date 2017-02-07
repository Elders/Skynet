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

        [Option('i', "instance", Required = true, HelpText = "Specifies the unique process instance name.")]
        public string ProcessName { get; set; }
    }
}