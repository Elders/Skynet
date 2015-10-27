using System;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Core.Util
{
    public class CompletedObserver : IObserver<BasicMessage>
    {
        private Action onCompleted;

        public CompletedObserver(Action onCompleted)
        {
            this.onCompleted = onCompleted;
        }

        void IObserver<BasicMessage>.OnCompleted()
        {
            onCompleted();
        }

        void IObserver<BasicMessage>.OnError(Exception error) { }

        void IObserver<BasicMessage>.OnNext(BasicMessage value) { }
    }
}