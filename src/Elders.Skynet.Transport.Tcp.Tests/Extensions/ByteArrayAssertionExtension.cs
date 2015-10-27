using System.IO;
using Machine.Specifications;

namespace Elders.Skynet.Transport.Tcp.Tests
{
    public static class ByteArrayAssertionExtension
    {
        public static void ShouldBeEqualTo(this byte[] bytesLeft, byte[] bytesRight)
        {
            bytesLeft.Length.ShouldEqual(bytesRight.Length);
            for (int i = 0; i < bytesLeft.Length; i++)
            {
                bytesLeft[i].ShouldEqual(bytesRight[i]);
            }
        }

        public static byte[] GetBytes(this byte[] bytes, int offset, int count)
        {
            return new MemoryStream(bytes, offset, count).ToArray();
        }
    }
}