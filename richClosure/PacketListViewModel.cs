using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Controls;

namespace richClosure
{
    class PacketListViewModel
    {
        public ObservableCollection<IPacket> PacketList { get; set; }
        public List<IPacket> SearchResultList { get; set; }
        private PacketSniffer packetSniffer;
        private PacketListFilter packetListFilter;
        public PacketViewModel SelectedPacket { get; set; }


        public PacketListViewModel()
        {
            PacketList = new ObservableCollection<IPacket>();
            packetListFilter = new PacketListFilter();
            SearchResultList = new List<IPacket>();
        }

        public void StartSniffingPackets(NetworkInterface networkInterface)
        {
            packetSniffer = new PacketSniffer(networkInterface);

            Thread packetSnifferThread = new Thread(() => packetSniffer.SniffPackets(PacketList));
            packetSnifferThread.IsBackground = true;
            packetSnifferThread.Start();


            Thread factoryThread = new Thread(() => packetSniffer.CreatePacketsFromQueue(PacketList));
            factoryThread.IsBackground = true;
            factoryThread.Start();
        }

        public void SearchPacketList(string searchString)
        {
            SearchResultList.Clear();
            packetListFilter.SearchList(PacketList.ToList(), SearchResultList, searchString);
        }

        public void StopSniffingPackets()
        {
            packetSniffer.StopWorking();
        }

        public int GetSearchListCount()
        {
            return SearchResultList.Count;
        }

        public void ClearPacketList()
        {
            PacketList.Clear();
        }

        public void ChangeSelectedPacket(ulong packetId)
        {
            SelectedPacket = new PacketViewModel(PacketList[(int)packetId - 1]);
        }
    }
}
