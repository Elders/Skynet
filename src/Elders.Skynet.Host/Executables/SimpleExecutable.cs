using System;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;

namespace Elders.Skynet.Host
{
    public class SimpleExecutable : IExectuable
    {
        public SimpleExecutable(string file, string serverLocation, string port)
        {
            var arguments = new string[] { "simple-client", file }.ToList();
            arguments.Add(serverLocation);
            arguments.Add(port);
            string codeBase = typeof(Elders.Skynet.Subprocess.SimpleClient).Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            ExecutableLocation = Uri.UnescapeDataString(uri.Path);
            this.Args = arguments.ToArray();
        }

        public string ExecutableLocation { get; set; }

        public string[] Args { get; set; }
    }

    public class SkynetModuleExecutable : IExectuable
    {
        public SkynetModuleExecutable(string file, string serverLocation, string port)
        {
            var arguments = new string[] { "skynet-client", file }.ToList();
            arguments.Add(serverLocation);
            arguments.Add(port);
            string codeBase = typeof(Elders.Skynet.Subprocess.SimpleClient).Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            ExecutableLocation = Uri.UnescapeDataString(uri.Path);
            this.Args = arguments.ToArray();
        }

        public string ExecutableLocation { get; set; }

        public string[] Args { get; set; }
    }
}