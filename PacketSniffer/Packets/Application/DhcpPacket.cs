using System.Collections.Generic;
using PacketSniffer.Packets.Transport_Layer;

namespace PacketSniffer.Packets.Application_Layer
{
    public enum DhcpOpcodeEnum
    {
        Bootrequest = 1,
        Bootreply = 2
    };

    public enum DhcpHardwareTypeEnum
    {
        Ethernet = 1,
        ExperimentalEthernet = 2,
        AmateurRadioAx25 = 3,
        ProtenProNetTokenRing = 4,
        Chaos = 5,
        Ieee802 = 6,
        Arcnet = 7,
        Hyperchannel = 8,
        Lanstar = 9,
        AutonetShortAddress = 10,
        LocalTalk = 11,
        LocalNet = 12,
        UltraLink = 13,
        Smds = 14,
        FameRelay = 15,
        Atm = 16,
        Hdlc = 17,
        FibreChannel = 18,
        SerialLine = 20,
        MilStd188220 = 22,
        Metricom = 23,
        Ieee13941995 = 24,
        Mapos = 25,
        Twinaxial = 26,
        Eui64 = 27,
        Hiparp = 28,
        IpAndArpOverIso78163 = 29,
        ArpSec = 30,
        PsecTunnel = 31,
        Infiniband = 32,
        Cai = 33,
        WiegandInterface = 34,
        PureIp = 35
    };

    public enum DhcpOptionsTagEnum
    {
        Pad = 0,
        SubnetMask = 1,
        TimeOffset = 2,
        Router = 3,
        TimeServer = 4,
        NameServer = 5,
        DomainServer = 6,
        LogServer = 7,
        QuotesServer = 8,
        LprServer = 9,
        ImpressServer = 10,
        RlpServer = 11,
        Hostname = 12,
        BootFileSize = 13,
        MeritDumpFile = 14,
        DomainName = 15,
        SwapServer = 16,
        RootPath = 17,
        ExtensionFile = 18,
        ForwardOnOff = 19,
        SrcRteOnOff = 20,
        PolicyFilter = 21,
        MaxDgAssembly = 22,
        DefaultIpttl = 23,
        MtuTimeout = 24,
        MtuPlateau = 25,
        MtuInterface = 26,
        MtuSubnet = 27,
        BroadcastAddress = 28,
        MaskDiscovery = 29,
        MaskSupplier = 30,
        RouterDiscover = 30,
        RouterRequest = 32,
        StaticRoute = 33,
        Trailers = 34,
        ArpTimeout = 35,
        Ethernet = 36,
        DefaultTcpttl = 37,
        KeepaliveTime = 38,
        KeepaliveData = 39,
        NisDomain = 40,
    };

    public class DhcpPacket : UdpPacket
    {
        public DhcpOpcodeEnum DhcpOpcode { get; private set; }
        public DhcpHardwareTypeEnum DhcpHardType { get; private set; }
        public byte DhcpHardAdrLength { get; private set; }
        public byte DhcpHopCount { get; private set; }
        public uint DhcpTransactionId { get; private set; }
        public ushort DhcpNumOfSeconds { get; private set; }
        public string DhcpFlags { get; private set; }
        public string DhcpClientIpAdr { get; private set; }
        public string DhcpYourIpAdr { get; private set; }
        public string DhcpServerIpAdr { get; private set; }
        public string DhcpGatewayIpAdr { get; private set; }
        public string DhcpClientHardAdr { get; private set; }
        public string DhcpServerName { get; private set; }
        public string DhcpBootFilename { get; private set; }

        public DhcpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetDhcpPacketValues(valuesDictionary);
            SetDisplayedProtocol("DHCP");
        }

        private void SetDhcpPacketValues(Dictionary<string, object> valuesDictionary)
        {
            DhcpOpcode = (DhcpOpcodeEnum)valuesDictionary["DhcpOpcode"];
            DhcpHardType = (DhcpHardwareTypeEnum)valuesDictionary["DhcpHardType"];
            DhcpHopCount = (byte)valuesDictionary["DhcpHopCount"];
            DhcpHardAdrLength = (byte) valuesDictionary["DhcpHardAdrLength"];
            DhcpTransactionId = (uint)valuesDictionary["DhcpTransactionId"];
            DhcpNumOfSeconds = (ushort)valuesDictionary["DhcpNumOfSeconds"];
            DhcpFlags = (string)valuesDictionary["DhcpFLags"];
            DhcpClientIpAdr = (string)valuesDictionary["DhcpClientIpAdr"];
            DhcpYourIpAdr = (string)valuesDictionary["DhcpYourIpAdr"];
            DhcpServerIpAdr = (string)valuesDictionary["DhcpServerIpAdr"];
            DhcpGatewayIpAdr = (string)valuesDictionary["DhcpGatewayIpAdr"];
            DhcpClientHardAdr = (string)valuesDictionary["DhcpClientHardAdr"];
            DhcpServerName = (string)valuesDictionary["DhcpServerName"];
            DhcpBootFilename = (string)valuesDictionary["DhcpBootFilename"];
        }
    }
}
