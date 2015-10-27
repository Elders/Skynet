using System;

namespace Elders.Skynet.Core
{
    public static class MessageFactory
    {
        public static object ToPublishedMessage(this IMessage message, IMessageContext sender)
        {
            var genericType = typeof(Message<>);
            var messageType = message.GetType();
            var type = genericType.MakeGenericType(messageType);
            return Activator.CreateInstance(type, new object[] { sender, message });
        }
    }

    public class Message<T>
    {
        public Message(IMessageContext sender, T payload)
        {
            Context = sender;
            Payload = payload;
        }

        public IMessageContext Context { get; private set; }
        public T Payload { get; private set; }
    }
}