using MessagePack;

namespace MasterServers.Packets
{
    [MessagePackObject]
    public class NetworkPacket
    {
        [Key(0)]
        public Message message;

        [Key(1)]
        public byte[] data;

        public T ConvertToPacket<T>() => MessagePackSerializer.Deserialize<T>(data);
    }
}