using System;

namespace Elders.Skynet.Core.Transport
{
    public static class MessageHeadersAppender
    {
        const string SenderKey = "Sender";

        const string MessageIdKey = "MessageId";

        const string ResponseIdKey = "MessageResponseId";

        public static string Sender(this BasicMessage message)
        {
            return message.Headers[SenderKey];
        }

        public static BasicMessage Sender(this BasicMessage message, string clientName)
        {
            message.Headers.Add(SenderKey, clientName);
            return message;
        }

        public static Guid Id(this BasicMessage message)
        {
            return Guid.Parse(message.Headers[MessageIdKey]);
        }

        public static BasicMessage Id(this BasicMessage message, Guid id)
        {
            message.Headers.Add(MessageIdKey, id.ToString());
            return message;
        }

        public static Guid ResponseId(this BasicMessage message)
        {
            Guid result = default(Guid);
            if (message.Headers.ContainsKey(ResponseIdKey))
                Guid.TryParse(message.Headers[ResponseIdKey], out result);
            return result;
        }

        public static BasicMessage ResponseId(this BasicMessage message, Guid id)
        {
            message.Headers.Add(ResponseIdKey, id.ToString());
            return message;
        }
    }
}