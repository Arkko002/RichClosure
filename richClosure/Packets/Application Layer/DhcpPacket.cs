using System;
using richClosure.Packets.TransportLayer;

namespace richClosure.Packets.ApplicationLayer
{
    public enum DhcpOpcodeEnum
    {
        BOOTREQUEST = 1,
        BOOTREPLY = 2
    };

    public enum DhcpHardwareTypeEnum
    {
        Ethernet = 1,
        Experimental_Ethernet = 2,
        Amateur_RadioAX25 = 3,
        Proten_ProNET_TokenRing = 4,
        Chaos = 5,
        IEEE802 = 6,
        ARCNET = 7,
        Hyperchannel = 8,
        Lanstar = 9,
        Autonet_Short_Address = 10,
        LocalTalk = 11,
        LocalNet = 12,
        UltraLink = 13,
        SMDS = 14,
        Fame_Relay = 15,
        ATM = 16,
        HDLC = 17,
        Fibre_Channel = 18,
        Serial_Line = 20,
        MIL_STD_188_220 = 22,
        Metricom = 23,
        IEEE_1394_1995 = 24,
        MAPOS = 25,
        Twinaxial = 26,
        EUI_64 = 27,
        HIPARP = 28,
        IP_and_ARP_over_ISO_7816_3 = 29,
        ARPSec = 30,
        IPsec_tunnel = 31,
        Infiniband = 32,
        CAI = 33,
        Wiegand_Interface = 34,
        Pure_IP = 35        
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
        LPRServer = 9,
        ImpressServer = 10,
        RLPServer = 11,
        Hostname = 12,
        BootFileSize = 13,
        MeritDumpFile = 14,
        DomainName = 15,
        SwapServer = 16,
        RootPath = 17,
        ExtensionFile = 18,
        ForwardOn_Off = 19,
        SrcRteOn_Off = 20,
        PolicyFilter = 21,
        MaxDGAssembly = 22,
        DefaultIPTTL = 23,
        MTUTimeout = 24,
        MTUPlateau = 25,
        MTUInterface = 26,
        MTUSubnet = 27,
        BroadcastAddress = 28,
        MaskDiscovery = 29,
        MaskSupplier = 30,
        RouterDiscover = 30,
        RouterRequest = 32,
        StaticRoute = 33,
        Trailers = 34,
        ARPTimeout = 35,
        Ethernet = 36,
        DefaultTCPTTL = 37,
        KeepaliveTime = 38,
        KeepaliveData = 39,
        NISDomain = 40,
    };

    class DhcpPacket : UdpPacket
    {
        public DhcpOpcodeEnum DhcpOpcode { get; set; }
        public DhcpHardwareTypeEnum DhcpHardType { get; set; }
        public byte DhcpHardAdrLength { get; set; }
        public byte DhcpHopCount { get; set; }
        public UInt32 DhcpTransactionId { get; set; }
        public UInt16 DhcpNumOfSeconds { get; set; }
        public string DhcpFlags { get; set; }
        public string DhcpClientIpAdr { get; set; }
        public string DhcpYourIpAdr { get; set; }
        public string DhcpServerIpAdr { get; set; }
        public string DhcpGatewayIpAdr { get; set; }
        public string DhcpClientHardAdr { get; set; }
        public string DhcpServerName { get; set; }
        public string DhcpBootFilename { get; set; }

        public DhcpPacket(UdpPacket packet) : base (packet)
        {
            UdpLength = packet.UdpLength;
            UdpPorts = packet.UdpPorts;
            UdpChecksum = packet.UdpChecksum;
            IpAppProtocol = AppProtocolEnum.DHCP;
            PacketDisplayedProtocol = "DHCP";

        }
    }    
}
