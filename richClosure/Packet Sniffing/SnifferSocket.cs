using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace richClosure.Packet_Sniffing
{
    class SnifferSocket : Socket
    {
        private readonly SocketConfigurator _socketConfigurator;

        private readonly PacketQueue _packetQueue;

        public SnifferSocket(SocketType socketType, ProtocolType protocolType, NetworkInterface networkInterface, PacketQueue packetQueue)
            : base(socketType, protocolType)
        {
            _socketConfigurator = CreateSocketConfigurator(networkInterface);
            _socketConfigurator.ConfigureSocket();

            _packetQueue = packetQueue;
        }

        public SnifferSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, NetworkInterface networkInterface,
            PacketQueue packetQueue)
            : base(addressFamily, socketType, protocolType)
        {
            _socketConfigurator = CreateSocketConfigurator(networkInterface);
            _socketConfigurator.ConfigureSocket();

            _packetQueue = packetQueue;
        }

        public SnifferSocket(SocketInformation socketInformation, NetworkInterface networkInterface, PacketQueue packetQueue)
            : base(socketInformation)
        {
            _socketConfigurator = CreateSocketConfigurator(networkInterface);
            _socketConfigurator.ConfigureSocket();

            _packetQueue = packetQueue;
        }

        private SocketConfigurator CreateSocketConfigurator(NetworkInterface networkInterface)
        {
            return new SocketConfigurator(networkInterface, this);
        }

        /// <summary>
        /// Receives packet's data and enqueues it into PacketQueue
        /// </summary>
        public void ReceivePacket()
        {
            byte[] buffer = new byte[65565];
            Receive(buffer);

            _packetQueue.EnqueuePacket(buffer);
        }
    }
}
