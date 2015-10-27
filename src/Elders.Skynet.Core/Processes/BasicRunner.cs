using System.Threading;

namespace Elders.Skynet.Core.Processes
{

    public class BasicRunner : ISkynetRunner
    {
        private ISkynetRunner runner;

        public BasicRunner(ISkynetRunner runner)
        {
            this.runner = runner;
        }

        public void Command(params string[] args)
        {
            if (runner != null)
                runner.Command(args);
        }

        public void Start(params string[] args)
        {
            if (runner != null)
                runner.Start(args);
        }

        public void Stop()
        {
            if (runner != null)
                runner.Stop();
        }
    }
}