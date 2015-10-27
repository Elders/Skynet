using System.Collections.Generic;
using System.IO;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public interface IFrameReader
    {
        IEnumerable<Frame> Read(Stream stream);
    }
}