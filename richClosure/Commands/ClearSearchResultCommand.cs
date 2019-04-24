using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using richClosure.ViewModels;

namespace richClosure.Views.Commands
{
    class ClearSearchResultCommand : ICommand
    {
        private PacketCollectionViewModel _collectionViewModel;

        public ClearSearchResultCommand(PacketCollectionViewModel collectionViewModel)
        {
            _collectionViewModel = collectionViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _collectionViewModel.ClearSearchResultList();
        }

        public event EventHandler CanExecuteChanged;
    }
}
