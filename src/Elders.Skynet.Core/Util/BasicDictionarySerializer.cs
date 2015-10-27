using System;
using System.Collections.Generic;
using System.Text;

namespace Elders.Skynet.Core.Util
{
    public static class BasicDictionarySerializer
    {
        public static byte[] Serialize(Dictionary<string, string> headers)
        {
            var headersAsString = string.Empty;
            var kvSeparator = "&";
            var headerSeparator = "?";
            foreach (var item in headers)
            {
                var key = Uri.EscapeDataString(item.Key);
                var value = Uri.EscapeDataString(item.Value);
                headersAsString += key + kvSeparator + value + headerSeparator;
            }
            return Encoding.UTF8.GetBytes(headersAsString);
        }

        public static Dictionary<string, string> Deserialize(byte[] headers)
        {
            var headersAsString = Encoding.UTF8.GetString(headers);
            var headersDictionary = new Dictionary<string, string>();
            foreach (var item in headersAsString.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var keyAndValue = item.Split(new char[] { '&' });
                var unescapedKey = Uri.UnescapeDataString(keyAndValue[0]);
                var unescapedValue = Uri.UnescapeDataString(keyAndValue[1]);
                headersDictionary.Add(unescapedKey, unescapedValue);
            }
            return headersDictionary;
        }
    }
}