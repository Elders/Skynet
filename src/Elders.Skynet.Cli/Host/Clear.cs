using System;
using CommandLine;
using Elders.Skynet.Core;

namespace Elders.Skynet.Cli.Host
{
    [Verb("clear", HelpText = "Clears the console.")]
    public class Clear : IHostCommand
    {
        public void Execute(SkynetClient client)
        {
            Console.Clear();
        }
    }
}