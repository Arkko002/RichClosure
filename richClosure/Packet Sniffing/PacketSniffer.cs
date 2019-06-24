using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using richClosure.Packet_Sniffing.Packet_Factories;
using richClosure.Packets;

namespace richClosure.Packet_Sniffing
{
    // TODO Make this highest level of abstraction (like in factories)
    // TODO PacketFactory DI, maybe separate class for passing socket data to factory?
    // TODO Clean this up
    public class PacketSniffer
    {
        private readonly ObservableCollection<IPacket> _packetCollection;

        private SnifferSocket _socket;

        private SnifferThreads _snifferThreads;

        private readonly PacketQueue _packetQueue;

        public bool IsWorking { get; set; }

        public PacketSniffer(ObservableCollection<IPacket> packetCollection, PacketQueue packetQueue, SnifferThreads snifferThreads)
        {       
            _packetCollection = packetCollection;
            _packetQueue = packetQueue;
            _snifferThreads = snifferThreads;
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

                IAbstractFactory packetFactory = CreatePacketFactory(buffer);

                IPacket packet = packetFactory.CreatePacket();
                _packetCollection.Add(packet);
            }
        }

        private IAbstractFactory CreatePacketFactory(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            return new PacketFactory(binaryReader, buffer);
        }
    }
}