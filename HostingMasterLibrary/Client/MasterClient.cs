using System.Net;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using HostingMasterLibrary.Server;
using HostingMasterLibrary.Shared;
using HostingMasterLibrary.Shared.Extensions;
using HostingMasterLibrary.Shared.Packets;
using MessagePack;

namespace HostingMasterLibrary.Client
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

        public async UniTask<RoomData[]> GetServerList()
        {
            try
            {
                await _client.SendAsync(new ServerListRequestPacket(), _masterEndPoint);

                UdpReceiveResult result = await _client.ReceiveAsync();

                NetworkPacket networkPacket = MessagePackSerializer.Deserialize<NetworkPacket>(result.Buffer);

                ServerListResponsePacket responsePacket =
                    await networkPacket.ConvertToPacketAsync<ServerListResponsePacket>();

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
            catch (Exception)
            {
                return [];
            }
        }

        public async UniTask AddServerToList(RoomData roomData)
        {
            AddServerRequestPacket packet = new();

            packet.port = roomData.port;

            await _client.SendAsync(packet, _masterEndPoint);
        }

        public async UniTask RemoveServerFromList(RoomData roomData)
        {
            RemoveServerRequestPacket packet = new();

            packet.port = roomData.port;

            await _client.SendAsync(packet, _masterEndPoint);
        }
    }
}