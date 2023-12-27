using System.Net;

namespace MasterServers
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Server is 1\nClient is other");

            string input = Console.ReadLine();

            if (input == "1")
            {
                MasterServer masterServer = new();

                Console.WriteLine("Server is started\nPress enter to stop");

                masterServer.Start();
                Console.ReadLine();
                masterServer.Stop();
                return;
            }

            Console.WriteLine("Client is started\nPress enter to stop");

            HandleClient();
        }

        private static async void HandleClient()
        {
            MasterClient masterClient = new("127.0.0.1");

            RoomData[] list = await masterClient.GetServerList();

            PrintServers(list);

            Console.ReadLine();

            return;

            Random random = new Random();

            RoomData roomData = new RoomData();

            roomData.address = random.Next();
            roomData.port = (short)random.Next(0, short.MaxValue);

            await masterClient.AddServerToList(roomData);

            list = await masterClient.GetServerList();

            PrintServers(list);

            Console.WriteLine("Added server to List\nPress enter to remove server\n");

            Console.ReadLine();

            await masterClient.RemoveServerFromList(roomData);
        }

        private static void PrintServers(RoomData[] list)
        {
            Console.WriteLine("Server List: \n");

            for (int i = 0; i < list.Length; i++)
            {
                IPAddress ipAddress = new IPAddress(list[i].address);

                Console.WriteLine($"{ipAddress}:{list[i].port}");
            }
        }
    }
}