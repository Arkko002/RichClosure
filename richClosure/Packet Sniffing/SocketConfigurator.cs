using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace richClosure.Packet_Sniffing
{
    class SocketConfigurator
    {
        private NetworkInterface _networkInterface;
        private EndPoint _endPoint;

        private Socket _socket;

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
            IPInterfaceProperties AdapterProperties = _networkInterface.GetIPProperties();
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
    }
}
