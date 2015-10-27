using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;
using Elders.Skynet.Transport.Tcp.Protocols;

namespace Elders.Skynet.Transport.Tcp
{
    public class TcpClient : IObservable<IConnection>, IClient
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TcpServer));

        private TimeSpan RetryConnectSpan = TimeSpan.FromSeconds(3);

        private Socket clientSocket;

        private TcpConnection connection;

        private IPEndPoint serverEndPoint;

        private ConcurrentList<IObserver<IConnection>> observers;

        private bool autoreconnect;

        public TcpClient(string address, int port)
        {
            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            observers = new ConcurrentList<IObserver<IConnection>>();
        }

        public void Connect(bool autoreconnect)
        {
            Connect(autoreconnect, TimeSpan.FromSeconds(5));
        }

        public void Connect(bool autoreconnect, TimeSpan autoRecconectTimespan)
        {
            if (connection == null || connection.Connected == false)
            {
                this.autoreconnect = autoreconnect;
                this.RetryConnectSpan = autoRecconectTimespan;
                log.InfoFormat("Connecting to server {0}...", serverEndPoint);
                clientSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (autoreconnect == false)
                {
                    clientSocket.Connect(serverEndPoint);
                    EstablishConnection();
                }
                else
                    clientSocket.BeginConnect(serverEndPoint, ConnectedToServer, this);
            }
        }

        private void EstablishConnection()
        {
            try
            {
                connection = new TcpConnection(clientSocket, new FramingMessageProtocol());
                log.InfoFormat("Connected to server {0}", connection);
                Next(connection);

            }
            catch (Exception ex)
            {
                OnException(ex);
                if (autoreconnect)
                {
                    log.InfoFormat("Failed to connect to server({0}). Retrying in {1}", serverEndPoint, RetryConnectSpan);
                    Thread.Sleep(RetryConnectSpan);
                    try
                    {
                        clientSocket.BeginConnect(serverEndPoint, ConnectedToServer, this);
                    }
                    catch (Exception x)
                    {
                        log.Error($"Failed to connect to server({serverEndPoint}).", ex);
                    }
                }
                else
                {
                    log.ErrorFormat("Failed to connect to server({0}).", serverEndPoint);
                    Complete();
                }
            }
        }

        private void ConnectedToServer(IAsyncResult result)
        {
            EstablishConnection();
        }

        public void Disconnect()
        {
            if (connection != null && connection.Connected)
            {
                lock (this)
                {
                    if (connection != null && connection.Connected)
                    {
                        autoreconnect = false;
                        clientSocket.Close();
                        clientSocket.Dispose();
                        clientSocket = null;
                        connection.Close();
                        connection = null;
                        log.Info("Client disconected");
                        Complete();
                        observers = new ConcurrentList<IObserver<IConnection>>();
                    }
                }
            }
        }

        public void Next(TcpConnection tcpSocket)
        {
            foreach (var item in observers)
            {
                item.OnNext(tcpSocket);
            }
            if (autoreconnect)
                tcpSocket.Subscribe(new CompletedObserver(() => Connect(autoreconnect, RetryConnectSpan)));
        }

        public void Complete()
        {
            foreach (var item in observers)
            {
                item.OnCompleted();
            }
            observers = new ConcurrentList<IObserver<IConnection>>();
        }

        public void OnException(Exception ex)
        {
            foreach (var item in observers)
            {
                item.OnError(ex);
            }
        }

        public IDisposable Subscribe(IObserver<IConnection> observer)
        {
            observers.Add(observer);
            return new BasicSubscription(() => observers.Remove(observer));
        }
    }
}