using MasterServers.Packets;

namespace MasterServers.Interfaces
{
    public interface IConvertableToNetwork
    {
        NetworkPacket ConvertToNetworkPacket();
    }
}