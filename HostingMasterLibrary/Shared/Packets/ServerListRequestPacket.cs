using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Packets
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