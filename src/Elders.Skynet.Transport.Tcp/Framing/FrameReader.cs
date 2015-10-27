using System.Collections.Generic;
using System.IO;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public class FrameReader : IFrameReader
    {
        private Frame currentFrame;

        public FrameReader()
        {
            currentFrame = new Frame();
        }

        public IEnumerable<Frame> Read(Stream stream)
        {
            while (stream.Position < stream.Length)
            {
                currentFrame.Read(stream);
                while (currentFrame.IsValid == false)
                {
                    ClearFrame();

                    if (stream.Position == stream.Length)
                        break;

                    stream.ReadByte();
                    currentFrame.Read(stream);
                }
                if (currentFrame.IsCompleted)
                    yield return currentFrame;

                if (currentFrame.IsValid == false || currentFrame.IsCompleted)
                    ClearFrame();
            }
        }

        private void ClearFrame()
        {
            currentFrame = new Frame();
        }
    }
}