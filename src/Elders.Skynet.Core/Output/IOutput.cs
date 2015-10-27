using System;

namespace Elders.Skynet.Core.Output
{
    public interface IOutput : IObservable<string>, IObserver<string>
    {
        void Write(string output);
    }
}