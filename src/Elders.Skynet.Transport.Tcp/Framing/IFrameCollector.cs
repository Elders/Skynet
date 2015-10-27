using System.Collections.Generic;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public interface IFrameCollector
    {
        IEnumerable<Frame> ToFrames(IEnumerable<BasicMessage> messages);
        IEnumerable<BasicMessage> ToMessage(IEnumerable<Frame> frames);
    }
}