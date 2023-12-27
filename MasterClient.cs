using System.Net;
using System.Net.Sockets;
using MasterServers.Extensions;
using MasterServers.Packets;
using MessagePack;

namespace MasterServers
{
    public class MasterClient
    {
        private readonly UdpClient _client;
        private readonly IPEndPoint _masterEndPoint;

        public MasterClient(string masterAddress)
        {
            _client = new UdpClient();
            _masterEndPoint = new IPEndPoint(IPAddress.Parse(masterAddress), MasterServer.PORT);
        }

        public async Task<RoomData[]> GetServerList()
        {
            await _client.SendAsync(new ServerListRequestPacket(), _masterEndPoint);

            UdpReceiveResult result = await _client.ReceiveAsync();

            NetworkPacket networkPacket = MessagePackSerializer.Deserialize<NetworkPacket>(result.Buffer);

            ServerListResponsePacket responsePacket = networkPacket.ConvertToPacket<ServerListResponsePacket>();

            if (responsePacket.addresses.Length == 0) return Array.Empty<RoomData>();

            RoomData[] rooms = new RoomData[responsePacket.addresses.Length];

            for (int roomIndex = 0; roomIndex < rooms.Length; roomIndex++)
            {
                RoomData roomData = new();

                roomData.address = responsePacket.addresses[roomIndex];
                roomData.port = responsePacket.ports[roomIndex];

                rooms[roomIndex] = roomData;
            }

            return rooms.ToArray();
        }

        public async Task AddServerToList(RoomData roomData)
        {
            AddServerRequestPacket packet = new();

            packet.address = roomData.address;
            packet.port = roomData.port;

            await _client.SendAsync(packet, _masterEndPoint).ConfigureAwait(false);
        }

        public async Task RemoveServerFromList(RoomData roomData)
        {
            RemoveServerRequestPacket packet = new();

            packet.address = roomData.address;
            packet.port = roomData.port;

            await _client.SendAsync(packet, _masterEndPoint).ConfigureAwait(false);
        }
    }
}