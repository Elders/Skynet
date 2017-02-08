using System;
using System.Diagnostics;
using System.Threading;
using Elders.Skynet.Core.Contracts.Processes;
using Elders.Skynet.Core.Contracts.System;
using Elders.Skynet.Core.Processes;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;
using Elders.Skynet.Models;

namespace Elders.Skynet.Core
{
    public class SkynetClient : IObserver<IConnection>
    {
        private SkynetConnection SkynetConnection;

        private bool redirectOutput;

        private BasicContainer container;

        private ManualResetEvent @event;

        private Timer timer;

        private IClient client;

        private IDisposable subscription;

        public string ClientName { get; private set; }

        public SkynetClient(string clientName, IClient client) : this(clientName, client, null)
        {
        }

        public SkynetClient(string clientName, IClient client, T800 runner)
        {
            this.client = client;
            ClientName = clientName;
            container = new BasicContainer();
            container.Register(() => this);
            container.Register<T800>(() => new BasicModel(runner));
            timer = new Timer((x) => { if (SkynetConnection != null) SkynetConnection.SendMessage(new Heartbeat(Process.GetCurrentProcess().Id)); }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
            subscription = client.Subscribe(this);
        }

        public void OnCompleted()
        {

        }

        public void Connect(bool autoReconnect, bool redirectOutput, TimeSpan timeout)
        {
            ConnectAsync(autoReconnect, redirectOutput);
            @event = new ManualResetEvent(false);
            @event.Reset();
            @event.WaitOne(timeout);
            if (SkynetConnection == null)
                throw new TimeoutException("Can not connect");
            ReleaseWait();
        }

        public void ConnectAsync(bool autoReconnect)
        {
            ConnectAsync(autoReconnect, false);
        }

        public void ConnectAsync(bool autoReconnect, bool redirectOutput)
        {
            this.redirectOutput = redirectOutput;
            client.Connect(autoReconnect);
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(IConnection connection)
        {
            SkynetConnection = new SkynetConnection(ClientName, connection, new SkynetPublisher(container.Resolve), new JsonSerializer());
            if (redirectOutput)
                SkynetConnection.SendMessage(new RedirectHostOutput());
            if (@event != null)
            {
                @event.Set();
                ReleaseWait();
            }
        }

        public void Send(IMessage message)
        {
            SkynetConnection.SendMessage(message);
        }

        public V Send<V>(IMessage<V> message, TimeSpan timeout)
        {
            return SkynetConnection.SendMessage<V>(message, timeout);
        }

        public void Disconnect()
        {

            subscription.Dispose();
            SkynetConnection.Disconnect();
            timer.Dispose();
            client.Disconnect();
            ReleaseWait();
            container = null;
            SkynetConnection = null;
            client = null;
        }

        private void ReleaseWait()
        {
            if (@event != null)
            {
                @event.Dispose();
                @event = null;
            }
        }
    }
}