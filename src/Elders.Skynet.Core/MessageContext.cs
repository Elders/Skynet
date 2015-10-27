using System;

namespace Elders.Skynet.Core.Transport
{
    public class MessageContext : IMessageContext
    {
        private Action<IMessage> respond;

        public MessageContext(Guid id, string name, Action<IMessage> respond)
        {
            ConnectionId = id;
            SenderName = name;
            this.respond = respond;
        }

        public Guid ConnectionId { get; set; }

        public string SenderName { get; set; }

        public void Respond(IMessage message)
        {
            respond(message);
        }
    }
}