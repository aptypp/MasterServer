using MasterServers.Interfaces;
using MessagePack;

namespace MasterServers.Packets
{
    [MessagePackObject]
    public class ServerListResponsePacket : IConvertableToNetwork
    {
        [Key(0)]
        public long[] addresses;

        [Key(1)]
        public short[] ports;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new NetworkPacket();

            networkPacket.message = Message.GetServerListResponse;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}