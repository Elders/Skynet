using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Skynet.Core.Packages;

namespace Elders.Skynet.Core.Processes
{
    public class SkynetProcessRegistry
    {
        List<SkynetProcess> startedProcesses;

        public SkynetProcessRegistry(IPackageRepository repostory)
        {
            startedProcesses = new List<SkynetProcess>();
        }

        public SkynetProcessInfo StartNewProcess(string packageName, string processName, string executable, string[] args)
        {
            lock (startedProcesses)
            {
                if (startedProcesses.Select(x => x.Name).Contains(processName))
                    throw new InvalidOperationException(String.Format("There is already a process with name {0}", processName));
                var process = new SkynetProcess(executable, args, packageName, processName);
                startedProcesses.Add(process);
                return process.GetProcessInfo();
            }
        }

        public void Kill(SkynetProcessInfo process)
        {
            lock (startedProcesses)
            {
                var proc = startedProcesses.Where(x => x.ProcessId == process.ProcessId).FirstOrDefault();
                if (proc != null)
                    proc.Kill();
            }
        }

        public SkynetProcess GetProcesss(int processId)
        {
            lock (startedProcesses)
            {
                var result = startedProcesses.SingleOrDefault(x => x.ProcessId == processId);
                return result;
            }
        }

        public IEnumerable<SkynetProcessInfo> GetAllProcesses()
        {
            lock (startedProcesses)
            {
                var result = startedProcesses.Select(x => x.GetProcessInfo()).ToList();
                return result;
            }
        }

        public void Heartbeat(Guid origin, int processId)
        {
            lock (startedProcesses)
            {
                var process = startedProcesses.Where(x => x.ProcessId == processId).SingleOrDefault();
                if (process != null)
                    process.RegisterHeartbeat(origin);
                else
                {
                    var processName = origin.GetHashCode();
                    var proc = SkynetProcess.FromId(processId, processName.ToString());
                    startedProcesses.Add(proc);
                    proc.RegisterHeartbeat(origin);
                }
            }
        }

        public IDisposable SubscrbeForOutput(SkynetProcessInfo processInfo, IObserver<string> subscriber)
        {
            lock (startedProcesses)
            {
                var process = startedProcesses.Where(x => x.ProcessId == processInfo.ProcessId).SingleOrDefault();
                if (process != null)
                    return process.Output.Subscribe(subscriber);
                else
                    return null;
            }
        }
    }
}
