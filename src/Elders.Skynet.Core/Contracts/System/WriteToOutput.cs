namespace Elders.Skynet.Core.Contracts.System
{
    public class WriteToOutput : IMessage
    {
        public WriteToOutput() { }

        public WriteToOutput(string message)
        {
            Text = message;
            Type = MessageType.Debug;
        }

        public WriteToOutput(MessageType type, string message)
        {
            Text = message;
            Type = type;
        }

        public string Text { get; set; }

        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
