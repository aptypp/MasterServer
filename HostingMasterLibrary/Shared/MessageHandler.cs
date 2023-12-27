using System.Net;
using Cysharp.Threading.Tasks;
using HostingMasterLibrary.Shared.Packets;

namespace HostingMasterLibrary.Shared
{
    public class MessageHandler<T> where T : struct
    {
        private readonly Dictionary<T, Func<NetworkPacket, IPEndPoint, UniTask>> _dictionary = new();

        public void RegisterHandler(T key, Func<NetworkPacket, IPEndPoint, UniTask> handler)
        {
            _dictionary.Add(key, handler);
        }

        public bool TryGetResolver(T key, out Func<NetworkPacket, IPEndPoint, UniTask> resolver)
        {
            return _dictionary.TryGetValue(key, out resolver);
        }
    }
}