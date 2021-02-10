namespace GUI.WPF.ViewModels
{
    public class PacketCollectionViewModel : INotifyPropertyChanged, IViewModel
    {
        public ObservableCollection<PacketViewModel> PacketObservableCollection { get; }
        public ObservableCollection<IPacket> ModelCollection { get; }

        private PacketViewModel _selectedPacket;
        public PacketViewModel SelectedPacket
        {
            get => _selectedPacket;
            set
            {
                _selectedPacket = value;
                OnPropertyChanged(nameof(SelectedPacket));
            }
        }

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

            modelCollection.CollectionChanged += ModelCollection_CollectionChanged;
            PacketObservableCollection.CollectionChanged += PacketObservableCollection_CollectionChanged;
        }

        //TODO
        private void ModelCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddPacketViewModelsToCollection(e.NewItems);
                    break;

                default:
                    break;
            }
        }

        private void PacketObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePacketCount(sender as ObservableCollection<PacketViewModel>);
        }

        private void AddPacketViewModelsToCollection(IList newPackets)
        {
            foreach (var packet in newPackets)
            {
                PacketObservableCollection.Add(new PacketViewModel((IPacket)packet));
            }
        }

        public void ClearPacketList()
        {
            PacketObservableCollection.Clear();
        }

        public void ChangeSelectedPacket(ulong packetId)
        {
            // SelectedPacket = new PacketViewModel(ModelCollection[(int)packetId - 1]);
        }

        private void UpdatePacketCount(ObservableCollection<PacketViewModel> packetCollection)
        {
            TotalPacketCount = packetCollection.Count;
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
