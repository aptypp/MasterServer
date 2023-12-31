﻿using MasterServers.Interfaces;
using MessagePack;

namespace MasterServers.Packets
{
    [MessagePackObject]
    public class AddServerRequestPacket : IConvertableToNetwork
    {
        [Key(0)]
        public long address;

        [Key(1)]
        public short port;

        public NetworkPacket ConvertToNetworkPacket()
        {
            NetworkPacket networkPacket = new();

            networkPacket.message = Message.AddServerRequest;
            networkPacket.data = MessagePackSerializer.Serialize(this);

            return networkPacket;
        }
    }
}