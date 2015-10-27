using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts.Processes;

namespace Elders.Skynet.Cli.Host
{
    [Verb("shutdown", HelpText = "Stops a process.")]
    public class SendShutdown : IHostCommand
    {
        public void Execute(SkynetClient client)
        {
            client.Send(new Shutdown(ProcessName));
        }

        [Option('n', "name", Required = true, HelpText = "Specifies the process name.")]
        public string ProcessName { get; set; }
    }
}