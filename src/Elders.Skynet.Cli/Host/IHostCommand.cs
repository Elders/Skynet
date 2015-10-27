using Elders.Skynet.Core;

namespace Elders.Skynet.Cli.Host
{
    public interface IHostCommand
    {
        void Execute(SkynetClient client);
    }
}