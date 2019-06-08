using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using richClosure.Packet_Sniffing.Factories;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;
using richClosure.Packet_Sniffing;

namespace richClosure
{
    //TODO Make this highest level of abstraction (like in factories)
    //TODO PacketFactory DI, maybe separate class for passing socket data to factory?
    //TODO Clean this up
    public class PacketSniffer
    {
        private ObservableCollection<IPacket> _packetCollection;
        private PacketFactory _packetFactory;

        private SnifferSocket _socket;

        private PacketQueue _packetQueue;

        private SnifferThreads _snifferThreads;
        public bool IsWorking { get; set; }

        public PacketSniffer(ObservableCollection<IPacket> packetCollection)
        {       
            _packetCollection = packetCollection;
            _packetQueue = new PacketQueue();
            _snifferThreads = new SnifferThreads(EnqueueIncomingPackets, CreatePacketFromBuffer);
        }

        public void SniffPackets(NetworkInterface networkInterface)
        {         
            _socket = new SnifferSocket(AddressFamily.InterNetwork,
                SocketType.Raw,
                ProtocolType.IP,
                networkInterface,
                _packetQueue);

            IsWorking = true;
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

        private void CreatePacketFromBuffer()
        {
            while (IsWorking)
            {
                var buffer = _packetQueue.DequeuePacket();

                MemoryStream memoryStream = new MemoryStream(buffer);
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                IAbstractFactory packetFactory = new PacketFactory(binaryReader, buffer);

                IPacket packet = packetFactory.CreatePacket();
                _packetCollection.Add(packet);
            }
        }
    }
}