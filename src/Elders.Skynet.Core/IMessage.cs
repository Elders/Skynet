namespace Elders.Skynet.Core
{
    public interface IMessage
    {

    }

    public interface IMessage<TResult> : IMessage
    {

    }

    public interface IMessageHandler<T>
        where T : IMessage
    {
        void Handle(Message<T> message);
    }

    public interface IMessageHandler<T, V>
        where T : IMessage<V>
        where V : IMessage
    {
        V Handle(Message<T> message);
    }
}