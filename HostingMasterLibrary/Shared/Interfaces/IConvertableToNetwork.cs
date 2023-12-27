using HostingMasterLibrary.Shared.Packets;

namespace HostingMasterLibrary.Shared.Interfaces
{
    public interface IConvertableToNetwork
    {
        NetworkPacket ConvertToNetworkPacket();
    }
}