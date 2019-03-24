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
    class PacketSniffer
    {
        NetworkInterface Adapter { get; set; }
        private IPEndPoint endPoint;
        private ConcurrentQueue<byte[]> packetQueue = new ConcurrentQueue<byte[]>();
        private AutoResetEvent _queueNotifier = new AutoResetEvent(false);
        private ObservableCollection<IPacket> _packetCollection;


        volatile bool _shouldWork = true;

        public PacketSniffer(NetworkInterface adapter, ObservableCollection<IPacket> packetCollection)
        {
            Adapter = adapter;
            IPInterfaceProperties AdapterProperties = Adapter.GetIPProperties();
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

            while (_shouldWork)
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
            while (_shouldWork)
            {
                _queueNotifier.WaitOne();

                byte[] buffer;
                if (packetQueue.TryDequeue(out buffer))
                {
                    CreatePacketFromBuffer(buffer);
                }
            }
        }

        private void CreatePacketFromBuffer(byte[] buffer)
        {
            IAbstractBufferFactory packetFactory = new PacketFactory();

            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            IPacket packet = packetFactory.CreatePacket(buffer, binaryReader);
            _packetCollection.Add(packet);          
        }

        public void StopWorking()
        {
            _shouldWork = false;
        }
    }
}