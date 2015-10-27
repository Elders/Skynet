using System.Collections.Generic;
using System.IO;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public interface IFrameWriter
    {
        void WriteToStream(Stream stream, IEnumerable<Frame> frames);
    }
}