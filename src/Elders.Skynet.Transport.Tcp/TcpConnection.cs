using System;
using System.IO;
using System.Net.Sockets;
using Elders.Skynet.Core.Transport;
using Elders.Skynet.Core.Util;
using Elders.Skynet.Transport.Tcp.Protocols;

namespace Elders.Skynet.Transport.Tcp
{
    public class TcpConnection : IConnection
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TcpConnection));

        private const int maxBufferSize = 4000;

        private Socket clientSocket;

        private byte[] buffer;

        private bool closed;

        public bool Connected { get { return closed == false; } }

        private IMessageProtocol protocol;

        private ConcurrentList<IObserver<BasicMessage>> observers;

        public TcpConnection(Socket clientSocket, IMessageProtocol protocol)
        {
            observers = new ConcurrentList<IObserver<BasicMessage>>();
            this.clientSocket = clientSocket;
            this.protocol = protocol;
            closed = false;
            buffer = new byte[maxBufferSize];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BytesRecieved, this);
        }

        private void BytesRecieved(IAsyncResult result)
        {
            if (!closed)
            {
                try
                {
                    int bytesRecieved = clientSocket.EndReceive(result);//Get the number if recieved bytes
                    if (bytesRecieved > 0)
                    {
                        using (var stream = new MemoryStream(buffer))
                        {
                            var messages = protocol.Read(stream);
                            foreach (var item in messages)
                            {
                                NextMessage(item);
                            }
                            buffer = new byte[maxBufferSize];
                        }
                        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, BytesRecieved, this);
                    }
                    else
                    {
                        log.Warn("Recieve callback raised, but no bytes were read. So the connection is considered dropped");
                        Close();
                    }
                }
                catch (SocketException ex)
                {
                    log.Warn("Socket error.", ex);
                    OnException(ex);
                    Close();
                }
                catch (Exception ex)
                {
                    log.Fatal("Unexpected error.", ex);
                    OnException(ex);
                    Close();
                }
            }
        }

        public void SendMessage(BasicMessage message)
        {
            if (closed)
                throw new InvalidOperationException("Socket is closed");

            using (var stream = protocol.ToStream(new BasicMessage[] { message }))
            {
                stream.Position = 0;
                while (stream.Position < stream.Length)
                {
                    var remainingBytes = stream.Length - stream.Position;
                    var bufferSize = (remainingBytes < maxBufferSize) ? (int)remainingBytes : maxBufferSize;
                    var buffer = new byte[bufferSize];
                    stream.Read(buffer, 0, bufferSize);
                    var sent = clientSocket.Send(buffer);
                    if (sent != bufferSize)
                    {
                        log.WarnFormat("Sent less bytes({0}) than the buffer size({1}).", sent, bufferSize);
                        var bytesToReturn = (stream.Position - bufferSize) + sent;
                        stream.Position = bytesToReturn;
                    }
                }
            }
        }

        public void Close()
        {
            if (!closed)
            {
                lock (this)
                {
                    if (!closed)
                    {
                        try
                        {
                            closed = true;
                            if (clientSocket != null)
                            {
                                clientSocket.Disconnect(false);
                                clientSocket.Close();
                                clientSocket.Dispose();
                                clientSocket = null;
                                log.Info("Connection closed");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Fatal("Failed to close socket.");
                            OnException(ex);
                        }
                        SocketClosed();
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<BasicMessage> observer)
        {
            observers.Add(observer);
            return new BasicSubscription(() => observers.Remove(observer));
        }

        private void NextMessage(BasicMessage message)
        {
            foreach (var item in observers)
            {
                item.OnNext(message);
            }
        }

        private void SocketClosed()
        {
            foreach (var item in observers)
            {
                item.OnCompleted();
            }
        }

        private void OnException(Exception ex)
        {
            foreach (var item in observers)
            {
                item.OnError(ex);
            }
        }

        public override string ToString()
        {
            return clientSocket.RemoteEndPoint.ToString();
        }
    }
}