using System.Collections.ObjectModel;
using System.Windows;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Data;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.TransportLayer;
using richClosure.ViewModels;
using Autofac;
namespace richClosure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        IContainer Container;
        
        object _packetListLockObject = new object();
        

        //TODO VM for packet sniffer, seprate collection from sniffing
        //TODO vm DI resolving in App OnStartup (graph!!)
        public MainWindow(PacketCollectionViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            Closed += (s, e) => tokenSource.Cancel();

            //TODO MVVM multi-threading
            BindingOperations.EnableCollectionSynchronization(vm.PacketObservableCollection, _packetListLockObject);
        }

        private void AdapterSelection_adapterSelected(object sender, AdapterSelectedEventArgs e)
        {
            NetworkInterface selectedInterface = e.Adapter;
            //packetCollectionViewModel.StartSniffingPackets(selectedInterface);       
        }

        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            //packetCollectionViewModel.PacketObservableCollection.Clear();

            AdapterSelectionWindow adapterSelection = new AdapterSelectionWindow();
            adapterSelection.AdapterSelected += AdapterSelection_adapterSelected;

            if (adapterSelection.ShowDialog() == true)
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true;
            }
        }

        private void SetFilterMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (packetDataGrid.SelectedItem != null)
            {
                var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

                if(dataGridPacket.EthProtocol == 0)
                {
                    EthFilter.IsEnabled = false;
                }
                else
                {
                    EthFilter.IsEnabled = true;
                }

                switch(dataGridPacket.IpProtocol)
                {
                    case IpProtocolEnum.ICMP:
                        UdpFilter.IsEnabled = false;
                        TcpFilter.IsEnabled = false;
                        break;

                    case IpProtocolEnum.TCP:
                        UdpFilter.IsEnabled = false;
                        TcpFilter.IsEnabled = true;
                        break;

                    case IpProtocolEnum.UDP:
                        UdpFilter.IsEnabled = true;
                        TcpFilter.IsEnabled = false;
                        break;
                }
            }
        }

        private void MenuItem_UdpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as UdpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "udpPorts = " + dataGridPacket.UdpPorts["dst"] +
                "&" + "udpPorts = " + dataGridPacket.UdpPorts["src"];

            //packetCollectionViewModel.SearchPacketList(filterString);
            //packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;

        }

        private void MenuItem_TcpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as TcpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["dst"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["src"];

            //packetCollectionViewModel.SearchPacketList(filterString);
            //packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_Ip4FilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"];

            //packetCollectionViewModel.SearchPacketList(filterString);
            //packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_EthernetFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ethDestinationMacAdr = " + dataGridPacket.EthDestinationMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "|" + "ethDestinationMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthDestinationMacAdr;

            //packetCollectionViewModel.SearchPacketList(filterString);
            //packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }
    }
}
