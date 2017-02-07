using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;

namespace Elders.Skynet.Cli.Host
{
    [Verb("redirect-output", HelpText = "Redirects the output from a specified process.")]
    public class RedirectOutput : IHostCommand
    {
        public void Execute(SkynetClient client)
        {
            client.Send(new Core.Contracts.Processes.RedirectOutput(Client));
        }

        [Option('i', "instance", Required = true, HelpText = "Specifies the unique process instance name.")]
        public string Client { get; set; }
    }
}