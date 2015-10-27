using System.Collections.Generic;
using System.IO;
using Machine.Specifications;
using Elders.Skynet.Transport.Tcp.Protocols;
using System.Linq;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    [Subject(typeof(FramingMessageProtocol))]
    public class When_reading_messages_from_two_streams
    {
        Establish ctx = () =>
        {
            protocol = new FramingMessageProtocol();
            message1 = new BasicMessage(new byte[] { 1, 2, 3, 4 }, new Dictionary<string, string>() { { "123", "1234" } });
            message2 = new BasicMessage(new byte[] { 4, 5, 6, 7 }, new Dictionary<string, string>() { { "asd", "ffs" } });
            stream1 = protocol.ToStream(new BasicMessage[] { message1 });
            stream2 = protocol.ToStream(new BasicMessage[] { message2 });

            stream1.Position = 0;
            stream2.Position = 0;
        };

        Because of = () =>
        {
            var buffer1 = new byte[4000];
            stream1.Read(buffer1, 0, 4000);
            result1 = protocol.Read(new MemoryStream(buffer1)).Single();

            var buffer2 = new byte[4000];
            stream2.Read(buffer2, 0, 4000);
            result2 = protocol.Read(new MemoryStream(buffer2)).Single();
        };

        It should_have_read_message_one = () => result1.Body.ShouldBeEqualTo(message1.Body);

        It should_have_read_message_two = () => result2.Body.ShouldBeEqualTo(message2.Body);

        static FramingMessageProtocol protocol;

        static BasicMessage message1;
        static BasicMessage message2;

        static Stream stream1;
        static Stream stream2;

        static BasicMessage result1;
        static BasicMessage result2;

    }
}