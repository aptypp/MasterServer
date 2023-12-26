using MasterServers.Interfaces;
using MessagePack;

namespace MasterServers.Packets
{
    [MessagePackObject]
    public class ServerListRequestPacket : IConvertableToNetwork
    {
        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new();

            networkPacket.message = Message.GetServerListRequest;

            return networkPacket;
        }
    }
}