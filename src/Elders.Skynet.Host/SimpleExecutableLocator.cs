using System;
using System.IO;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;

namespace Elders.Skynet.Host
{
    public interface IExecutableLocator
    {
        IExectuable Locate(string location);
    }

    public class SimpleExecutableLocator : IExecutableLocator
    {
        private string serverLocation;

        private int port;

        public SimpleExecutableLocator(string location, int port)
        {
            this.serverLocation = location;
            this.port = port;
        }

        public IExectuable Locate(string location)
        {
            if (File.Exists(location))
            {
                return new SimpleExecutable(location, new string[] { serverLocation, port.ToString() });
            }
            else
            {
                var files = Directory.GetFiles(location).Where(x => x.EndsWith(".exe"));
                if (files.Count() > 1)
                    throw new InvalidOperationException("More than one executable found");
                if (files.Count() == 1)
                    return new SimpleExecutable(files.First(), new string[] { serverLocation, port.ToString() });
                else
                    return null;
            }
        }

        public class SimpleExecutable : IExectuable
        {
            public SimpleExecutable(string file, string[] args)
            {
                string codeBase = typeof(Elders.Skynet.Subprocess.SimpleClient).Assembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                ExecutableLocation = Uri.UnescapeDataString(uri.Path);
                var arguments = new string[] { "simple-client", file }.ToList();
                arguments.AddRange(args);
                this.Args = arguments.ToArray();

            }

            public string ExecutableLocation { get; set; }

            public string[] Args { get; set; }
        }
    }
}