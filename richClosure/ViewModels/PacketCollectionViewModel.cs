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
using System.Collections.Specialized;
using richClosure.Commands;

namespace richClosure.ViewModels
{
    //TODO maybe separate class for organizing vm stuff? 
    public class PacketCollectionViewModel : INotifyPropertyChanged, IViewModel
    {
        public ObservableCollection<PacketViewModel> PacketObservableCollection { get; private set; }
        public ObservableCollection<IPacket> ModelCollection { get; private set; }

        private PacketFilterViewModel PacketFilterViewModel { get; set; }
        private PacketSnifferViewModel PacketSnifferViewModel { get; set; }

        public PacketViewModel SelectedPacket { get; private set; }

        private IWindowManager _windowManager;


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

        public PacketCollectionViewModel(ObservableCollection<IPacket> modelCollection, IWindowManager windowManager)
        {
            PacketObservableCollection = new ObservableCollection<PacketViewModel>();
            ModelCollection = modelCollection;

            PacketFilterViewModel = new PacketFilterViewModel(this);
            PacketFilterViewModel = new PacketFilterViewModel(this);

            _windowManager = windowManager;

            PacketObservableCollection.CollectionChanged += PacketObservableCollection_CollectionChanged;
        }

        private void PacketObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePacketCount(sender as ObservableCollection<PacketViewModel>);
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
