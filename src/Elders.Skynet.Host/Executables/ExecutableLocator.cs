using System;
using System.Linq;
using Elders.Skynet.Core.Handlers.Packages;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Elders.Skynet.Subprocess;
using Elders.Skynet.Models;

namespace Elders.Skynet.Host
{
    public class ExecutableLocator : IExecutableLocator
    {
        private string serverLocation;

        private int port;

        public ExecutableLocator(string serverLocation, int serverPort)
        {
            this.serverLocation = serverLocation;
            this.port = serverPort;
        }

        public IExectuable GetExecutable(string location)
        {
            var module = SkynetModule(location);
            if (module == null)
                module = SimpleExecutable(location);
            return module;
        }

        private List<string> GetAssemblies(string location)
        {
            var assemblies = Directory.GetFiles(location).Where(x => x.EndsWith(".exe") || x.EndsWith(".dll")).ToList();
            foreach (var item in Directory.GetDirectories(location))
                assemblies.AddRange(GetAssemblies(item));
            return assemblies;
        }

        private IExectuable SkynetModule(string location)
        {
            var assemblies = GetAssemblies(location);
            var module = typeof(T800);
            var moduleAssembly = assemblies.FirstOrDefault(x => Assembly.LoadFrom(x).GetTypes().Any(y => module.IsAssignableFrom(y)));
            return new SkynetModuleExecutable(new FileInfo(moduleAssembly).FullName, serverLocation, port.ToString());
        }

        private IExectuable SimpleExecutable(string location)
        {
            if (File.Exists(location) && location.EndsWith(".exe"))
            {
                return new SimpleExecutable(location, serverLocation, port.ToString());
            }
            else
            {
                var files = Directory.GetFiles(location).Where(x => x.EndsWith(".exe"));
                if (files.Count() > 1)
                    throw new NotSupportedException("More than one executable found");
                if (files.Count() == 1)
                    return new SimpleExecutable(files.First(), serverLocation, port.ToString());
                else
                    return null;
            }
        }
    }
}