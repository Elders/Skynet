using System;
using System.Linq;
using CommandLine;
using Elders.Skynet.Cli.TableBuilder;
using Elders.Skynet.Core;
using Elders.Skynet.Core.Contracts.Processes;

namespace Elders.Skynet.Cli.Host
{
    [Verb("processes", HelpText = "Lists all processes.")]
    public class Processes : IHostCommand
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Processes));

        public void Execute(SkynetClient client)
        {
            try
            {
                var result = client.Send(new GetAllProcesses(), TimeSpan.FromSeconds(10));
                var table = new Table();
                table.AddRow("Package Name", "Name", "Process Id", "Last Heartbeat", "Status");
                foreach (var item in result.Packages.OrderBy(x => x.PackageName))
                {

                    var status = item.Extied ? "Dead" : "Live";
                    status = item.Extied == false && item.Responding == false ? "Not responding" : status;
                    table.AddRow(item.PackageName, item.Name, item.ProcessId, TimeAgo(item.LastHeartbeat), status);
                }
                Console.Write(table.Output());
                Console.WriteLine();
            }
            catch (TimeoutException ex)
            {
                log.Error("Operation timeout after 10 seconds.");
            }

        }

        public static string TimeAgo(DateTime dt)
        {
            if (DateTime.UtcNow - dt > TimeSpan.FromDays(700))
                return "----";
            TimeSpan span = DateTime.UtcNow - dt;

            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 12)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 12)
                return "just now";
            return string.Empty;
        }
    }
}