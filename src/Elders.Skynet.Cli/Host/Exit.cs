using System;
using CommandLine;
using Elders.Skynet.Core;

namespace Elders.Skynet.Cli.Host
{
    [Verb("exit", HelpText = "Exits from the host")]
    public class Exit : IHostCommand
    {
        public void Execute(SkynetClient client)
        {
            client.Disconnect();
            Environment.Exit(0);
        }
    }
}