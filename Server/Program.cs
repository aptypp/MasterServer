using HostingMasterLibrary.Server;

namespace Server
{
    public static class Program
    {
        private static void Main(string[] _)
        {
            MasterServer masterServer = new();

            Console.WriteLine("Server is started");
            Console.WriteLine("Press Enter to stop the Server");

            masterServer.Start();
            Console.ReadLine();
            masterServer.Stop();
        }
    }
}