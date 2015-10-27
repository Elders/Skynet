using System.IO;

namespace Elders.Skynet.Transport.Tcp
{
    public static class StreamExtensions
    {
        public static void Write(this Stream destination, Stream stream, long count)
        {
            var bytes = new byte[count];
            stream.Read(bytes, 0, (int)count);
            destination.Write(bytes, 0, (int)count);
        }

        public static void TryWrite(this Stream destination, Stream stream, int count)
        {
            long bytesLeft = stream.Length - stream.Position;//Bytes left until the end of the stream
            if (bytesLeft < count)
                destination.Write(stream, bytesLeft);//Write to end
            else
                destination.Write(stream, count);
        }
    }
}