using System;
using System.IO;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;
using System.Collections.Generic;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Host
{
    public interface IPackageLocator
    {
        IExectuable GetExecutable(PackageMeta package);

        bool Recognise(PackageMeta package);

        Dictionary<string, string> GetMetadata(PackageMeta package);
    }

    public class SimpleExecutableLocator : IPackageLocator
    {
        private string serverLocation;

        private int port;

        public SimpleExecutableLocator(string location, int port)
        {
            this.serverLocation = location;
            this.port = port;
        }

        public IExectuable GetExecutable(PackageMeta package)
        {
            if (File.Exists(package.Location) && package.Location.EndsWith(".exe"))
            {
                return new SimpleExecutable(package.Location, new string[] { serverLocation, port.ToString() });
            }
            else
            {
                var files = Directory.GetFiles(package.Location).Where(x => x.EndsWith(".exe"));
                if (files.Count() > 1)
                    throw new NotSupportedException("More than one executable found");
                if (files.Count() == 1)
                    return new SimpleExecutable(files.First(), new string[] { serverLocation, port.ToString() });
                else
                    return null;
            }
        }

        public bool Recognise(PackageMeta package)
        {
            return Directory.GetFiles(package.Location).Where(x => x.EndsWith(".exe")).Any();
        }

        public Dictionary<string, string> GetMetadata(PackageMeta package)
        {
            var result = new Dictionary<string, string>();
            var files = Directory.GetFiles(package.Location).Where(x => x.EndsWith(".exe")).ToList();
            if (files.Count() > 1)
            {
                result.Add("ERROR", "More than one executable found");
                for (int i = 0; i < files.Count(); i++)
                {
                    result.Add("Executable-" + i, files[i]);
                }
            }
            if (files.Count() == 1)
                result.Add("Executable", files.First());
            return result;
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