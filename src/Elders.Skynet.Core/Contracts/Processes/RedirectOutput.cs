namespace Elders.Skynet.Core.Contracts.Processes
{
    public class RedirectOutput : IMessage
    {
        public RedirectOutput() { }

        public RedirectOutput(string processName)
        {
            ProcessName = processName;
        }

        public string ProcessName { get; set; }
    }
}