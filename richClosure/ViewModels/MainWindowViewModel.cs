using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using richClosure.Commands;

namespace richClosure.ViewModels
{
    public class MainWindowViewModel : IViewModel
    {
        public PacketCollectionViewModel PacketCollectionViewModel { get; set; }
        public PacketFilterViewModel PacketFilterViewModel { get; set; }
        public InterfaceSelectionViewModel InterfaceSelectionViewModel { get; set; }
        public PacketSnifferViewModel PacketSnifferViewModel { get; set; }

        private IWindowManager _windowManager;

        public ICommand ShowAdapterSelectionCommand { get; private set; }

        public MainWindowViewModel(PacketCollectionViewModel packetCollectionViewModel, IWindowManager windowManager)
        {
            //TODO Better DI for this
            PacketCollectionViewModel = packetCollectionViewModel;
            PacketFilterViewModel = new PacketFilterViewModel(PacketCollectionViewModel);
            PacketSnifferViewModel = new PacketSnifferViewModel(PacketCollectionViewModel.ModelCollection);
            InterfaceSelectionViewModel = new InterfaceSelectionViewModel(PacketSnifferViewModel);

            _windowManager = windowManager;

            ShowAdapterSelectionCommand = new RelayCommand(x => StartSniffing(), x => true);
        }

        public void StartSniffing()
        {
            _windowManager.ShowWindow(typeof(InterfaceSelectionWindow), InterfaceSelectionViewModel);
        }

        public void StopSniffing()
        {
            if (PacketSnifferViewModel.StopSniffingCommand.CanExecute(null))
            {
                PacketSnifferViewModel.StopSniffingCommand.Execute(null);
            }
        }

    }
}
