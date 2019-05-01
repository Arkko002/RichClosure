using richClosure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace richClosure.ViewModels
{
    public class PacketFilterViewModel : IViewModel
    {
        private PacketCollectionViewModel _packetCollectionViewModel;

        private PacketListFilter _packetListFilter;
        public List<IPacket> SearchResultList { get; private set; }

        public ICommand ClearSearchResultCommand { get; private set; }
        public ICommand SearchPacketsCommand { get; private set; }

        public string SearchString { get; set; }

        public PacketFilterViewModel(PacketCollectionViewModel packetCollectionViewModel)
        {
            _packetCollectionViewModel = packetCollectionViewModel;

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
