using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PacketSniffer.Socket
{
    internal class SocketConfigurator
    {
        private readonly NetworkInterface _networkInterface;
        private EndPoint _endPoint;

        private readonly System.Net.Sockets.Socket _socket;

        public SocketConfigurator(NetworkInterface networkInterface, System.Net.Sockets.Socket socket)
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

        private void SetSocketConfiguration()
        {
            _socket.Bind(_endPoint);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] inValue = { 1, 0, 0, 0 };
            byte[] outValue = { 0, 0, 0, 0 };
            // TODO Not supported on linux
            //_socket.IOControl(IOControlCode.ReceiveAll, inValue, outValue);
        }
    }
}
