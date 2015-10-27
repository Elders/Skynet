using System;
using System.IO;
using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(Frame))]
    public class When_getting_the_frame_array
    {
        Establish ctx = () =>
        {
            type = FrameType.BodyFrame;
            initialBytes = new byte[] { 123, 128 };
            frame = new Frame(type, initialBytes);
            typeBytes = BitConverter.GetBytes((int)type);
            bodyLenght = BitConverter.GetBytes(initialBytes.Length);
        };

        Because of = () => frameBytes = frame.ToArray();

        It should_have_valid_the_correct_GUID_header = () => new MemoryStream(frameBytes, 0, 16).ToArray().ShouldBeEqualTo(guidHeaderBytes);

        It should_have_the_correct_frame_type = () => frameBytes.GetBytes(16, 4).ShouldBeEqualTo(typeBytes);

        It should_have_the_correct_body_Lenght = () => frameBytes.GetBytes(20, 4).ShouldBeEqualTo(bodyLenght);

        It should_have_the_correct_body = () => frameBytes.GetBytes(24, 2).ShouldBeEqualTo(initialBytes);

        static FrameType type;

        static byte[] initialBytes;

        static byte[] frameBytes;

        static Frame frame;

        static byte[] typeBytes;

        static byte[] bodyLenght;

        static byte[] guidHeaderBytes = Guid.Parse("8ec9c605-5ce9-4c6c-bee8-47fd4098e2f5").ToByteArray();
    }
}