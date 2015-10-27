namespace Elders.Skynet.Core.Contracts.Processes
{
    public class Shutdown : IMessage
    {
        public Shutdown(string processName)
        {
            ProcessName = processName;
        }

        public string ProcessName { get; set; }
    }

    public class Teardown : IMessage
    {
        public Teardown()
        {

        }
    }
}
