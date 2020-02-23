using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application_Layer;
using PacketSniffer.Packets.Internet_Layer;
using PacketSniffer.Packets.Session_Layer;
using PacketSniffer.Packets.Transport_Layer;
using richClosure.Properties;
using richClosure.ViewModels.PacketViewModelFactories;

namespace richClosure.ViewModels
{
    //TODO Create a proper bit viewer for ascii and hex data
    //TODO Refrence to source packet breaks threading, find a way to copy/reference packet's data on UI
    public class PacketViewModel : INotifyPropertyChanged, IViewModel
    {
        public string AsciiData { get; set; }
        public string HexData { get; set; }

        public TreeViewItem FrameTreeViewItem { get; set; }

        public PacketViewModel(IPacket sourcePacket)
        {
            CreatePacketViewModel(sourcePacket);
        }

        private void CreatePacketViewModel(IPacket sourcePacket)
        {
            GetAsciiPacketData(sourcePacket);
            GetHexPacketData(sourcePacket);
            FillPacketTreeView(sourcePacket);
        }

        private void GetAsciiPacketData(IPacket sourcePacket)
        {
            string[] hexString = sourcePacket.PacketData.Split('-');

            string tempAscii = string.Empty;

            foreach (string hexval in hexString)
            {
                uint decval = Convert.ToUInt32(hexval, 16);

                if (decval >= 33 && decval <= 126)
                {
                    char ch = Convert.ToChar(decval);
                    tempAscii += ch;
                }
                else
                {
                    tempAscii += ".";
                }
            }

            string resString = string.Empty;

            for (int x = 1; x <= hexString.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += tempAscii[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += tempAscii[x - 1] + "   ";
                }
                else
                {
                    resString += tempAscii[x - 1] + " ";
                }
            }

            AsciiData = resString;
        }

        private void GetHexPacketData(IPacket sourcePacket)
        {
            string[] hexTempStr = sourcePacket.PacketData.Split('-');
            string resString = string.Empty;

            for (int x = 1; x <= hexTempStr.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += hexTempStr[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += hexTempStr[x - 1] + "   ";
                }
                else
                {
                    resString += hexTempStr[x - 1] + " ";
                }
            }

            HexData = resString;
        }

        private void FillPacketTreeView(IPacket sourcePacket)
        {
            FillFrameTreeView(sourcePacket);
            FillIpTreeView(sourcePacket);
            FillIpProtocolTreeView(sourcePacket);
            FillAppProtocolTreeView(sourcePacket);

        }

        private void FillFrameTreeView(IPacket sourcePacket)
        {
            FrameTreeViewItemFactory factory = new FrameTreeViewItemFactory();
            FrameTreeViewItem = factory.CreatePacketTreeViewItem(sourcePacket);
        }

        private void FillIpTreeView(IPacket sourcePacket)
        {
            IpTreeViewItemFactory factory = new IpTreeViewItemFactory();
            FrameTreeViewItem.Items.Add(factory.CreatePacketTreeViewItem(sourcePacket));
        }

        private void FillIpProtocolTreeView(IPacket sourcePacket)
        {
            IpProtocolTreeViewItemFactory factory = new IpProtocolTreeViewItemFactory();
            FrameTreeViewItem.Items.Add(factory.CreatePacketTreeViewItem(sourcePacket));
        }

        private void FillAppProtocolTreeView(IPacket sourcePacket)
        {
            ApplicationProtocolTreeViewItemFactory factory = new ApplicationProtocolTreeViewItemFactory();
            FrameTreeViewItem.Items.Add(factory.CreatePacketTreeViewItem(sourcePacket));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
