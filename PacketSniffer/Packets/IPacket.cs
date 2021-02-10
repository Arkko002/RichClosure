#nullable enable
using System;
using System.Collections.Generic;

namespace PacketSniffer.Packets
{
    public interface IPacket
    {
        PacketProtocol PacketProtocol { get; }
        IPacket? PreviousHeader { get; }
        PacketProtocol NextProtocol { get; }
    }
}