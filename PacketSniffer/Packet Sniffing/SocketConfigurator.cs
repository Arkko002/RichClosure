using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PacketSniffer.Packet_Sniffing
{
    internal class SocketConfigurator
    {
        private readonly NetworkInterface _networkInterface;
        private EndPoint _endPoint;

        private readonly Socket _socket;

        public SocketConfigurator(NetworkInterface networkInterface, Socket socket)
        {
            _networkInterface = networkInterface;
            _socket = socket;
        }

        public void ConfigureSocket()
        {
            var ipInfo = GetInterfaceIpInfromation();
            GetInterfaceEndpoints(ipInfo);

            SetSocketConfiguration();
        }

        private void SetSocketConfiguration()
        {
            _socket.Bind(_endPoint);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] inValue = new byte[] { 1, 0, 0, 0 };
            byte[] outValue = new byte[] { 0, 0, 0, 0 };
            _socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);
        }

        private UnicastIPAddressInformationCollection GetInterfaceIpInfromation()
        {
            IPInterfaceProperties adapterProperties = _networkInterface.GetIPProperties();
            return adapterProperties.UnicastAddresses;
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
    }
}
