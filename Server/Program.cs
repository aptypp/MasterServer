using HostingMasterLibrary.Server;

namespace Server
{
    public static class Program
    {
        private static void Main(string[] _)
        {
            MasterServer masterServer = new();

            Console.WriteLine("Server is started");
            Console.WriteLine("Write 'stop' to stop and exit");

            masterServer.Start();

            while (true)
            {
                string? input = Console.ReadLine();

                if (input is not "stop") continue;

                Console.WriteLine("Server is stopped");
                break;
            }

            masterServer.Stop();
        }
    }
}