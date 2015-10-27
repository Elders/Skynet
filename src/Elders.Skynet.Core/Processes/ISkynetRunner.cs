namespace Elders.Skynet.Core.Processes
{
    public interface ISkynetRunner
    {
        void Start(params string[] args);

        void Command(params string[] args);

        void Stop();
    }
}