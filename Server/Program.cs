using MasterServers;

namespace Server
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            HandleServer();
        }

        private static void HandleServer()
        {
            MasterServer masterServer = new();

            Console.WriteLine("Server is started\n\nPress Enter to stop the Server");

            masterServer.Start();
            Console.ReadLine();
            masterServer.Stop();
        }
    }
}