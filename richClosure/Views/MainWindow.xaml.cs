using System.Threading;
using System.Windows;
using System.Windows.Data;
using richClosure.Packets;
using richClosure.Packets.Internet_Layer;
using richClosure.Packets.Transport_Layer;
using richClosure.ViewModels;

namespace richClosure.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        readonly object _packetListLockObject = new object();
        
        public MainWindow(MainWindowViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            Closed += (s, e) => _tokenSource.Cancel();

            // TODO MVVM multi-threading
            BindingOperations.EnableCollectionSynchronization(vm.PacketCollectionViewModel.PacketObservableCollection, _packetListLockObject);
        }

        private void SetFilterMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (PacketDataGrid.SelectedItem != null)
            {
                var dataGridPacket = PacketDataGrid.SelectedItem as IpPacket;

                switch(dataGridPacket.IpProtocol)
                {
                    case IpProtocolEnum.Icmp:
                        UdpFilter.IsEnabled = false;
                        TcpFilter.IsEnabled = false;
                        break;

                    case IpProtocolEnum.Tcp:
                        UdpFilter.IsEnabled = false;
                        TcpFilter.IsEnabled = true;
                        break;

                    case IpProtocolEnum.Udp:
                        UdpFilter.IsEnabled = true;
                        TcpFilter.IsEnabled = false;
                        break;
                }
            }
        }

        //TODO MVVM this
        private void MenuItem_UdpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = PacketDataGrid.SelectedItem as UdpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "udpPorts = " + dataGridPacket.UdpPorts["dst"] +
                "&" + "udpPorts = " + dataGridPacket.UdpPorts["src"];

            // packetCollectionViewModel.SearchPacketList(filterString);
            // packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            SearchTextBox.Text = filterString;

        }

        private void MenuItem_TcpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = PacketDataGrid.SelectedItem as TcpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["dst"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["src"];

            // packetCollectionViewModel.SearchPacketList(filterString);
            // packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            SearchTextBox.Text = filterString;
        }

        private void MenuItem_Ip4FilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = PacketDataGrid.SelectedItem as IpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"];

            // packetCollectionViewModel.SearchPacketList(filterString);
            // packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            SearchTextBox.Text = filterString;
        }
    }
}
