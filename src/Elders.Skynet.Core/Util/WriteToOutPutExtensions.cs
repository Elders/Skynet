using Elders.Skynet.Core.Contracts.System;

namespace Elders.Skynet.Core
{
    public static class WriteToOutPutExtensions
    {
        public static void Error(this IMessageContext context, string message)
        {
            context.Respond(new WriteToOutput(MessageType.Error, message));
        }

        public static void ErrorFormat(this IMessageContext context, string message, params object[] args)
        {
            context.Respond(new WriteToOutput(MessageType.Error, string.Format(message, args)));
        }

        public static void Info(this IMessageContext context, string message)
        {
            context.Respond(new WriteToOutput(MessageType.Info, message));
        }

        public static void InfoFormat(this IMessageContext context, string message, params object[] args)
        {
            context.Respond(new WriteToOutput(MessageType.Info, string.Format(message, args)));
        }
    }
}
