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
using System.Windows.Input;
using richClosure.Views.Commands;

namespace richClosure.ViewModels
{
    public class PacketCollectionViewModel
    {
        public ObservableCollection<PacketViewModel> PacketObservableCollection { get; set; }
        public ObservableCollection<IPacket> ModelCollection { get; set; }
        public List<IPacket> SearchResultList { get; set; }

        public PacketViewModel SelectedPacket { get; set; }

        private PacketListFilter _packetListFilter;
        private PacketSniffer _packetSniffer;

        private StartSniffingCommand _startSniffingCommand;
        private StopSniffingCommand _stopSniffingCommand;

        public PacketCollectionViewModel()
        {
            PacketObservableCollection = new ObservableCollection<PacketViewModel>();
            _packetListFilter = new PacketListFilter(ModelCollection.ToList());
            SearchResultList = new List<IPacket>();

            _startSniffingCommand = new StartSniffingCommand(this, _packetSniffer);
            _stopSniffingCommand = new StopSniffingCommand(_packetSniffer);
        }

        public void SearchPacketList(string searchString)
        {
            SearchResultList.Clear();
            _packetListFilter.SearchList(searchString);
        }

        public ICommand StartSniffingPackets => _startSniffingCommand;
        public ICommand StopSniffingPackets => _stopSniffingCommand;

        public int GetSearchListCount()
        {
            return SearchResultList.Count;
        }

        public void ClearPacketList()
        {
            PacketObservableCollection.Clear();
        }

        public void ChangeSelectedPacket(ulong packetId)
        {
            //SelectedPacket = new PacketViewModel(ModelCollection[(int)packetId - 1]);
        }
    }
}
