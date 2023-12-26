namespace MasterServers
{
    public struct RoomData : IEquatable<RoomData>
    {
        public long address;
        public short port;

        public bool Equals(RoomData other)
        {
            return address == other.address && port == other.port;
        }

        public override bool Equals(object? obj)
        {
            return obj is RoomData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(address, port);
        }
    }
}