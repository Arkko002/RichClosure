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
using richClosure.Commands;

namespace richClosure.ViewModels
{
    public class PacketCollectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PacketViewModel> PacketObservableCollection { get; private set; }
        public ObservableCollection<IPacket> ModelCollection { get; private set; }


        private PacketFilterViewModel PacketFilterViewModel { get; set; }

        private PacketSniffer _packetSniffer;

        public ICommand StartSniffingCommand { get; private set; }
        public ICommand StopSniffingCommand { get; private set; }


        public PacketViewModel SelectedPacket { get; private set; }


        private int _totalPacketCount;
        public int TotalPacketCount
        {
            get => _totalPacketCount;
            private set
            {
                _totalPacketCount = value;
                OnPropertyChanged(nameof(TotalPacketCount));
            }
        }

        private int _shownPacketCount;
        public int ShownPacketCount
        {
            get => _shownPacketCount;
            private set
            {
                _shownPacketCount = value;
                OnPropertyChanged(nameof(ShownPacketCount));
            }
        }

        public PacketCollectionViewModel(ObservableCollection<IPacket> modelCollection)
        {
            PacketObservableCollection = new ObservableCollection<PacketViewModel>();
            ModelCollection = modelCollection;

            PacketFilterViewModel = new PacketFilterViewModel(this);

            StartSniffingCommand = new RelayCommand(x => StartSniffingPackets(), x => !_packetSniffer.IsWorking);
            StopSniffingCommand = new RelayCommand(x => StopSniffingPackets(), x => _packetSniffer.IsWorking);

            PacketObservableCollection.CollectionChanged += PacketObservableCollection_CollectionChanged;
        }

        private void PacketObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePacketCount(sender as ObservableCollection<PacketViewModel>);
        }



        private void StartSniffingPackets()
        {
            if(_packetSniffer is null)
            {
                //TODO
            }

            _packetSniffer.SniffPackets();
        }

        private void StopSniffingPackets()
        {
            _packetSniffer.StopWorking();
        }

        public void ClearPacketList()
        {
            PacketObservableCollection.Clear();
        }


        public void ChangeSelectedPacket(ulong packetId)
        {
            //SelectedPacket = new PacketViewModel(ModelCollection[(int)packetId - 1]);
        }

        private void UpdatePacketCount(ObservableCollection<PacketViewModel> packetCollection)
        {
            TotalPacketCount = packetCollection.Count();
        }

        public void UpdateShownPacketCount(ObservableCollection<PacketViewModel> packetCollection)
        {
            ShownPacketCount = packetCollection.Count;
        }

        public void UpdateShownPacketCount(List<IPacket> packetList)
        {
            ShownPacketCount = packetList.Count;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
