using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
{
    [MessagePackObject]
    public class RemoveServerRequestPacket : IConvertableToNetwork
    {
        [Key(0)]
        public long address;

        [Key(1)]
        public short port;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new();

            networkPacket.message = Message.RemoveServerRequest;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}