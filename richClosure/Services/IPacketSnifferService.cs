using System.Collections.ObjectModel;
using PacketSniffer.Packets;

namespace richClosure.Services
{
    //TODO
    /// <summary>
    /// Interface used to provide contract for sniffing packets, and decouple GUI from sniffing implementation
    /// </summary>
    public interface IPacketSnifferService
    {
        void SniffPackets(ObservableCollection<IPacket> packetCollection);
    }
}