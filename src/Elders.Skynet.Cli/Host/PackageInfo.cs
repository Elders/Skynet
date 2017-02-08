using System;
using CommandLine;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts;
using System.Linq;
using Elders.Skynet.Cli.TableBuilder;

namespace Elders.Skynet.Cli.Host
{
    [Verb("package-info", HelpText = "Gets package info.")]
    public class PackageInfo : IHostCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PackageInfo));

        [Option('p', "package", Required = true, HelpText = "Specifies the package name.")]
        public string PackageName { get; set; }

        public void Execute(SkynetClient client)
        {
            try
            {
                var result = client.Send(new Core.Contracts.Packages.GetPackageInfo(PackageName), TimeSpan.FromSeconds(10));
                Console.WriteLine("Pacakge Name: \t {0}", result.PackageName);
                Console.WriteLine();
                Console.WriteLine("Location: \t {0}", result.Location);
                Console.WriteLine();
                Console.WriteLine("Files: \t");
                foreach (var item in result.Files.OrderBy(x => x))
                {
                    Console.WriteLine(" \t \t " + item);
                }
                Console.WriteLine();
                if (result.Metadata != null && result.Metadata.Any())
                {
                    Console.WriteLine("Metadata: \t");
                    var table = new Table();
                    foreach (var item in result.Metadata)
                    {
                        table.AddRow(item.Key, item.Value);
                        //Console.WriteLine("{0}: \t {1}", item.Key, item.Value);
                    }
                    Console.WriteLine(table.Output());
                    Console.WriteLine();
                }
            }
            catch (TimeoutException ex)
            {
                log.Error("Operation timeout after 10 seconds.");
            }
        }
    }
}