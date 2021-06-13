using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PacketSniffer.Socket
{
    internal class SnifferSocket : System.Net.Sockets.Socket, ISnifferSocket
    {
        private readonly IPacketQueue _packetQueue;

        public SnifferSocket(SocketType socketType, ProtocolType protocolType, NetworkInterface networkInterface, IPacketQueue packetQueue)
            : base(socketType, protocolType)
        {
            var socketConfigurator = CreateSocketConfigurator(networkInterface);
            socketConfigurator.ConfigureSocket();

            _packetQueue = packetQueue;
        }

        public SnifferSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, NetworkInterface networkInterface,
            IPacketQueue packetQueue)
            : base(addressFamily, socketType, protocolType)
        {
            var socketConfigurator = CreateSocketConfigurator(networkInterface);
            socketConfigurator.ConfigureSocket();

            _packetQueue = packetQueue;
        }

        public SnifferSocket(SocketInformation socketInformation, NetworkInterface networkInterface, IPacketQueue packetQueue)
            : base(socketInformation)
        {
            var socketConfigurator = CreateSocketConfigurator(networkInterface);
            socketConfigurator.ConfigureSocket();

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
