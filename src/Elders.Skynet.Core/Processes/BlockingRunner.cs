using System.Threading;

namespace Elders.Skynet.Core.Processes
{

    public class BlockingRunner : ISkynetRunner
    {
        private ManualResetEvent evnt;

        public BlockingRunner()
        {
            evnt = new ManualResetEvent(false);
        }

        public void Command(params string[] args)
        {

        }

        public void Start(params string[] args)
        {
            evnt.Reset();
            evnt.WaitOne(Timeout.Infinite);
        }

        public void Stop()
        {
            evnt.Set();
            evnt.Dispose();
            evnt = null;
        }
    }
}
