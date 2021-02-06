using System.Net.NetworkInformation;
using System.Reactive;
using ReactiveUI;
using richClosure.Avalonia.Views;

namespace richClosure.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase 
    {
        // TODO Refactor, maybe seprate class?
        public MainWindowViewModel(
            PacketCollectionViewModel packetCollectionViewModel,
            PacketFilterViewModel packetFilterViewModel,
            PacketSnifferViewModel packetSnifferViewModel)
        {
            PacketCollectionViewModel = packetCollectionViewModel;
            PacketFilterViewModel = packetFilterViewModel;
            PacketSnifferViewModel = packetSnifferViewModel;

            StartSniffingCommand = ReactiveCommand.Create(() => StartSniffing());
        }

        
        public static NetworkInterface SniffedInterface { get; set; }
        public PacketCollectionViewModel PacketCollectionViewModel { get; set; }
        public PacketFilterViewModel PacketFilterViewModel { get; set; }
        public PacketSnifferViewModel PacketSnifferViewModel { get; set; }

        public ReactiveCommand<Unit, Unit> StartSniffingCommand { get; }


        public void ChoseNetworkInterface()
        {
            
            //TODO Show network selection window / dialog
        }
        
        public void StartSniffing()
        {
            //TODO Network selection starts sniffing, recouple sniffing from network selection
            if (SniffedInterface == null)
            {
                ChoseNetworkInterface();
            }
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
