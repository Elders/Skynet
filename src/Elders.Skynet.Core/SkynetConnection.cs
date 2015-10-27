using System;
using Elders.Skynet.Core.Transport;

namespace Elders.Skynet.Core
{
    public class SkynetConnection : IObserver<BasicMessage>
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SkynetConnection));

        private IConnection connection;

        private IDisposable subscription;

        private IPublisher publisher;

        private ISerializer serializer;

        private SyncWait sync;

        public string HostName { get; private set; }

        public Guid Id { get; private set; }

        public SkynetConnection(string hostName, IConnection connection, IPublisher publisher, ISerializer serializer)
        {
            Id = Guid.NewGuid();
            sync = new SyncWait();
            this.connection = connection;
            this.publisher = publisher;
            this.serializer = serializer;
            HostName = hostName;
            subscription = connection.Subscribe(this);
        }

        void IObserver<BasicMessage>.OnNext(BasicMessage message)
        {
            sync.NoWait(() =>
            {
                var msg = serializer.Deserialize(message.Body);
                publisher.Publish(msg, new MessageContext(Id, message.Sender(), (x) => Respond(x, message.Id())));
                sync.Push(message.ResponseId(), msg);
            });
        }

        public void SendMessage(IMessage message)
        {
            sync.NoWait(() => SendMessageInternal(Guid.NewGuid(), message));
        }

        public V SendMessage<V>(IMessage<V> message, TimeSpan timeout)
        {
            var id = Guid.NewGuid();
            return sync.Wait<V>(id, timeout, () => SendMessageInternal(id, message));
        }

        public void Respond(IMessage message, Guid responseId)
        {
            sync.NoWait(() => SendMessageInternal(Guid.NewGuid(), message, responseId));
        }

        void IObserver<BasicMessage>.OnError(Exception error)
        {
        }

        void IObserver<BasicMessage>.OnCompleted()
        {

        }

        public void Disconnect()
        {
            subscription.Dispose();
            connection.Close();
            sync.Dispose();
        }

        private void SendMessageInternal(Guid id, IMessage message, Guid responseId = default(Guid))
        {
            try
            {
                var msg = serializer.Serialize(message);
                var basicMessage = new BasicMessage(msg);
                basicMessage.Sender(HostName);
                basicMessage.Id(id);
                basicMessage.ResponseId(responseId);
                connection.SendMessage(basicMessage);
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Failed to send message to {0} ", id), ex);
            }
        }
    }
}