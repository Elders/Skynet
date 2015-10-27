using Elders.Skynet.Core.Contracts.System;

namespace Elders.Skynet.Core.Handlers.System
{
    public class WriteToOutputHandler : IMessageHandler<WriteToOutput>
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetHost));

        public void Handle(Message<WriteToOutput> message)
        {
            switch (message.Payload.Type)
            {
                case MessageType.Debug:
                    log.Debug(message.Payload.Text);
                    break;
                case MessageType.Info:
                    log.Info(message.Payload.Text);
                    break;
                case MessageType.Warn:
                    log.Warn(message.Payload.Text);
                    break;
                case MessageType.Error:
                    log.Error(message.Payload.Text);
                    break;
                case MessageType.Fatal:
                    log.Fatal(message.Payload.Text);
                    break;
                default:
                    log.Fatal(message.Payload.Text);
                    break;
            }
        }
    }
}
