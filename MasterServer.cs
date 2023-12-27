using System.Net;
using System.Net.Sockets;
using MasterServers.Extensions;
using MasterServers.Packets;
using MessagePack;

namespace MasterServers
{
    internal class MasterServer
    {
        public const int PORT = 21784;

        private bool _isWorking;
        private readonly UdpClient _listener;
        private readonly List<RoomData> _rooms;
        private readonly MessageHandler<Message> _messageHandler;

        public MasterServer()
        {
            _listener = new UdpClient(PORT, AddressFamily.InterNetwork);
            _rooms = new List<RoomData>();
            _messageHandler = new MessageHandler<Message>();

            _messageHandler.RegisterHandler(Message.AddServerRequest, AddServerRequest);
            _messageHandler.RegisterHandler(Message.RemoveServerRequest, RemoveServerRequest);
            _messageHandler.RegisterHandler(Message.GetServerListRequest, GetServerListRequest);
            _messageHandler.RegisterHandler(Message.GetServerByIdRequest, GetServerByIdRequest);

            _rooms.Add(new RoomData { address = 10000, port = 1235 });
            _rooms.Add(new RoomData { address = 20000, port = 1234 });
            _rooms.Add(new RoomData { address = 40000, port = 1233 });
        }

        public void  Start()
        {
            _isWorking = true;
            ListenRequests();
        }

        public void Stop()
        {
            _isWorking = false;
        }

        private async void ListenRequests()
        {
            while (_isWorking)
            {
                try
                {
                    UdpReceiveResult result = await _listener.ReceiveAsync().ConfigureAwait(false);
                    HandleRequest(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void HandleRequest(UdpReceiveResult result)
        {
            NetworkPacket networkPacket = MessagePackSerializer.Deserialize<NetworkPacket>(result.Buffer);

            if (!_messageHandler.TryGetResolver(networkPacket.message,
                    out Action<NetworkPacket, IPEndPoint> resolver))
                return;

            resolver(networkPacket, result.RemoteEndPoint);
        }

        private async void GetServerListRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            ServerListResponsePacket responsePacket = new();

            responsePacket.addresses = new long[_rooms.Count];
            responsePacket.ports = new short[_rooms.Count];

            for (int roomIndex = 0; roomIndex < _rooms.Count; roomIndex++)
            {
                responsePacket.addresses[roomIndex] = _rooms[roomIndex].address;
                responsePacket.ports[roomIndex] = _rooms[roomIndex].port;
            }

            await _listener.SendAsync(responsePacket, remoteEndPoint);
        }

        private async void AddServerRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            AddServerRequestPacket packet = networkPacket.ConvertToPacket<AddServerRequestPacket>();

            _rooms.Add(new RoomData { address = packet.address, port = packet.port });
        }

        private async void RemoveServerRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            RemoveServerRequestPacket packet = networkPacket.ConvertToPacket<RemoveServerRequestPacket>();

            _rooms.Remove(new RoomData { address = packet.address, port = packet.port });
        }

        private async void GetServerByIdRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
        }
    }
}