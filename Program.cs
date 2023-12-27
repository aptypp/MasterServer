using System.Net;
using Cysharp.Threading.Tasks;

namespace MasterServers
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Console.WriteLine("| Server is 1 | Client is 2 |");

            string input = Console.ReadLine();

            if (input == "1")
            {
                HandleServer();
                return 0;
            }

            await HandleClient();
            return 0;
        }

        private static void HandleServer()
        {
            MasterServer masterServer = new();

            Console.WriteLine("Server is started\n\nPress Enter to stop the Server");

            masterServer.Start();
            Console.ReadLine();
            masterServer.Stop();
        }

        private static async UniTask HandleClient()
        {
            Console.WriteLine("Client is started\nPress enter to stop");

            MasterClient masterClient = new("127.0.0.1");

            RoomData[] list = await masterClient.GetServerList();

            PrintServers(list);

            Console.ReadLine();
        }

        private static void PrintServers(RoomData[] list)
        {
            Console.WriteLine("Server List: \n");

            for (int i = 0; i < list.Length; i++)
            {
                IPAddress ipAddress = new(list[i].address);

                Console.WriteLine($"{ipAddress}:{list[i].port}");
            }
        }
    }
}