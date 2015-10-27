using System.Collections.Generic;
using System.IO;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Protocols
{
    public class FramingMessageProtocol : IMessageProtocol
    {
        FrameCollector frameCollector;
        FrameWriter frameWriter;
        FrameReader frameReader;

        public FramingMessageProtocol()
        {
            this.frameCollector = new FrameCollector();
            this.frameWriter = new FrameWriter();
            this.frameReader = new FrameReader();
        }

        public Stream ToStream(IEnumerable<BasicMessage> messages)
        {
            var stream = new MemoryStream();
            var frames = frameCollector.ToFrames(messages);
            frameWriter.WriteToStream(stream, frames);
            return stream;
        }

        public IEnumerable<BasicMessage> Read(Stream stream)
        {
            var frames = frameReader.Read(stream);
            return frameCollector.ToMessage(frames);
        }
    }
}
