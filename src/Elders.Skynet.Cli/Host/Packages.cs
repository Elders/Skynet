using System;
using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;
using System.Linq;

namespace Elders.Skynet.Cli.Host
{
    [Verb("packages", HelpText = "Lists all registered packages.")]
    public class Packages : IHostCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Packages));

        public void Execute(SkynetClient client)
        {
            try
            {
                var result = client.Send(new Core.Contracts.Packages.GetAllPackages(), TimeSpan.FromSeconds(10));
                Console.WriteLine("{0} \t {1}", "Name", "Executable");
                foreach (var item in result.Packages)
                {
                    Console.WriteLine("{0} \t {1}", item.Name, item.Location);
                }
            }
            catch (TimeoutException ex)
            {
                log.Error("Operation timeout after 10 seconds.");
            }
        }
    }
}