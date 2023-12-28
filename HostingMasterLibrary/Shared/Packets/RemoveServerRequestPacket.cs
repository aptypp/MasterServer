using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
{
    [MessagePackObject]
    public class RemoveServerRequestPacket : IConvertableToNetwork
    {
        [Key(0)]
        public ushort port;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new();

            networkPacket.message = (ushort)Message.RemoveServerRequest;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}