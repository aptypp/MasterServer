using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
{
    [MessagePackObject]
    public class ServerListResponsePacket : IConvertableToNetwork
    {
        [Key(0)]
        public long[] addresses;

        [Key(1)]
        public ushort[] ports;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new NetworkPacket();

            networkPacket.message = (ushort)Message.GetServerListResponse;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}