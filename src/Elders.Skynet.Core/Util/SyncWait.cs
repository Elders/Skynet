using System;
using System.Collections.Concurrent;
using System.Threading;
using Elders.Skynet.Core.Util;

namespace Elders.Skynet.Core
{

    public class SyncWait : IDisposable
    {
        private BlockingOperation<Action> actions;

        private ConcurrentDictionary<Guid, Result> threads;

        public SyncWait()
        {
            threads = new ConcurrentDictionary<Guid, Result>();
            actions = new BlockingOperation<Action>(x => x());
        }

        public V Wait<V>(Guid id, TimeSpan timeout, Action action)
        {
            var result = new Result(() =>
            {
                Result toRemove;
                threads.TryRemove(id, out toRemove);
            });
            threads.TryAdd(id, result);
            actions.Enqueue(action);
            return (V)(result.Wait(timeout));
        }

        public void NoWait(Action action)
        {
            actions.Enqueue(action);
        }

        public void Push(Guid id, object instance)
        {
            Result result;
            if (threads.TryRemove(id, out result))
            {
                result.Push(instance);
                result.Dispose();
            }
        }

        public void Dispose()
        {
            actions.Exit();
            foreach (var item in threads)
            {
                item.Value.Dispose();
            }
        }

        private class Result : IDisposable
        {
            private object instance;

            private ManualResetEvent @event;

            private Action onTimeout;

            public Result(Action onTimeout)
            {
                this.onTimeout = onTimeout;
                @event = new ManualResetEvent(false);
            }

            public object Wait(TimeSpan timeout)
            {
                @event.Reset();
                @event.WaitOne(timeout);
                if (instance == null)
                {
                    onTimeout();
                    throw new TimeoutException("Operation Timeout");
                }

                return instance;
            }

            public void Push(object instance)
            {
                this.instance = instance;
                @event.Set();
            }

            public void Dispose()
            {
                @event.Dispose();
            }
        }
    }
}