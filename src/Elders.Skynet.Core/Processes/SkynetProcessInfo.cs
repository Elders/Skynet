using System;

namespace Elders.Skynet.Core.Processes
{
    public class SkynetProcessInfo
    {
        public SkynetProcessInfo() { }

        public SkynetProcessInfo(string packageName, string processName, int processId, DateTime lastHeartbeat, Guid heartbeatOrigin, bool exited, bool responing)
        {
            PackageName = packageName;
            Name = processName;
            ProcessId = processId;
            LastHeartbeat = lastHeartbeat;
            HeartbeatOrigin = heartbeatOrigin;
            Extied = exited;
            Responding = responing;
        }

        public string PackageName { get; set; }

        public string Name { get; set; }

        public int ProcessId { get; set; }

        public DateTime LastHeartbeat { get; set; }

        public Guid HeartbeatOrigin { get; set; }

        public bool Extied { get; set; }

        public bool Responding { get; set; }

        public override string ToString()
        {
            string s = string.Empty;
            foreach (var item in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propValue = item.GetValue(this);
                if (propValue != null)
                {
                    s += item.Name + ":" + propValue + "|";
                }
            }
            return s;
        }
    }
}