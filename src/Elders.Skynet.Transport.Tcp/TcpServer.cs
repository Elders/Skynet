using System;
using System.Net;
using System.Net.Sockets;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;
using Elders.Skynet.Transport.Tcp.Protocols;

namespace Elders.Skynet.Transport.Tcp
{
    public class TcpServer : IServer
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TcpServer));

        private Socket serverSocket;

        private bool stopped;

        private ConcurrentList<TcpConnection> connections;

        private ConcurrentList<IObserver<TcpConnection>> observers;

        private EndPoint serverEndpoint;

        public TcpServer(EndPoint serverEndpoint)
        {
            this.serverEndpoint = serverEndpoint;
        }

        private void Init()
        {
            stopped = false;
            connections = new ConcurrentList<TcpConnection>();
            observers = new ConcurrentList<IObserver<TcpConnection>>();
        }

        public void Start()
        {
            Init();
            try
            {
                serverSocket = new Socket(serverEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (SocketException ex)
            {
                log.Fatal("Could not create socket, check to make sure not duplicating port", ex);
                throw new ApplicationException("Could not create socket, check to make sure not duplicating port", ex);
            }
            serverSocket.Bind(serverEndpoint);//Bind to endpoint
            serverSocket.Listen(200);
            log.InfoFormat("Server started at {0}", serverEndpoint);
            serverSocket.BeginAccept(ClientConnected, this);

        }

        private void ClientConnected(IAsyncResult result)
        {
            if (!stopped)
            {
                Socket clientSocket = null;
                TcpConnection connection = null;
                try
                {
                    clientSocket = serverSocket.EndAccept(result);
                    connection = new TcpConnection(clientSocket, new FramingMessageProtocol());
                    log.InfoFormat("New connection established: {0}", connection);
                    TcpSocketConnected(connection);
                    serverSocket.BeginAccept(ClientConnected, this);// Continue accepting
                }
                catch (SocketException ex)
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connections.Remove(connection);
                    }
                    else if (clientSocket != null)
                    {
                        clientSocket.Close();
                        clientSocket.Dispose();
                    }

                    log.Error("Socket exception.", ex);
                    OnException(ex);
                    AcceptNextClient();
                }
                catch (Exception ex)
                {
                    log.Fatal("Unexpected error.", ex);
                    if (connection != null)
                    {
                        connection.Close();
                        connections.Remove(connection);
                    }
                    else if (clientSocket != null)
                    {
                        clientSocket.Close();
                    }

                    OnException(ex);
                    AcceptNextClient();
                }
            }
        }

        private void AcceptNextClient()
        {
            try
            {
                if (!stopped)
                    serverSocket.BeginAccept(ClientConnected, this);
            }
            catch (Exception ex)
            {
                log.Fatal("Failed to continiue accepting sockets. The server will sutting down", ex);
                Stop();
            }
        }

        public void Stop()
        {
            if (!stopped)
            {
                lock (this)
                {
                    if (!stopped)
                    {
                        stopped = true;
                        if (serverSocket != null)
                        {
                            serverSocket.Close();
                            serverSocket.Dispose();
                            serverSocket = null;
                        }
                        foreach (var item in connections)
                        {
                            item.Close();
                        }
                        ServerStopped();
                        log.Info("Server stopped");
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<IConnection> observer)
        {
            observers.Add(observer);
            return new BasicSubscription(() => observers.Remove(observer));
        }

        private void TcpSocketConnected(TcpConnection tcpSocket)
        {
            connections.Add(tcpSocket);
            tcpSocket.Subscribe(new CompletedObserver(() => connections.Remove(tcpSocket)));
            foreach (var item in observers)
            {
                item.OnNext(tcpSocket);
            }
        }

        private void ServerStopped()
        {
            foreach (var item in observers)
            {
                item.OnCompleted();
            }
            observers = new ConcurrentList<IObserver<TcpConnection>>();
        }

        private void OnException(Exception ex)
        {
            foreach (var item in observers)
            {
                item.OnError(ex);
            }
        }

    }
}
