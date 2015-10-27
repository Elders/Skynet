using System.Linq;
using Elders.Skynet.Core.Util;

namespace Elders.Skynet.Core
{
    public class JsonSerializer : ISerializer
    {
        public IMessage Deserialize(byte[] message)
        {
            var dict = BasicDictionarySerializer.Deserialize(message);
            var type = this.GetType().Assembly.GetTypes().Where(x => x.AssemblyQualifiedName == dict.Keys.First()).Single();
            return Newtonsoft.Json.JsonConvert.DeserializeObject(dict.Values.First(), type) as IMessage;
        }

        public byte[] Serialize(IMessage message)
        {
            var messageString = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var messageName = message.GetType().AssemblyQualifiedName;
            return BasicDictionarySerializer.Serialize(new System.Collections.Generic.Dictionary<string, string>() { { messageName, messageString } });
        }
    }
}