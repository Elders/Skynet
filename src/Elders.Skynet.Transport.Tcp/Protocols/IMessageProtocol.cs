using System.Collections.Generic;
using System.IO;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Transport.Tcp.Protocols
{
    public interface IMessageProtocol
    {
        IEnumerable<BasicMessage> Read(Stream stream);
        Stream ToStream(IEnumerable<BasicMessage> messages);
    }
}