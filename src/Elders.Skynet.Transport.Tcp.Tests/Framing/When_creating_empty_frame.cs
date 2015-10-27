using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Framing;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(Frame))]
    public class When_creating_empty_frame
    {
        Because of = () => frame = new Frame();

        It should_have_the_correct_lenght = () => frame.FrameLenght.ShouldEqual(0);

        It should_be_completed = () => frame.IsCompleted.ShouldEqual(false);

        It should_be_valid = () => frame.IsValid.ShouldEqual(true);

        It should_be_of_the_correct_type = () => frame.Type.ShouldEqual((FrameType)0);

        static Frame frame;
    }
}