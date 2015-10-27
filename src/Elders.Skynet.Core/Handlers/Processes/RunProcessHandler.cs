using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Handlers.Packages;
using Elders.Skynet.Core.Output;
using Elders.Skynet.Core.Packages;
using Elders.Skynet.Core.Processes;

namespace Elders.Skynet.Core.Handlers.Processes
{
    public class RunProcessHandler : IMessageHandler<Run>
    {
        public SkynetProcessRegistry Registry { get; set; }

        public IPackageRepository Repository { get; set; }

        public IPackageExplorer PackageExplorer { get; set; }

        public IOutput Output { get; set; }

        public void Handle(Message<Run> message)
        {
            var package = Repository.GetPackage(message.Payload.PackageName);
            var executable = PackageExplorer.GetExecutable(package);
            var proccess = Registry.StartNewProcess(package.Name, message.Payload.ProcessName, executable.ExecutableLocation, executable.Args);
        }
    }
}
