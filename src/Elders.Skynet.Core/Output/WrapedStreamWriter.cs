using System;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Skynet.Core.Output
{
    public class WrapedStreamWriter : TextWriter
    {
        private TextWriter writer;

        private char[] buffer;

        private int bufferSize;

        private int position;

        private Action<string> write;

        public WrapedStreamWriter(TextWriter writer, Action<string> write, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.write = write;
            this.writer = writer;
        }

        public override Encoding Encoding { get { return this.writer.Encoding; } }

        public override void Close()
        {
            writer.Close();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return writer.CreateObjRef(requestedType);
        }

        public override bool Equals(object obj)
        {
            return writer.Equals(obj);
        }

        public override void Flush()
        {
            write(new string(buffer));
            position = 0;
            buffer = new char[bufferSize];
            writer.Flush();
        }

        public override Task FlushAsync()
        {
            return writer.FlushAsync();
        }

        public override IFormatProvider FormatProvider
        {
            get
            {
                return writer.FormatProvider;
            }
        }

        public override int GetHashCode()
        {
            return writer.GetHashCode();
        }

        public override object InitializeLifetimeService()
        {
            return writer.InitializeLifetimeService();
        }
        public override string NewLine
        {
            get
            {
                return writer.NewLine;
            }

            set
            {
                writer.NewLine = value;
            }
        }
        public override string ToString()
        {
            return writer.ToString();
        }

        public override void Write(bool value)
        {
            writer.Write(value);
        }

        public override void Write(char value)
        {
            buffer[position] = value;
            position++;
            if (position == bufferSize)
            {
                write(new string(buffer));
                position = 0;
                buffer = new char[bufferSize];
            }
            writer.Write(value);
        }

        public override void Write(char[] buffer)
        {
            writer.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            writer.Write(buffer, index, count);
        }

        public override void Write(decimal value)
        {
            writer.Write(value);
        }

        public override void Write(double value)
        {
            writer.Write(value);
        }

        public override void Write(float value)
        {
            writer.Write(value);
        }

        protected override void Dispose(bool disposing)
        {
            //writer.Dispose(disposing);
        }

        public override void Write(int value)
        {
            writer.Write(value);
        }

        public override void Write(long value)
        {
            writer.Write(value);
        }

        public override void Write(object value)
        {
            writer.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            writer.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            writer.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            writer.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            writer.Write(format, arg);
        }
        public override void Write(string value)
        {
            writer.Write(value);
        }

        public override void Write(uint value)
        {
            writer.Write(value);
        }
        public override void Write(ulong value)
        {
            writer.Write(value);
        }

        public override Task WriteAsync(char value)
        {
            return writer.WriteAsync(value);
        }
        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return writer.WriteAsync(buffer, index, count);
        }
        public override Task WriteAsync(string value)
        {
            return writer.WriteAsync(value);
        }

        public override void WriteLine()
        {
            writer.WriteLine();
        }
        public override void WriteLine(bool value)
        {
            writer.WriteLine(value);

        }
        public override void WriteLine(char value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(char[] buffer)
        {
            writer.WriteLine(buffer);
        }
        public override void WriteLine(char[] buffer, int index, int count)
        {
            writer.WriteLine(buffer, index, count);
        }
        public override void WriteLine(decimal value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(double value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(float value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(int value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(long value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(object value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(string format, object arg0)
        {
            writer.WriteLine(format, arg0);
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            writer.WriteLine(format, arg0, arg1);
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            writer.WriteLine(format, arg0, arg1, arg2);
        }
        public override void WriteLine(string format, params object[] arg)
        {
            writer.WriteLine(format, arg);
        }
        public override void WriteLine(string value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(uint value)
        {
            writer.WriteLine(value);
        }
        public override void WriteLine(ulong value)
        {
            writer.WriteLine(value);
        }
        public override Task WriteLineAsync()
        {
            return writer.WriteLineAsync();
        }
        public override Task WriteLineAsync(char value)
        {
            return writer.WriteLineAsync(value);
        }
        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return writer.WriteLineAsync(buffer, index, count);
        }
        public override Task WriteLineAsync(string value)
        {
            return writer.WriteLineAsync(value);
        }
    }
}