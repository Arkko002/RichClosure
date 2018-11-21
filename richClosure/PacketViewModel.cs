using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace richClosure
{
    class PacketViewModel
    {
        public string HexData { get; }
        public string AsciiData { get; }
        private IPacket SourcePacket { get; }
        public List<TreeViewItem> TreeViewItems { get; set; }


        public PacketViewModel(IPacket packet)
        {
            TreeViewItems = new List<TreeViewItem>();
            SourcePacket = packet;
            AsciiData = GetAsciiPacketData(packet);
            HexData = GetHexPacketData(packet);
            FillPacketTreeView(packet, TreeViewItems);
        }

        public string GetAsciiPacketData(IPacket packet)
        {
            string[] hexString = packet.PacketData.Split('-');

            string asciiData = String.Empty;

            foreach (string hexval in hexString)
            {
                uint decval = Convert.ToUInt32(hexval, 16);

                if (decval >= 33 && decval <= 126)
                {
                    char ch = Convert.ToChar(decval);
                    asciiData += ch;
                }
                else
                {
                    asciiData += ".";
                }
            }

            string resString = String.Empty;

            for (int x = 1; x <= hexString.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += asciiData[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += asciiData[x - 1] + "   ";
                }
                else
                {
                    resString += asciiData[x - 1] + " ";
                }
            }

            return resString;
        }

        public string GetHexPacketData(IPacket packet)
        {
            string[] hexString = packet.PacketData.Split('-');
            string resString = String.Empty;

            for (int x = 1; x <= hexString.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += hexString[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += hexString[x - 1] + "   ";
                }
                else
                {
                    resString += hexString[x - 1] + " ";
                }
            }

            return resString;
        }

        private void FillPacketTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            IpPacket pac = packet as IpPacket;


            FillFrameTreeView(pac, TreeViewItems);

            //Ethernet frame is unincluded if ethProtocol == 0
            if (pac.EthProtocol != 0)
            {
                FillEthernetTreeView(pac, TreeViewItems);
            }

            FillIpTreeView(pac, TreeViewItems);
            FillIpProtocolTreeView(pac, TreeViewItems);

            if (packet.IpAppProtocol != AppProtocolEnum.NoAppProtocol)
            {
                FillAppProtocolTreeView(pac, TreeViewItems);
            }
        }

        private void FillFrameTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            TreeViewItem frameItem = new TreeViewItem { Header = "Frame " + packet.PacketId + ", Time Captured: " + packet.TimeDateCaptured };
            TreeViewItems.Add(frameItem);
        }

        private void FillEthernetTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            IpPacket pac = packet as IpPacket;

            TreeViewItem ethItem = new TreeViewItem { Header = "Ethernet Layer, " + "Dest: " + pac.EthDestinationMacAdr + ", Src: " + pac.EthSourceMacAdr };

            ethItem.Items.Add(new TreeViewItem { Header = "Ethernet Dest. MAC: " + pac.EthDestinationMacAdr });
            ethItem.Items.Add(new TreeViewItem { Header = "Ethernet Src. MAC: " + pac.EthSourceMacAdr });
            ethItem.Items.Add(new TreeViewItem { Header = "Ethernet Protocol: " + pac.EthProtocol });

            TreeViewItems.Add(ethItem);
        }

        private void FillIpTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            switch (packet.IpVersion)
            {
                case 4:
                    IpPacket ip4Packet = packet as IpPacket;

                    TreeViewItem ipItem = new TreeViewItem { Header = "IP4 Layer, " + "Dest: " + ip4Packet.Ip4Adrs["dst"] + ", Src: " + ip4Packet.Ip4Adrs["src"] };

                    ipItem.Items.Add(new TreeViewItem { Header = "Version: " + ip4Packet.IpVersion });
                    ipItem.Items.Add(new TreeViewItem { Header = "Header Length: " + ip4Packet.Ip4HeaderLength });
                    ipItem.Items.Add(new TreeViewItem { Header = "Protocol: " + ip4Packet.IpProtocol });
                    ipItem.Items.Add(new TreeViewItem { Header = "Dest. Address: " + ip4Packet.Ip4Adrs["dst"] });
                    ipItem.Items.Add(new TreeViewItem { Header = "Src. Address: " + ip4Packet.Ip4Adrs["src"] });
                    ipItem.Items.Add(new TreeViewItem { Header = "DSCP: " + ip4Packet.Ip4Dscp });
                    ipItem.Items.Add(new TreeViewItem { Header = "Total Length: " + ip4Packet.IpTotalLength });
                    ipItem.Items.Add(new TreeViewItem { Header = "Identification: " + ip4Packet.Ip4Identification });
                    ipItem.Items.Add(new TreeViewItem { Header = "Offset: " + ip4Packet.Ip4Offset });

                    switch (ip4Packet.Ip4Flags)
                    {
                        case IpFlagsEnum.DontFragment:
                            ipItem.Items.Add(new TreeViewItem { Header = "Flags: Don't Fragment" });
                            break;

                        case IpFlagsEnum.MoreFragments:
                            ipItem.Items.Add(new TreeViewItem { Header = "Flags: More Fragments Coming" });
                            break;

                        case IpFlagsEnum.NoFlags:
                            ipItem.Items.Add(new TreeViewItem { Header = "Flags: No Flags Set" });
                            break;
                    }

                    ipItem.Items.Add(new TreeViewItem { Header = "TTL: " + ip4Packet.Ip4TimeToLive });
                    ipItem.Items.Add(new TreeViewItem { Header = "Header Checksum: " + ip4Packet.Ip4HeaderChecksum });
                    TreeViewItems.Add(ipItem);
                    break;

                case 6:
                    IpPacket ip6Packet = packet as IpPacket;
                    TreeViewItem ip6item = new TreeViewItem { Header = "IPv6 Layer, " + "Dest: " + ip6Packet.Ip6Adrs["dst"] + ", Src: " + ip6Packet.Ip6Adrs["src"] };


                    ip6item.Items.Add(new TreeViewItem { Header = "Version: " + ip6Packet.IpVersion });
                    ip6item.Items.Add(new TreeViewItem { Header = "Traffic Class: " + ip6Packet.Ip6TrafficClass });
                    ip6item.Items.Add(new TreeViewItem { Header = "Flow Label: " + ip6Packet.Ip6FlowLabel });
                    ip6item.Items.Add(new TreeViewItem { Header = "Payload Length: " + ip6Packet.IpTotalLength });
                    ip6item.Items.Add(new TreeViewItem { Header = "Next Header: " + ip6Packet.IpProtocol });
                    ip6item.Items.Add(new TreeViewItem { Header = "Hop Limit: " + ip6Packet.Ip6HopLimit });
                    ip6item.Items.Add(new TreeViewItem { Header = "Src. Adr.: " + ip6Packet.Ip6Adrs["dst"] });
                    ip6item.Items.Add(new TreeViewItem { Header = "Dest. Adr.: " + ip6Packet.Ip6Adrs["src"] });
                    TreeViewItems.Add(ip6item);
                    break;
            }



        }

        private void FillIpProtocolTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            TreeViewItem ipProtocolItem = new TreeViewItem { Header = packet.IpProtocol.ToString() };
            TreeViewItems.Add(ipProtocolItem);


            switch (packet.IpProtocol)
            {
                case IpProtocolEnum.TCP:
                    FillTcpTreeView(ipProtocolItem, packet);
                    break;

                case IpProtocolEnum.ICMP:
                    FillIcmpTreeView(ipProtocolItem, packet);
                    break;

                case IpProtocolEnum.UDP:
                    FillUdpTreeView(ipProtocolItem, packet);
                    break;
            }
        }

        private void FillTcpTreeView(TreeViewItem ipProtocolItem, IPacket packet)
        {

            TcpPacket tcpPacket = packet as TcpPacket;

            ipProtocolItem.Header += ", Dest. Port: " + tcpPacket.TcpPorts["dst"] +
                                     ", Src. Port: " + tcpPacket.TcpPorts["src"];

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Dest. port: " + tcpPacket.TcpPorts["dst"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Src. port: " + tcpPacket.TcpPorts["src"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Seq. Number: " + tcpPacket.TcpSequenceNumber });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Ack Number: " + tcpPacket.TcpAckNumber });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Urg. Pointer: " + tcpPacket.TcpUrgentPointer });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Data Offset: " + tcpPacket.TcpDataOffset });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Window Size: " + tcpPacket.TcpWindowSize });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Checksum: " + tcpPacket.TcpChecksum });

            TreeViewItem tcpFlagsItem = new TreeViewItem { Header = "TCP Flags" };
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "FIN - " + tcpPacket.TcpFlags.FIN.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "SYN - " + tcpPacket.TcpFlags.SYN.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "RST - " + tcpPacket.TcpFlags.RST.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "PSH - " + tcpPacket.TcpFlags.PSH.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "ACK - " + tcpPacket.TcpFlags.ACK.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "URG - " + tcpPacket.TcpFlags.URG.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "ECE - " + tcpPacket.TcpFlags.ECE.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "CWR - " + tcpPacket.TcpFlags.CWR.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "NS - " + tcpPacket.TcpFlags.NS.IsSet.ToString() });

            ipProtocolItem.Items.Add(tcpFlagsItem);
        }

        private void FillIcmpTreeView(TreeViewItem ipProtocolItem, IPacket packet)
        {
            IcmpPacket icmpPacket = packet as IcmpPacket;
            ipProtocolItem.Header += ", Type: " + icmpPacket.IcmpType + ", Code: " + icmpPacket.IcmpCode;

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "ICMP Type: " + icmpPacket.IcmpType + ", " + (IcmpTypeEnum)icmpPacket.IcmpType });

            switch (icmpPacket.IcmpType)
            {
                case 3:
                    ipProtocolItem.Items.Add(new TreeViewItem
                    {
                        Header = "ICMP Code: " + icmpPacket.IcmpCode + ", " +
                        (DestinationUnreachableCodeEnum)icmpPacket.IcmpCode
                    });
                    break;

                case 5:
                    ipProtocolItem.Items.Add(new TreeViewItem
                    {
                        Header = "ICMP Code: " + icmpPacket.IcmpCode + ", " +
                        (RedirectMessageCodeEnum)icmpPacket.IcmpCode
                    });
                    break;

                case 11:
                    ipProtocolItem.Items.Add(new TreeViewItem
                    {
                        Header = "ICMP Code: " + icmpPacket.IcmpCode + ", " +
                        (TimeExceededCodeEnum)icmpPacket.IcmpCode
                    });
                    break;

                case 12:
                    ipProtocolItem.Items.Add(new TreeViewItem
                    {
                        Header = "ICMP Code: " + icmpPacket.IcmpCode + ", " +
                        (ParameterProblemCodeEnum)icmpPacket.IcmpCode
                    });
                    break;

                default:
                    ipProtocolItem.Items.Add(new TreeViewItem
                    {
                        Header = "ICMP Code: " + icmpPacket.IcmpCode + ", " +
                        (IcmpTypeEnum)icmpPacket.IcmpType
                    });
                    break;
            }

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "ICMP Checksum: " + icmpPacket.IcmpChecksum });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "ICMP Rest: " + icmpPacket.IcmpRest });
        }

        private void FillUdpTreeView(TreeViewItem ipProtocolItem, IPacket packet)
        {
            UdpPacket udpPacket = packet as UdpPacket;

            ipProtocolItem.Header += ", Dest. Port: " + udpPacket.UdpPorts["dst"] +
                                     ", Src. Port: " + udpPacket.UdpPorts["src"];

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Dest. Port: " + udpPacket.UdpPorts["dst"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Src. Port: " + udpPacket.UdpPorts["src"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Length: " + udpPacket.UdpLength });

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Checksum: " + udpPacket.UdpChecksum });
        }

        private void FillAppProtocolTreeView(IPacket packet, List<TreeViewItem> TreeViewItems)
        {
            TreeViewItem appProtocolItem = new TreeViewItem { Header = packet.IpAppProtocol.ToString() };
            TreeViewItems.Add(appProtocolItem);

            switch (packet.IpAppProtocol)
            {
                case AppProtocolEnum.DNS:
                    FillDnsProtocolTreeView(appProtocolItem, packet);
                    break;

                case AppProtocolEnum.DHCP:
                    FillDhcpProtocolTreeView(appProtocolItem, packet);
                    break;

                case AppProtocolEnum.HTTP:
                    FillHttpProtocolTreeView(appProtocolItem, packet);
                    break;

                case AppProtocolEnum.TLS:
                    FillTlsProtocolTreeView(appProtocolItem, packet);
                    break;
            }
        }

        private void FillDnsProtocolTreeView(TreeViewItem appProtocolItem, IPacket packet)
        {
            if (packet.IpProtocol == IpProtocolEnum.TCP)
            {
                DnsTcpPacket pac = packet as DnsTcpPacket;
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Identification: " + pac.DnsIdentification });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "QR: " + pac.DnsQR });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Opcode: " + pac.DnsOpcode });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Rcode: " + pac.DnsRcode });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Flags: " + pac.DnsFlags });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Questions: " + pac.DnsQuestions });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Answers: " + pac.DnsAnswersRR });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Auth.: " + pac.DnsAuthRR });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Add.: " + pac.DnsAdditionalRR });

                TreeViewItem questionItem = new TreeViewItem { Header = "Questions: " + pac.DnsQuestions };
                appProtocolItem.Items.Add(questionItem);

                TreeViewItem answersItem = new TreeViewItem { Header = "Answers: " + pac.DnsAnswersRR };
                appProtocolItem.Items.Add(answersItem);

                TreeViewItem authItem = new TreeViewItem { Header = "Auth.: " + pac.DnsAuthRR };
                appProtocolItem.Items.Add(authItem);

                TreeViewItem addItem = new TreeViewItem { Header = "Add.: " + pac.DnsAdditionalRR };
                appProtocolItem.Items.Add(addItem);

                for (int i = 0; i < pac.DnsQuestions; i++)
                {
                    FillDnsQuestionTreeView(questionItem, pac.DnsQuerryList[i], i);
                }

                for (int i = 0; i < pac.DnsAnswersRR; i++)
                {
                    FillDnsRecordTreeView(answersItem, pac.DnsAnswerList[i], i);
                }

                for (int i = 0; i < pac.DnsAuthRR; i++)
                {
                    FillDnsRecordTreeView(authItem, pac.DnsAuthList[i], i);
                }

                for (int i = 0; i < pac.DnsAdditionalRR; i++)
                {
                    FillDnsRecordTreeView(addItem, pac.DnsAdditionalList[i], i);
                }

            }
        }

        private void FillDnsRecordTreeView(TreeViewItem recordItem, DnsRecord record, int num)
        {
            TreeViewItem recItem = new TreeViewItem { Header = "Record " + num.ToString() };
            recItem.Items.Add(new TreeViewItem { Header = "Name: " + record.DnsRecordName });
            recItem.Items.Add(new TreeViewItem { Header = "Type: " + record.DnsRecordType });
            recItem.Items.Add(new TreeViewItem { Header = "Class: " + record.DnsRecordClass });
            recItem.Items.Add(new TreeViewItem { Header = "TTL: " + record.DnsTimeToLive });
            recItem.Items.Add(new TreeViewItem { Header = "Data Length: " + record.DnsRdataLength });

            TreeViewItem dataItem = new TreeViewItem { Header = "Data" };
            dataItem.Items.Add(new TreeViewItem { Header = record.DnsRdata });
            recItem.Items.Add(dataItem);

            recordItem.Items.Add(recItem);
        }
        private void FillDnsQuestionTreeView(TreeViewItem questionItem, DnsQuery query, int num)
        {
            TreeViewItem qItem = new TreeViewItem { Header = "Question " + num.ToString() };
            qItem.Items.Add(new TreeViewItem { Header = "Name: " + query.DnsQueryName });
            qItem.Items.Add(new TreeViewItem { Header = "Type: " + query.DnsQueryType });
            qItem.Items.Add(new TreeViewItem { Header = "Class: " + query.DnsQueryClass });

            questionItem.Items.Add(qItem);

        }

        private void FillDhcpProtocolTreeView(TreeViewItem appProtocolItem, IPacket packet)
        {
            DhcpPacket dhcpPacket = packet as DhcpPacket;

            appProtocolItem.Items.Add(new TreeViewItem { Header = "Opcode: " + dhcpPacket.DhcpOpcode });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Hardware Type: " + dhcpPacket.DhcpHardType });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Hardware Adr. Length: " + dhcpPacket.DhcpHardAdrLength });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Hops: " + dhcpPacket.DhcpHopCount });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Transaction ID: " + dhcpPacket.DhcpTransactionId });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Seconds: " + dhcpPacket.DhcpNumOfSeconds });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Flags: " + dhcpPacket.DhcpFlags });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Client IP: " + dhcpPacket.DhcpClientIpAdr });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Your IP: " + dhcpPacket.DhcpYourIpAdr });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Gateway IP: " + dhcpPacket.DhcpGatewayIpAdr });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Client Hardware Adr.: " + dhcpPacket.DhcpClientHardAdr });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Server Name: " + dhcpPacket.DhcpServerName });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Boot Filename: " + dhcpPacket.DhcpBootFilename });
        }

        private void FillHttpProtocolTreeView(TreeViewItem appProtocolItem, IPacket packet)
        {
            HttpPacket pac = packet as HttpPacket;

            foreach (KeyValuePair<string, string> entry in pac.HttpFieldsDict)
            {
                appProtocolItem.Items.Add(new TreeViewItem { Header = entry.Key + ": " + entry.Value });
            }
        }

        private void FillTlsProtocolTreeView(TreeViewItem appProtocolItem, IPacket packet)
        {
            TlsPacket pac = packet as TlsPacket;
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Content Type: " + pac.TlsType.ToString() });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Version: " + pac.TlsVersion.ToString() });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Data Length: " + pac.TlsDataLength.ToString() });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Encrypted Data: " + pac.TlsEncryptedData });
        }
    }
}
