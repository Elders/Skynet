namespace Elders.Skynet.Core.Contracts.Processes
{
    public class Run : IMessage
    {
        public Run() { }

        public Run(string packageName, string processName)
        {
            PackageName = packageName;
            ProcessName = processName;
        }

        public string PackageName { get; set; }

        public string ProcessName { get; set; }
    }
}
