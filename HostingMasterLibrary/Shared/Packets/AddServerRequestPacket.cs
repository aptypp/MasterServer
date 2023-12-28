using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
{
    [MessagePackObject]
    public class AddServerRequestPacket : IConvertableToNetwork
    {
        [Key(0)]
        public ushort port;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new();

            networkPacket.message = (ushort)Message.AddServerRequest;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}