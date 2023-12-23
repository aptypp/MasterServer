using System.Net;
using System.Net.Sockets;
using LiteNetLib.Utils;

namespace MasterServers
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string input = Console.ReadLine();

            if (int.Parse(input) == 1)
            {
                MasterServer masterServer = new MasterServer();

                masterServer.Start();
                Console.ReadLine();
                masterServer.Stop();
                return;
            }

            HandleClient();
            Console.ReadLine();
        }

        private static async void HandleClient()
        {
            MasterClient masterClient = new MasterClient("127.0.0.1");

            RoomData[] list = await masterClient.GetServerList();

            for (int i = 0; i < list.Length; i++)
            {
                Console.WriteLine($"{list[i].address.ToString()}:{list[i].port}\n");
            }
        }
    }

    public enum Message : byte
    {
        RequestServerList = 100,
        ResponseServerList = 101
    }

    public struct RoomDataStruct
    {
        public byte addressByte1;
        public byte addressByte2;
        public byte addressByte3;
        public byte addressByte4;
        public short port;

        public static int GetSize() => sizeof(byte) * 4 + sizeof(short);
    }

    public class RoomData
    {
        public IPAddress address;
        public short port;
    }

    public class MasterClient
    {
        private UdpClient _client;
        private IPEndPoint _masterEndPoint;

        public MasterClient(string masterAddress)
        {
            _client = new UdpClient();
            _masterEndPoint = new IPEndPoint(IPAddress.Parse(masterAddress), MasterServer.PORT);
        }

        public async Task<RoomData[]> GetServerList()
        {
            byte[] data = { (byte)Message.RequestServerList };

            await _client.SendAsync(data, 1, _masterEndPoint);

            UdpReceiveResult result = await _client.ReceiveAsync();

            byte[] buffer = result.Buffer;

            if ((Message)buffer[0] != Message.ResponseServerList) return Array.Empty<RoomData>();

            int roomsCount = BitConverter.ToInt32(buffer, 1);

            RoomData[] rooms = new RoomData[roomsCount];
            byte[] address = new byte[sizeof(int)];

            for (int roomIndex = 0; roomIndex < rooms.Length; roomIndex++)
            {
                address[0] = buffer[5 + roomIndex * RoomDataStruct.GetSize() + 0];
                address[1] = buffer[5 + roomIndex * RoomDataStruct.GetSize() + 1];
                address[2] = buffer[5 + roomIndex * RoomDataStruct.GetSize() + 2];
                address[3] = buffer[5 + roomIndex * RoomDataStruct.GetSize() + 3];

                RoomData roomData = new RoomData();

                roomData.address = new IPAddress(address);
                roomData.port = BitConverter.ToInt16(buffer, 5 + roomIndex * RoomDataStruct.GetSize() + 4);

                rooms[roomIndex] = roomData;
            }

            return rooms;
        }
    }

    internal class MasterServer
    {
        public const int PORT = 21783;

        private bool _isWorking;
        private readonly UdpClient _listener;
        private readonly List<RoomDataStruct> _rooms;

        public MasterServer()
        {
            _listener = new UdpClient(PORT, AddressFamily.InterNetwork);
            _rooms = new List<RoomDataStruct>();

            _rooms.Add(new RoomDataStruct
                { addressByte1 = 192, addressByte2 = 168, addressByte3 = 0, addressByte4 = 38, port = 9000 });
            _rooms.Add(new RoomDataStruct
                { addressByte1 = 192, addressByte2 = 168, addressByte3 = 0, addressByte4 = 39, port = 9001 });
        }

        public void Start()
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
                UdpReceiveResult result = await _listener.ReceiveAsync();
                HandleRequest(result);
            }
        }

        private async void HandleRequest(UdpReceiveResult result)
        {
            if ((Message)result.Buffer[0] != Message.RequestServerList) return;

            int bufferSize = sizeof(byte) + sizeof(int) + RoomDataStruct.GetSize() * _rooms.Count;

            byte[] buffer = new byte[bufferSize];

            buffer[0] = (byte)Message.ResponseServerList;
            byte[] roomsCountData = BitConverter.GetBytes(_rooms.Count);

            Array.ConstrainedCopy(roomsCountData, 0, buffer, 1, sizeof(int));

            for (int roomIndex = 0; roomIndex < _rooms.Count; roomIndex++)
            {
                RoomDataStruct roomDataStruct = _rooms[roomIndex];

                buffer[5 + roomIndex * RoomDataStruct.GetSize() + 0] = roomDataStruct.addressByte1;
                buffer[5 + roomIndex * RoomDataStruct.GetSize() + 1] = roomDataStruct.addressByte2;
                buffer[5 + roomIndex * RoomDataStruct.GetSize() + 2] = roomDataStruct.addressByte3;
                buffer[5 + roomIndex * RoomDataStruct.GetSize() + 3] = roomDataStruct.addressByte4;

                int portIndex = 5 + roomIndex * RoomDataStruct.GetSize() + 4;

                Array.ConstrainedCopy(BitConverter.GetBytes(roomDataStruct.port), 0, buffer,
                    portIndex, sizeof(short));
            }

            await _listener.SendAsync(buffer, buffer.Length, result.RemoteEndPoint);
        }
    }
}