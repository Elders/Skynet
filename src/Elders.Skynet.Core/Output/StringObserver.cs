using System;

namespace Elders.Skynet.Core.Output
{
    public class StringObserver : IObserver<string>
    {
        private Action<string> write;

        public StringObserver(Action<string> write)
        {
            this.write = write;
        }

        public void OnNext(string value)
        {
            write(value);
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }
    }
}