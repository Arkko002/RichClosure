using System;
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
    //TODO Clean this up
    public class PacketSniffer
    {
        private IPEndPoint _endPoint;
        private ConcurrentQueue<byte[]> _packetQueue = new ConcurrentQueue<byte[]>();
        private AutoResetEvent _queueNotifier = new AutoResetEvent(false);

        private ObservableCollection<IPacket> _packetCollection;

        private Socket _socket;
        public bool IsWorking { get; set; } = false;

        private Thread _enqueuThread;
        private Thread _dequeuThread;

        public PacketSniffer(ObservableCollection<IPacket> packetCollection)
        {       
            _packetCollection = packetCollection;
        }

        public void SniffPackets(NetworkInterface networkInterface)
        {
            var ipInformations = GetInterfaceIpInfromation(networkInterface);
            GetInterfaceEndpoints(ipInformations);

            _socket = CreateSocket();

            IsWorking = true;

            StartQueueThreads();
        }

        public void StopSniffing()
        {
            IsWorking = false;
        }

        private Socket CreateSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            return ConfigureSocket(socket);
        }

        private Socket ConfigureSocket(Socket socket)
        {
            socket.Bind(_endPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] inValue = new byte[] { 1, 0, 0, 0 };
            byte[] outValue = new byte[] { 0, 0, 0, 0 };
            socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);

            return socket;
        }

        public UnicastIPAddressInformationCollection GetInterfaceIpInfromation(NetworkInterface networkInterface)
        {
            IPInterfaceProperties AdapterProperties = networkInterface.GetIPProperties();
            return AdapterProperties.UnicastAddresses;
        }

        private void GetInterfaceEndpoints(UnicastIPAddressInformationCollection unicastIps)
        {
            foreach (var adr in unicastIps)
            {
                if (adr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    _endPoint = new IPEndPoint(adr.Address, 0);
                    break;
                }
            }
        }

        private void StartQueueThreads()
        {
            _enqueuThread = new Thread(EnqueueLoop);
            _dequeuThread = new Thread(DequeueLoop);

            _enqueuThread.Start();
            _dequeuThread.Start();            
        }

        private void AbortQueueThreads()
        {
            try
            {
                _dequeuThread.Abort();
                _enqueuThread.Abort();
            }
            catch (ThreadAbortException e)
            {
                
            }
        }

        private void EnqueueLoop()
        {
            while (IsWorking)
            {
                EnqueueIncomingPackets(_socket);
            }

            AbortQueueThreads();
        }

        public void DequeueLoop()
        {
            while (IsWorking)
            {
                _queueNotifier.WaitOne();

                if (_packetQueue.TryDequeue(out var buffer))
                {
                    CreatePacketFromBuffer(buffer);
                }
            }
        }

        private void EnqueueIncomingPackets(Socket socket)
        {
            byte[] buffer = new byte[65565];
            socket.Receive(buffer);
            _packetQueue.Enqueue(buffer);
            _queueNotifier.Set();
        }

        private void CreatePacketFromBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            IAbstractFactory packetFactory = new PacketFactory(binaryReader, buffer);

            IPacket packet = packetFactory.CreatePacket();
            _packetCollection.Add(packet);
        }
    }
}