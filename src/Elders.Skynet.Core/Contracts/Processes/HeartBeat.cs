namespace Elders.Skynet.Core.Contracts.Processes
{
    public class Heartbeat : IMessage
    {
        public Heartbeat() { }

        public Heartbeat(int processid)
        {
            ProcessId = processid;
        }

        public int ProcessId { get; set; }
    }
}