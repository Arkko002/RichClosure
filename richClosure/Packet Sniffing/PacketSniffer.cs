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

        volatile bool _shouldWork = true;

        public PacketSniffer(NetworkInterface adapter)
        {
            Adapter = adapter;
            IPInterfaceProperties AdapterProperties = Adapter.GetIPProperties();
            UnicastIPAddressInformationCollection unicastIPs = AdapterProperties.UnicastAddresses;

            foreach (var adr in unicastIPs)
            {
                if (adr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    endPoint = new IPEndPoint(adr.Address, 0);
                    break;
                }
            }            
        }

        public void SniffPackets(ObservableCollection<IPacket> packetList)
        { 
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Raw, ProtocolType.IP);
            socket.Bind(endPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] inValue = new byte[] { 1, 0, 0, 0 };
            byte[] outValue = new byte[] { 0, 0, 0, 0 };
            socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);

            byte[] buffer = new byte[65565];

            while (_shouldWork)
            {
                socket.Receive(buffer);
                packetQueue.Enqueue(buffer);
                _queueNotifier.Set();
            }
        }

        public void CreatePacketsFromQueue(ObservableCollection<IPacket> packetList)
        {
            IAbstractBufferFactory packetFactory = new PacketFactory();

            while (_shouldWork)
            {
                _queueNotifier.WaitOne();

                byte[] buffer;
                if (packetQueue.TryDequeue(out buffer))
                {
                    MemoryStream memoryStream = new MemoryStream(buffer);
                    BinaryReader binaryReader = new BinaryReader(memoryStream);

                    IPacket packet = packetFactory.CreatePacket(buffer, binaryReader);
                    packetList.Add(packet);
                }
            }
        }

        public void StopWorking()
        {
            _shouldWork = false;
        }
    }
}