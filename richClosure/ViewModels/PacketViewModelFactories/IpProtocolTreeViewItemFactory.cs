using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet_Layer;
using PacketSniffer.Packets.Transport_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace richClosure.ViewModels.PacketViewModelFactories
{
    class IpProtocolTreeViewItemFactory : IPacketTreeViewItemFactory
    {
        public TreeViewItem CreatePacketTreeViewItem(IPacket sourcePacket)
        {
            TreeViewItem ipProtocolItem = new TreeViewItem { Header = sourcePacket.IpProtocol.ToString() };
         
            switch (sourcePacket.IpProtocol)
            {
                case IpProtocolEnum.Tcp:
                    FillTcpTreeView(ipProtocolItem, sourcePacket);
                    break;

                case IpProtocolEnum.Icmp:
                    FillIcmpTreeView(ipProtocolItem, sourcePacket);
                    break;

                case IpProtocolEnum.Udp:
                    FillUdpTreeView(ipProtocolItem, sourcePacket);
                    break;
            }

            return ipProtocolItem;
        }

        private void FillTcpTreeView(TreeViewItem ipProtocolItem, IPacket sourcePacket)
        {

            TcpPacket tcpPacket = sourcePacket as TcpPacket;

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
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "FIN - " + tcpPacket.TcpFlags.Fin.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "SYN - " + tcpPacket.TcpFlags.Syn.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "RST - " + tcpPacket.TcpFlags.Rst.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "PSH - " + tcpPacket.TcpFlags.Psh.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "ACK - " + tcpPacket.TcpFlags.Ack.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "URG - " + tcpPacket.TcpFlags.Urg.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "ECE - " + tcpPacket.TcpFlags.Ece.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "CWR - " + tcpPacket.TcpFlags.Cwr.IsSet.ToString() });
            tcpFlagsItem.Items.Add(new TreeViewItem { Header = "NS - " + tcpPacket.TcpFlags.Ns.IsSet.ToString() });

            ipProtocolItem.Items.Add(tcpFlagsItem);
        }


        private void FillIcmpTreeView(TreeViewItem ipProtocolItem, IPacket sourcePacket)
        {
            IcmpPacket icmpPacket = sourcePacket as IcmpPacket;
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

        private void FillUdpTreeView(TreeViewItem ipProtocolItem, IPacket sourcePacket)
        {
            UdpPacket udpPacket = sourcePacket as UdpPacket;

            ipProtocolItem.Header += ", Dest. Port: " + udpPacket.UdpPorts["dst"] +
                                     ", Src. Port: " + udpPacket.UdpPorts["src"];

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Dest. Port: " + udpPacket.UdpPorts["dst"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Src. Port: " + udpPacket.UdpPorts["src"] });
            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Length: " + udpPacket.UdpLength });

            ipProtocolItem.Items.Add(new TreeViewItem { Header = "Checksum: " + udpPacket.UdpChecksum });
        }

    }
}
