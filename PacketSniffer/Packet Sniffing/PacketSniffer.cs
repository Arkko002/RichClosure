using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using PacketSniffer.Packet_Sniffing.Packet_Factories;
using PacketSniffer.Packet_Sniffing.Packet_Factories.AbstractFactories;
using PacketSniffer.Packets;

namespace PacketSniffer.Packet_Sniffing
{
    // TODO Make this highest level of abstraction (like in factories)
    // TODO PacketFactory DI, maybe separate class for passing socket data to factory?
    // TODO Clean this up
    // TODO Remove dependency on concrete factory, DI it instead and use interface
    public class PacketSnifferService
    {
        private readonly ObservableCollection<IPacket> _packetCollection;

        private SnifferSocket _socket;

        private SnifferThreads _snifferThreads;

        private readonly PacketQueue _packetQueue;

        private IAbstractByteFactory _packetByteFactory;

        public bool IsWorking { get; set; }

        //TODO Replace usage of concrete classes with interfaces
        public PacketSnifferService(ObservableCollection<IPacket> packetCollection,
            PacketQueue packetQueue,
            SnifferThreads snifferThreads,
            IAbstractByteFactory packetByteFactory)
        {       
            _packetCollection = packetCollection;
            _packetQueue = packetQueue;
            _snifferThreads = snifferThreads;
            _packetByteFactory = packetByteFactory;
        }

        public void SniffPackets(NetworkInterface networkInterface)
        {         
            _socket = new SnifferSocket(AddressFamily.InterNetwork,
                SocketType.Raw,
                ProtocolType.IP,
                networkInterface,
                _packetQueue);

            IsWorking = true;

            _snifferThreads = new SnifferThreads();
            _snifferThreads.AssignMethodsToThreads(EnqueueIncomingPackets, DequeuePacketBuffer);
            _snifferThreads.StartThreads();
        }

        public void StopSniffing()
        {
            IsWorking = false;
        }

        private void EnqueueIncomingPackets()
        {
            while (IsWorking)
            {
                _socket.ReceivePacket();
            }
        }

        private void DequeuePacketBuffer()
        {
            while (IsWorking)
            {
                var buffer = _packetQueue.DequeuePacket();
                
                IPacket packet = _packetByteFactory.CreatePacket(buffer);
                _packetCollection.Add(packet);
            }
        }
    }
}