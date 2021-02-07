using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using PacketSniffer.Packets;

namespace PacketSniffer
{
    //TODO
    public interface IPacketSniffer
    {
        void SniffPackets(NetworkInterface networkInterface);
        void StopSniffing();
    }
}