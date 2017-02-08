using Elders.Skynet.Models;
using System.Threading;

namespace Elders.Skynet.Core.Processes
{

    public class BlockingModel : T800
    {
        private ManualResetEvent evnt;

        public BlockingModel()
        {
            evnt = new ManualResetEvent(false);
        }

        public void Command(params string[] args)
        {

        }

        public void PowerUp(params string[] args)
        {
            evnt.Reset();
            evnt.WaitOne(Timeout.Infinite);
        }

        public void Shutdown(params string[] args)
        {
            evnt.Set();
            evnt.Dispose();
            evnt = null;
        }
    }
}
