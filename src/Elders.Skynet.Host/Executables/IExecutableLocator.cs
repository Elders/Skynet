using Elders.Skynet.Core.Handlers.Packages;

namespace Elders.Skynet.Host
{
    public interface IExecutableLocator
    {
        IExectuable GetExecutable(string location);
    }
}