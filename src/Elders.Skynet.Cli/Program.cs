using System;
using System.Linq;
using CommandLine;

namespace Elders.Skynet.Cli
{
    class Program
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var commands = typeof(Program).Assembly.GetTypes().Where(x => typeof(ICliCommand).IsAssignableFrom(x));
            try
            {
                var result = Parser.Default.ParseArguments(args, commands.ToArray());
                result.WithNotParsed(errors =>
                {
                    if (errors.Any())
                    {
                        log.Error("Exit Code: 1");
                        Environment.Exit(1);
                    }
                });
                result.WithParsed(x => (x as ICliCommand).Execute());

            }
            catch (Exception ex)
            {
                log.Error("Exit Code: 1", ex);
                Environment.Exit(1);
            }

            log.Info("Exit Code: 1");
        }
    }
}
