using System;
using System.Collections.Generic;

namespace Elders.Skynet.Core.Transport
{
    public class BasicMessage
    {
        public byte[] Body { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public BasicMessage(byte[] body)
        {
            Body = body;
            Headers = new Dictionary<string, string>();
        }

        public BasicMessage(byte[] body, Dictionary<string, string> headers)
            : this(body)
        {
            if (headers == null)
                throw new ArgumentNullException("Headers can not be null", nameof(headers));

            Headers = headers;
        }
    }
}