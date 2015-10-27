using System.Collections.Generic;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public class FrameCollector : IFrameCollector
    {
        private Queue<Frame> collectedFrames = new Queue<Frame>();

        public IEnumerable<Frame> ToFrames(IEnumerable<BasicMessage> messages)
        {
            foreach (var message in messages)
            {
                if (message.Headers.Count > 0)
                    yield return new Frame(FrameType.HeadersFrame, BasicDictionarySerializer.Serialize(message.Headers));
                yield return new Frame(FrameType.BodyFrame, message.Body);
            }
        }

        public IEnumerable<BasicMessage> ToMessage(IEnumerable<Frame> frames)
        {
            foreach (var frame in frames)
            {

                if (frame.Type == FrameType.BodyFrame)
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    if (collectedFrames.Count > 0)
                    {
                        var frameHeader = collectedFrames.Dequeue();
                        headers = BasicDictionarySerializer.Deserialize(frameHeader.ReadPayload());
                    }
                    yield return new BasicMessage(frame.ReadPayload(), headers);
                    collectedFrames.Clear();//If there are errored messages where there are only headers and no body. We expect the body frame to be the last frame
                }
                else
                    collectedFrames.Enqueue(frame);
            }
        }
    }
}