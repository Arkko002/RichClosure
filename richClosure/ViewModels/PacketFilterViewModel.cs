using richClosure.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PacketSniffer.Packets;
using richClosure.Packet_Filtering;

namespace richClosure.ViewModels
{
    public class PacketFilterViewModel : IViewModel
    {
        private readonly PacketCollectionViewModel _packetCollectionViewModel;

        private readonly PacketListFilter _packetListFilter;
        public List<IPacket> SearchResultList { get; }

        public ICommand ClearSearchResultCommand { get; }
        public ICommand SearchPacketsCommand { get; }

        public string SearchString { get; set; }

        //TODO Replace ViewModel dependency with the properties needed in this VM
        public PacketFilterViewModel(PacketCollectionViewModel packetCollectionViewModel)
        {
            _packetCollectionViewModel = packetCollectionViewModel;

            _packetListFilter = new PacketListFilter(packetCollectionViewModel.ModelCollection.ToList());
            SearchResultList = new List<IPacket>();

            ClearSearchResultCommand = new RelayCommand(x => ClearSearchResultList(), x => true);
            SearchPacketsCommand = new RelayCommand(x => SearchPacketList(SearchString), x => true);
        }

        public void SearchPacketList(string searchString)
        {
            SearchResultList.Clear();
            _packetListFilter.SearchList(searchString);
            _packetCollectionViewModel.UpdateShownPacketCount(SearchResultList);
        }

        public void ClearSearchResultList()
        {
            SearchResultList.Clear();
            _packetCollectionViewModel.UpdateShownPacketCount(_packetCollectionViewModel.PacketObservableCollection);
        }
    }
}
