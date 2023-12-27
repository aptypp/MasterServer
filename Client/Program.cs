using System.Net;
using Cysharp.Threading.Tasks;
using MasterServers;

namespace Client
{
    public static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            await HandleClient();
            return 0;
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