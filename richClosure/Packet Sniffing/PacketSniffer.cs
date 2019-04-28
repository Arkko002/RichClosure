using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using richClosure.Packet_Sniffing.Factories;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;

namespace richClosure
{
    public class PacketSniffer
    {
        NetworkInterface NetInterface { get; }
        private IPEndPoint endPoint;
        private ConcurrentQueue<byte[]> packetQueue = new ConcurrentQueue<byte[]>();
        private AutoResetEvent _queueNotifier = new AutoResetEvent(false);
        private ObservableCollection<IPacket> _packetCollection;


        public bool IsWorking { get; set; } = true;

        public PacketSniffer(NetworkInterface netInterface, ObservableCollection<IPacket> packetCollection)
        {
            NetInterface = netInterface;
            IPInterfaceProperties AdapterProperties = NetInterface.GetIPProperties();
            UnicastIPAddressInformationCollection unicastIPs = AdapterProperties.UnicastAddresses;
            _packetCollection = packetCollection;

            GetAdapterEndpoint(unicastIPs);

        }

        private void GetAdapterEndpoint(UnicastIPAddressInformationCollection unicastIps)
        {
            foreach (var adr in unicastIps)
            {
                if (adr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    endPoint = new IPEndPoint(adr.Address, 0);
                    break;
                }
            }
        }

        public void SniffPackets()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            socket = ConfigureSocket(socket);

            while (IsWorking)
            {
                EnqueueIncomingPackets(socket);
            }
        }

        private void EnqueueIncomingPackets(Socket socket)
        {
            byte[] buffer = new byte[65565];
            socket.Receive(buffer);
            packetQueue.Enqueue(buffer);
            _queueNotifier.Set();
        }

        private Socket ConfigureSocket(Socket socket)
        {
            socket.Bind(endPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] inValue = new byte[] { 1, 0, 0, 0 };
            byte[] outValue = new byte[] { 0, 0, 0, 0 };
            socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);

            return socket;
        }

        public void GetPacketDataFromQueue()
        {
            while (IsWorking)
            {
                _queueNotifier.WaitOne();

                if (packetQueue.TryDequeue(out var buffer))
                {
                    CreatePacketFromBuffer(buffer);
                }
            }
        }

        private void CreatePacketFromBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            IAbstractFactory packetFactory = new PacketFactory(binaryReader, buffer);

            IPacket packet = packetFactory.CreatePacket();
            _packetCollection.Add(packet);
        }

        public void StopWorking()
        {
            IsWorking = false;
        }
    }
}