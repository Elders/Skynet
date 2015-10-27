using System;
using System.Linq;
using Elders.Skynet.Core.Util;

namespace Elders.Skynet.Core.Output
{
    public class BasicOutput : IOutput
    {
        private ConcurrentList<IObserver<string>> subscribers;

        public BasicOutput()
        {
            subscribers = new ConcurrentList<IObserver<string>>();
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(string value)
        {
            Write(value);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            subscribers.Add(observer);
            return new BasicSubscription(() => subscribers.Remove(observer));
        }

        public void Write(string output)
        {
            foreach (var item in subscribers.ToList())
            {
                try
                {
                    item.OnNext(output);
                }
                catch (Exception ex)
                {
                    item.OnError(ex);
                    item.OnCompleted();
                    subscribers.Remove(item);
                }
            }
        }
    }
}
