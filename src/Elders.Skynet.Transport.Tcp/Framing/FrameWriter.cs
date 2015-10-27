using System.Collections.Generic;
using System.IO;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public class FrameWriter : IFrameWriter
    {
        public void WriteToStream(Stream stream, IEnumerable<Frame> frames)
        {
            foreach (var frame in frames)
            {
                var frameBytes = frame.ToArray();
                stream.Write(frameBytes, 0, frameBytes.Length);
            }
        }
    }
}