using System.Net;
using MasterServers.Packets;

namespace MasterServers
{
    public class MessageHandler<T> where T : struct
    {
        private readonly Dictionary<T, Action<NetworkPacket, IPEndPoint>> _dictionary = new();

        public void RegisterHandler(T key, Action<NetworkPacket, IPEndPoint> handler)
        {
            _dictionary.Add(key, handler);
        }

        public bool TryGetResolver(T key, out Action<NetworkPacket, IPEndPoint> resolver)
        {
            return _dictionary.TryGetValue(key, out resolver);
        }
    }
}