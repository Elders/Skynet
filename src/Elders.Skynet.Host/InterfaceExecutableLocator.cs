using System;
using System.IO;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;

namespace Elders.Skynet.Host
{
    public class InterfaceExecutableLocator
    {
        public InterfaceExecutable Locate(string location)
        {
            if (File.Exists(location))
            {
                return new InterfaceExecutable(location);
            }
            else
            {
                var files = Directory.GetFiles(location).Where(x => x.EndsWith(".exe"));
                if (files.Count() > 0)
                    throw new InvalidOperationException("More than one executable found");

                return new InterfaceExecutable(location);
            }
        }

        public class InterfaceExecutable : IExectuable
        {
            public InterfaceExecutable(string file)
            {

            }

            public string ExecutableLocation { get; set; }

            public string[] Args { get; set; }
        }
    }
}