using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace richClosure.Packet_Sniffing
{
    class SnifferSocket : Socket
    {
        private SocketConfigurator _socketConfigurator;

        private PacketQueue _packetQueue;

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

        public void ReceivePacket()
        {
            byte[] buffer = new byte[65565];
            Receive(buffer);

            _packetQueue.EnqueuePacket(buffer);
        }
    }
}
