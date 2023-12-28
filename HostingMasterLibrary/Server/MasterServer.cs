using System.Net;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using HostingMasterLibrary.Shared;
using HostingMasterLibrary.Shared.Extensions;
using HostingMasterLibrary.Shared.Packets;
using MessagePack;

namespace HostingMasterLibrary.Server
{
    public class MasterServer
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
        }

        public void Start()
        {
            _isWorking = true;
            ListenRequests().Forget();
        }

        public void Stop()
        {
            _isWorking = false;
        }

        private async UniTask ListenRequests()
        {
            while (_isWorking)
            {
                try
                {
                    UdpReceiveResult result = await _listener.ReceiveAsync();
                    HandleRequest(result).Forget();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async UniTask HandleRequest(UdpReceiveResult result)
        {
            NetworkPacket networkPacket = MessagePackSerializer.Deserialize<NetworkPacket>(result.Buffer);

            if (!_messageHandler.TryGetResolver(networkPacket.message,
                    out Func<NetworkPacket, IPEndPoint, UniTask> resolver))
                return;

            await resolver(networkPacket, result.RemoteEndPoint);
        }

        private async UniTask GetServerListRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            ServerListResponsePacket responsePacket = new();

            responsePacket.addresses = new long[_rooms.Count];
            responsePacket.ports = new ushort[_rooms.Count];

            for (int roomIndex = 0; roomIndex < _rooms.Count; roomIndex++)
            {
                responsePacket.addresses[roomIndex] = _rooms[roomIndex].address;
                responsePacket.ports[roomIndex] = _rooms[roomIndex].port;
            }

            await _listener.SendAsync(responsePacket, remoteEndPoint);
        }

        private async UniTask AddServerRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            AddServerRequestPacket packet = await networkPacket.ConvertToPacketAsync<AddServerRequestPacket>();

            _rooms.Add(new RoomData { address = remoteEndPoint.Address.Address, port = packet.port });
        }

        private async UniTask RemoveServerRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            RemoveServerRequestPacket packet = await networkPacket.ConvertToPacketAsync<RemoveServerRequestPacket>();

            _rooms.Remove(new RoomData { address = remoteEndPoint.Address.Address, port = packet.port });
        }

        private async UniTask GetServerByIdRequest(NetworkPacket networkPacket, IPEndPoint remoteEndPoint)
        {
            await UniTask.Yield();
        }
    }
}