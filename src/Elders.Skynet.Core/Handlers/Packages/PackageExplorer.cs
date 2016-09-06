using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Core.Handlers.Packages
{
    public interface IPackageExplorer
    {
        IExectuable GetExecutable(PackageMeta package);
    }
}
