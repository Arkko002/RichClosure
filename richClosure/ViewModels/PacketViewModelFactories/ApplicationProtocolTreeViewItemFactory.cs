using PacketSniffer.Packets;
using PacketSniffer.Packets.Application_Layer;
using PacketSniffer.Packets.Session_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace richClosure.ViewModels.PacketViewModelFactories
{
    class ApplicationProtocolTreeViewItemFactory : IPacketTreeViewItemFactory
    {
        public TreeViewItem CreatePacketTreeViewItem(IPacket sourcePacket)
        {
            TreeViewItem appProtocolItem = new TreeViewItem { Header = sourcePacket.IpAppProtocol.ToString() };
            
            switch (sourcePacket.IpAppProtocol)
            {
                case AppProtocolEnum.Dns:
                    FillDnsProtocolTreeView(appProtocolItem, sourcePacket);
                    break;

                case AppProtocolEnum.Dhcp:
                    FillDhcpProtocolTreeView(appProtocolItem, sourcePacket);
                    break;

                case AppProtocolEnum.Http:
                    FillHttpProtocolTreeView(appProtocolItem, sourcePacket);
                    break;

                case AppProtocolEnum.Tls:
                    FillTlsProtocolTreeView(appProtocolItem, sourcePacket);
                    break;
            }

            return appProtocolItem;
        }


        private void FillDnsProtocolTreeView(TreeViewItem appProtocolItem, IPacket sourcePacket)
        {
            if (sourcePacket.IpProtocol == IpProtocolEnum.Tcp)
            {
                DnsTcpPacket pac = sourcePacket as DnsTcpPacket;
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Identification: " + pac.DnsIdentification });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "QR: " + pac.DnsQr });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Opcode: " + pac.DnsOpcode });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Rcode: " + pac.DnsRcode });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Flags: " + pac.DnsFlags });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Questions: " + pac.DnsQuestions });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Answers: " + pac.DnsAnswersRr });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Auth.: " + pac.DnsAuthRr });
                appProtocolItem.Items.Add(new TreeViewItem { Header = "Num. of Add.: " + pac.DnsAdditionalRr });

                TreeViewItem questionItem = new TreeViewItem { Header = "Questions: " + pac.DnsQuestions };
                appProtocolItem.Items.Add(questionItem);

                TreeViewItem answersItem = new TreeViewItem { Header = "Answers: " + pac.DnsAnswersRr };
                appProtocolItem.Items.Add(answersItem);

                TreeViewItem authItem = new TreeViewItem { Header = "Auth.: " + pac.DnsAuthRr };
                appProtocolItem.Items.Add(authItem);

                TreeViewItem addItem = new TreeViewItem { Header = "Add.: " + pac.DnsAdditionalRr };
                appProtocolItem.Items.Add(addItem);

                for (int i = 0; i < pac.DnsQuestions; i++)
                {
                    FillDnsQuestionTreeView(questionItem, pac.DnsQuerryList[i], i);
                }

                for (int i = 0; i < pac.DnsAnswersRr; i++)
                {
                    FillDnsRecordTreeView(answersItem, pac.DnsAnswerList[i], i);
                }

                for (int i = 0; i < pac.DnsAuthRr; i++)
                {
                    FillDnsRecordTreeView(authItem, pac.DnsAuthList[i], i);
                }

                for (int i = 0; i < pac.DnsAdditionalRr; i++)
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

        private void FillDhcpProtocolTreeView(TreeViewItem appProtocolItem, IPacket sourcePacket)
        {
            DhcpPacket dhcpPacket = sourcePacket as DhcpPacket;

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

        private void FillHttpProtocolTreeView(TreeViewItem appProtocolItem, IPacket sourcePacket)
        {
            HttpPacket pac = sourcePacket as HttpPacket;

            foreach (KeyValuePair<string, string> entry in pac.HttpFieldsDict)
            {
                appProtocolItem.Items.Add(new TreeViewItem { Header = entry.Key + ": " + entry.Value });
            }
        }

        private void FillTlsProtocolTreeView(TreeViewItem appProtocolItem, IPacket sourcePacket)
        {
            TlsPacket pac = sourcePacket as TlsPacket;
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Content Type: " + pac.TlsType });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Version: " + pac.TlsVersion });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Data Length: " + pac.TlsDataLength });
            appProtocolItem.Items.Add(new TreeViewItem { Header = "Encrypted Data: " + pac.TlsEncryptedData });
        }

    }
}
