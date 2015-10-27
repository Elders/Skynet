namespace Elders.Skynet.Core
{
    public interface IPublisher
    {
        void Publish(IMessage message, IMessageContext sender);
    }
}