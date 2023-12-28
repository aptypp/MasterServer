using Cysharp.Threading.Tasks;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
{
    [MessagePackObject]
    public class NetworkPacket
    {
        [Key(0)]
        public ushort message;

        [Key(1)]
        public byte[] data;

        public T ConvertToPacket<T>() => MessagePackSerializer.Deserialize<T>(data);

        public async UniTask<T> ConvertToPacketAsync<T>()
        {
            using MemoryStream stream = new(data);

            return await MessagePackSerializer.DeserializeAsync<T>(stream);
        }
    }
}