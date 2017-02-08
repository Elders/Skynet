using Elders.Skynet.Models;

namespace Elders.Skynet.Core.Processes
{

    public class BasicModel : T800
    {
        private T800 runner;

        public BasicModel(T800 runner)
        {
            this.runner = runner;
        }

        public void Command(params string[] args)
        {
            if (runner != null)
                runner.Command(args);
        }

        public void PowerUp(params string[] args)
        {
            if (runner != null)
                runner.PowerUp(args);
        }

        public void Shutdown(params string[] args)
        {
            if (runner != null)
                runner.Shutdown();
        }
    }
}