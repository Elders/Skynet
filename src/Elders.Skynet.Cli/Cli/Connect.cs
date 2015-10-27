using System;
using System.Linq;
using CommandLine;
using Elders.Skynet.Cli.Host;
using Elders.Skynet.Core;
using Elders.Skynet.Transport.Tcp;

namespace Elders.Skynet.Cli
{
    [Verb("connect", HelpText = "Connects to Skynet Host.")]
    public class Connect : ICliCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Connect));

        public Connect()
        {
            Port = 25672;
            Address = "127.0.0.1";
        }

        public void Execute()
        {
            var client = new TcpClient(Address, Port);
            var skynet = new SkynetClient("Skynet Command Line Interface", client);

            try
            {
                skynet.Connect(true, true, TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                log.Error("Failed to connect", ex);
            }
            var commands = typeof(Program).Assembly.GetTypes().Where(x => typeof(IHostCommand).IsAssignableFrom(x));

            while (true)
            {
                Console.WriteLine();
                Console.Write("Skynet|>");
                var message = Console.ReadLine();
                Console.WriteLine();
                var cmdLine = message.Split(';');
                foreach (var command in cmdLine)
                {
                    try
                    {
                        var result = Parser.Default.ParseArguments(command.Split(' '), commands.ToArray());


                        result.WithParsed(x => (x as IHostCommand).Execute(skynet));
                    }
                    catch (Exception ex)
                    {
                        log.Fatal("Failed to execute command", ex);
                    }
                }
            }

        }

        [Option('p', "port", Required = false, HelpText = "Specify the port to connect to. Default is '25672'.")]
        public int Port { get; set; }

        [Option('a', "address", Required = false, HelpText = "Specify the address to connect to. Default is '127.0.0.1'.")]
        public string Address { get; set; }

        [Option('e', "execute", Required = false, HelpText = "Executes remote once and exits")]
        public string ExecuteRemote { get; set; }
    }
}