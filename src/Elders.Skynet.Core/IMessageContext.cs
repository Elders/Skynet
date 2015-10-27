using System;

namespace Elders.Skynet.Core
{
    public interface IMessageContext
    {
        string SenderName { get; }

        Guid ConnectionId { get; }

        void Respond(IMessage message);
    }
}