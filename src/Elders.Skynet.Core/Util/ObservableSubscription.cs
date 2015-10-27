using System;

namespace Elders.Skynet.Core.Util
{
    public class BasicSubscription : IDisposable
    {
        private Action onDispose;

        public BasicSubscription(Action onDispose)
        {
            this.onDispose = onDispose;
        }
        public void Dispose()
        {
            onDispose();
        }
    }
}
