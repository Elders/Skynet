using System;
using System.IO;
using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(Frame))]
    public class When_getting_the_frame_payload
    {
        Establish ctx = () =>
        {
            type = FrameType.BodyFrame;
            initialBytes = new byte[] { 123, 128 };
            frame = new Frame(type, initialBytes);
        };

        Because of = () => payload = frame.ReadPayload();

        It should_have_the_correct_payload = () => payload.ShouldBeEqualTo(initialBytes);


        It should_have_valid_the_correct_GUID_header = () =>
        {
            var array = frame.ToArray();
            var guidHeader = new byte[16];
            var guidBytes = new MemoryStream(array, 0, 16).ToArray();
            Guid.Parse("8ec9c605-5ce9-4c6c-bee8-47fd4098e2f5").ShouldEqual(new Guid(guidBytes));
        };

        static FrameType type;

        static byte[] initialBytes;

        static byte[] payload;

        static Frame frame;
    }
}