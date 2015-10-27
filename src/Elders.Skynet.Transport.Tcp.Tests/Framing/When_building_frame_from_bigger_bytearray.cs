using System;
using System.IO;
using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(Frame))]
    public class When_building_frame_from_bigger_bytearray
    {
        Establish ctx = () =>
        {
            type = FrameType.BodyFrame;
            initialBytes = new byte[] { 123, 128 };
            frame = new Frame(type, initialBytes);
            typeBytes = BitConverter.GetBytes((int)type);
            bodyLenght = BitConverter.GetBytes(initialBytes.Length);
            frameBytes = frame.ToArray();
            newFrame = new Frame();
            var increasedBytesStrean = new MemoryStream();
            increasedBytesStrean.Write(frameBytes, 0, frameBytes.Length);
            increasedBytesStrean.Write(new byte[] { 113, 114, 115, 116 }, 0, 4);
            biggerBytes = increasedBytesStrean.ToArray();
            biggerBytesStream = new MemoryStream(biggerBytes);
        };

        Because of = () => newFrame.Read(biggerBytesStream);

        It should_have_read_all_the_bytes = () => biggerBytesStream.Position.ShouldEqual(frameBytes.Length);

        It should_have_the_correct_lenght = () => newFrame.FrameLenght.ShouldEqual(26);

        It should_be_completed = () => newFrame.IsCompleted.ShouldEqual(true);

        It should_be_valid = () => newFrame.IsValid.ShouldEqual(true);

        It should_be_of_the_correct_type = () => newFrame.Type.ShouldEqual(type);

        It should_have_valid_the_correct_GUID_header = () => new MemoryStream(newFrame.ToArray(), 0, 16).ToArray().ShouldBeEqualTo(guidHeaderBytes);

        It should_have_the_correct_frame_type = () => newFrame.ToArray().GetBytes(16, 4).ShouldBeEqualTo(typeBytes);

        It should_have_the_correct_body_Lenght = () => newFrame.ToArray().GetBytes(20, 4).ShouldBeEqualTo(bodyLenght);

        It should_have_the_correct_body = () => newFrame.ToArray().GetBytes(24, 2).ShouldBeEqualTo(initialBytes);

        static FrameType type;

        static byte[] biggerBytes;

        static MemoryStream biggerBytesStream;

        static byte[] initialBytes;

        static byte[] frameBytes;

        static Frame frame;

        static Frame newFrame;

        static byte[] typeBytes;

        static byte[] bodyLenght;

        static byte[] guidHeaderBytes = Guid.Parse("8ec9c605-5ce9-4c6c-bee8-47fd4098e2f5").ToByteArray();
    }
}