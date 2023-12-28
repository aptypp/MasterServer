using System.Net;
using HostingMasterLibrary.Client;
using HostingMasterLibrary.Shared;

namespace Client
{
    public static class Program
    {
        private static async Task<int> Main(string[] _)
        {
            Console.WriteLine("Client is started");

            MasterClient masterClient = new("127.0.0.1");

            RoomData roomData = new();

            roomData.address = Random.Shared.Next();
            roomData.port = (ushort)Random.Shared.Next(0, short.MaxValue);

            await masterClient.AddServerToList(roomData);

            RoomData[] list = await masterClient.GetServerList();

            PrintServers(list);

            Console.WriteLine("Press enter to stop");
            Console.ReadLine();
            return 0;
        }

        private static void PrintServers(RoomData[] list)
        {
            Console.WriteLine("Server List: \n");

            for (int i = 0; i < list.Length; i++)
            {
                IPAddress ipAddress = new(list[i].address);

                Console.WriteLine($"{ipAddress}:{list[i].port}");
            }

            Console.WriteLine();
        }
    }
}