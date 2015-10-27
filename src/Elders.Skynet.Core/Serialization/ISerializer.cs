namespace Elders.Skynet.Core
{
    public interface ISerializer
    {
        byte[] Serialize(IMessage message);
        IMessage Deserialize(byte[] message);
    }
}