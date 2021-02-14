using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using PacketSniffer.Factories;
using PacketSniffer.Packets;
using PacketSniffer.Socket;

namespace PacketSniffer.Services
{
    // TODO Clean this up
    // TODO Remove dependency on concrete factory, DI it instead and use interface
    //TODO Add interfaces to concrete classes, rework interfaces 
    public class PacketSniffer : IPacketSniffer
    {
        public ObservableCollection<IPacketFrame> Packets { get; }
        private readonly PacketQueue _packetQueue;
        
        private ISnifferSocket _socket;
        private readonly Thread _enqueueThread;
        private readonly Thread _dequeueThread; 
        

        //TODO Replace usage of concrete classes with interfaces
        public PacketSniffer(PacketQueue packetQueue)
        {
            Packets = new ObservableCollection<IPacketFrame>();
            
            _packetQueue = packetQueue;

            _enqueueThread = new Thread(EnqueueIncomingPackets);
            _dequeueThread = new Thread(DequeuePacketBuffer);
        }

        public void SniffPackets(NetworkInterface networkInterface)
        {         
            _socket = new SnifferSocket(AddressFamily.InterNetwork,
                SocketType.Raw,
                ProtocolType.IP,
                networkInterface,
                _packetQueue);


            _enqueueThread.Start();
            _dequeueThread.Start();
        }

        public void StopSniffing()
        {
            _enqueueThread.Join();
            _dequeueThread.Join();
        }

        private void EnqueueIncomingPackets()
        {
            _socket.ReceivePacket();
        }

        private void DequeuePacketBuffer()
        {
            var buffer = _packetQueue.DequeuePacket();

            IAbstractFrameFactory packetFactory = new PacketFactory(buffer);
            IPacketFrame packet = packetFactory.CreatePacketFrame();
            Packets.Add(packet);
        }
    }
}