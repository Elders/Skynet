using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Elders.Skynet.Core.Util
{
    public class BlockingOperation<T>
    {
        private ConcurrentQueue<T> Items;

        private Action<T> onItem;

        private Thread trd;

        private ManualResetEvent handle;

        private bool running;

        public BlockingOperation(Action<T> onItem)
        {
            handle = new ManualResetEvent(false);
            this.onItem = onItem;
            Items = new System.Collections.Concurrent.ConcurrentQueue<T>();
            trd = new Thread(Operate);
            trd.Start();
            running = true;
        }

        public void Exit()
        {
            running = false;
            handle.Set();
            handle.Dispose();
        }

        public void Enqueue(T item)
        {
            Items.Enqueue(item);
            handle.Set();
        }

        private void Operate()
        {
            while (running)
            {
                T item;
                while (Items.TryDequeue(out item))
                {
                    onItem(item);
                }
                handle.Reset();
                handle.WaitOne(Timeout.Infinite);
            }
        }
    }
}