using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(Frame))]
    public class When_createing_initialized_frame
    {
        Establish ctx = () =>
        {
            type = FrameType.BodyFrame;
            initialBytes = new byte[] { 123, 128 };
            frame = new Frame(type, initialBytes);

        };

        Because of = () => frame = new Frame(type, initialBytes);

        It should_have_the_correct_lenght = () => frame.FrameLenght.ShouldEqual(26);

        It should_be_completed = () => frame.IsCompleted.ShouldEqual(true);

        It should_be_valid = () => frame.IsValid.ShouldEqual(true);

        It should_be_of_the_correct_type = () => frame.Type.ShouldEqual(type);

        static FrameType type;

        static byte[] initialBytes;

        static Frame frame;
    }
}