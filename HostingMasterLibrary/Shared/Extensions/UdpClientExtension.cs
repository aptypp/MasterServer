using System.Net;
using System.Net.Sockets;
using HostingMasterLibrary.Shared.Interfaces;
using MessagePack;

namespace HostingMasterLibrary.Shared.Extensions
{
    public static class UdpClientExtension
    {
        public static ValueTask<int> SendAsync(this UdpClient client, IConvertableToNetwork networkPacket,
            IPEndPoint ipEndPoint)
        {
            return client.SendAsync(MessagePackSerializer.Serialize(networkPacket.ConvertToNetworkPacket()),
                ipEndPoint);
        }
    }
}