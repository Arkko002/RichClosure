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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using richClosure.Annotations;
using richClosure.Views.Commands;
using System.Collections.Specialized;

namespace richClosure.ViewModels
{
    public class PacketCollectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PacketViewModel> PacketObservableCollection { get; set; }
        public ObservableCollection<IPacket> ModelCollection { get; set; }
        public List<IPacket> SearchResultList { get; set; }

        private PacketListFilter _packetListFilter;
        private PacketSniffer _packetSniffer;

        private StartSniffingCommand _startSniffingCommand;
        private StopSniffingCommand _stopSniffingCommand;
        private ClearSearchResultCommand _clearSearchResultCommand;
        private SearchPacketsCommand _searchPacketsCommand;

        public PacketViewModel SelectedPacket { get; set; }


        private int _totalPacketCount;
        public int TotalPacketCount
        {
            get => _totalPacketCount;
            set
            {
                _totalPacketCount = value;
                OnPropertyChanged(nameof(TotalPacketCount));
            }
        }

        private int _shownPacketCount;
        public int ShownPacketCount
        {
            get => _shownPacketCount;
            set
            {
                _shownPacketCount = value;
                OnPropertyChanged(nameof(ShownPacketCount));
            }
        }

        public PacketCollectionViewModel()
        {
            PacketObservableCollection = new ObservableCollection<PacketViewModel>();
            _packetListFilter = new PacketListFilter(ModelCollection.ToList());
            SearchResultList = new List<IPacket>();

            _startSniffingCommand = new StartSniffingCommand(this, _packetSniffer);
            _stopSniffingCommand = new StopSniffingCommand(_packetSniffer);
            _clearSearchResultCommand = new ClearSearchResultCommand(this);
            _searchPacketsCommand = new SearchPacketsCommand(this);

            PacketObservableCollection.CollectionChanged += PacketObservableCollection_CollectionChanged; ;

        }

        private void PacketObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePacketCount(sender as ObservableCollection<PacketViewModel>);
        }

        public void SearchPacketList(string searchString)
        {
            SearchResultList.Clear();
            _packetListFilter.SearchList(searchString);
            ShownPacketCount = SearchResultList.Count;
        }

        public ICommand StartSniffingPackets => _startSniffingCommand;
        public ICommand StopSniffingPackets => _stopSniffingCommand;
        public ICommand ClearSearchResult => _clearSearchResultCommand;
        public ICommand SearchPackets => _searchPacketsCommand;


        public void ClearPacketList()
        {
            PacketObservableCollection.Clear();
        }

        public void ClearSearchResultList()
        {
            SearchResultList.Clear();
            ShownPacketCount = PacketObservableCollection.Count;
        }

        public void ChangeSelectedPacket(ulong packetId)
        {
            //SelectedPacket = new PacketViewModel(ModelCollection[(int)packetId - 1]);
        }

        private void UpdatePacketCount(ObservableCollection<PacketViewModel> packetList)
        {
            TotalPacketCount = packetList.Count();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
