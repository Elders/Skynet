using System;
using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;
using System.Linq;
using Elders.Skynet.Cli.TableBuilder;

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
                var table = new Table();
                table.AddRow("Package Name", "Package Location");
                foreach (var item in result.Packages.OrderBy(x => x.Name))
                {
                    table.AddRow(item.Name, item.Location);
                }
                Console.Write(table.Output());
                Console.WriteLine();
            }
            catch (TimeoutException ex)
            {
                log.Error("Operation timeout after 10 seconds.");
            }
        }
    }
}