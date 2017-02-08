using Elders.Skynet.Core.Packages;
using System.Collections.Generic;

namespace Elders.Skynet.Core.Handlers.Packages
{
    public interface IPackageExplorer
    {
        IExectuable GetExecutable(PackageMeta package);

        bool Recognise(PackageMeta package);

        Dictionary<string, string> GetMetadata(PackageMeta package);
    }
}
