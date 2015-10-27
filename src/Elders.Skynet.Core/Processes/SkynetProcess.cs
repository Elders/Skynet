using System;
using System.Diagnostics;
using Elders.Skynet.Core.OSApi;
using Elders.Skynet.Core.Output;

namespace Elders.Skynet.Core.Processes
{
    public class SkynetProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetProcess));

        private Process process;

        public string PackageName { get; private set; }

        public string Name { get; private set; }

        public int ProcessId { get { return process.Id; } }

        public DateTime LastHeartbeat { get; private set; }

        public Guid HeartbeatOrigin { get; private set; }

        public IOutput Output { get; private set; }

        private SkynetProcess() { }

        public SkynetProcess(string executable, string[] args, string packageName, string processName)
        {
            PackageName = packageName;
            Name = processName;
            Output = new BasicOutput();
            var processInfo = new ProcessStartInfo(executable);
            processInfo.Arguments = string.Join(" ", args);
            log.InfoFormat("Starting {0} {1}", executable, processInfo.Arguments);
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.UseShellExecute = false;
            process = new Process();
            Win32Os.SetErrorMode(Win32Os.ErrorModes.SEM_NOGPFAULTERRORBOX);
            process.StartInfo = processInfo;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += ErrorRecieved;
            process.OutputDataReceived += OutputRecieved;
            process.Exited += Exited;
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

        }

        public static SkynetProcess FromId(int id, string processName)
        {
            var result = new SkynetProcess();
            result.process = Process.GetProcessById(id);
            result.Output = new BasicOutput();
            result.Name = "S-" + processName;
            result.PackageName = "S-" + result.process.ProcessName;
            return result;
        }

        public void RegisterHeartbeat(Guid origin)
        {
            HeartbeatOrigin = origin;
            LastHeartbeat = DateTime.UtcNow;
        }

        public void Kill()
        {
            process.Kill();
        }

        public SkynetProcessInfo GetProcessInfo()
        {
            try
            {
                process.Refresh();
                if (process.HasExited == false)
                    return new SkynetProcessInfo(PackageName, Name, ProcessId, LastHeartbeat, HeartbeatOrigin, process.HasExited, process.Responding);
                else
                    return new SkynetProcessInfo(PackageName, Name, ProcessId, LastHeartbeat, HeartbeatOrigin, process.HasExited, false);
            }
            catch (Exception ex)
            {
                return new SkynetProcessInfo(PackageName, Name, ProcessId, LastHeartbeat, HeartbeatOrigin, true, false);
            }
        }

        private void OutputRecieved(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    Output.Write(String.Format("========{0}========>", Name));
                    Output.Write(e.Data);
                }
            }
            catch (Exception ex) { log.Fatal("Unhandled exception.. the program is exiting here if the exception was not captured...", ex); }
        }

        private void ErrorRecieved(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    Output.Write(String.Format("========{0}========>", Name));
                    Output.Write(e.Data);
                }
            }
            catch (Exception ex) { log.Fatal("Unhandled exception.. the program is exiting here if the exception was not captured...", ex); }
        }

        private void Exited(object sender, EventArgs e)
        {
            try
            {
                log.WarnFormat("Processs exited {0} | Exit Code: {1}", this, process.ExitCode);
            }
            catch (Exception ex) { log.Fatal("Unhandled exception.. the program is exiting here if the exception was not captured...", ex); }
        }

        public override string ToString()
        {
            return GetProcessInfo().ToString();
        }
    }
}