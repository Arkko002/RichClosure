using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Data;
using Microsoft.Win32;
using System.Collections.Generic;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.TransportLayer;
using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.SessionLayer;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        PacketListViewModel packetListViewModel = new PacketListViewModel();
        
        object _packetListLockObject = new object();
        

        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => tokenSource.Cancel();

            BindingOperations.EnableCollectionSynchronization(packetListViewModel.PacketList, _packetListLockObject);

            packetDataGrid.ItemsSource = packetListViewModel.PacketList;

            packetListViewModel.PacketList.CollectionChanged += PacketList_totalPacketsCountUpdate;

            stopButton.IsEnabled = false;
            saveButton.IsEnabled = false;
        }

        private void PacketList_totalPacketsCountUpdate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!tokenSource.IsCancellationRequested)
            {
                Dispatcher.Invoke(() =>
                {
                    totalPacketsLabel.Content = "Total packets: " + packetListViewModel.PacketList.Count;
                });
            }
        }

        private void AdapterSelection_adapterSelected(object sender, AdapterSelectedEventArgs e)
        {
            NetworkInterface selectedInterface = e.Adapter;
            packetListViewModel.StartSniffingPackets(selectedInterface);       
        }


        private void PacketDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            packetHexTextBox.Clear();
            packetAsciiTextBox.Clear();

            if (packetDataGrid.SelectedItem != null)
            {
                var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

                packetListViewModel.ChangeSelectedPacket(dataGridPacket.PacketId);
                packetTreeView.ItemsSource = packetListViewModel.SelectedPacket.TreeViewItems;

                packetAsciiTextBox.Text = packetListViewModel.SelectedPacket.AsciiData;
                packetHexTextBox.Text = packetListViewModel.SelectedPacket.HexData;
            }
        }

        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            packetListViewModel.PacketList.Clear();

            AdapterSelectionWindow adapterSelection = new AdapterSelectionWindow();
            adapterSelection.AdapterSelected += AdapterSelection_adapterSelected;

            if (adapterSelection.ShowDialog() == true)
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true;
                saveButton.IsEnabled = true;
                loadButton.IsEnabled = false;
            }
        }

        private void Button_StopClick(object sender, RoutedEventArgs e)
        {
            packetListViewModel.StopSniffingPackets();

            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            saveButton.IsEnabled = true;
            loadButton.IsEnabled = true;
        }

        private void Button_SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            
            if(saveDialog.ShowDialog() == true)
            {
                //PacketLogWriter logWriter = new PacketLogWriter();
                //logWriter.WritePacketListLog(packetListViewModel.PacketList.ToList(), saveDialog.FileName);
            }

        }

        private void Button_LoadClick(object sender, RoutedEventArgs e)
        {
            packetListViewModel.PacketList.Clear();

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;

            if(openDialog.ShowDialog() == true)
            {
                List<Dictionary<string, string>> tempPacketListDict = new List<Dictionary<string, string>>();
                //PacketLogParser logParser = new PacketLogParser();
                //logParser.ReadLogFile(openDialog.FileName, ref tempPacketListDict);

                //LogPacketFactory packetFactory = new LogPacketFactory();

                foreach (var packetDict in tempPacketListDict)
                {
                    //IPacket packet = packetFactory.CreatePacket(packetDict);
                    //packetListViewModel.PacketList.Add(packet);
                }          
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    packetListViewModel.SearchPacketList(searchTextBox.Text);
                    packetDataGrid.ItemsSource = packetListViewModel.SearchResultList;
                    break;

                case Key.Escape:
                    searchTextBox.Clear();
                    break;            
            }
        }

        private void Button_SearchClearClick(object sender, RoutedEventArgs e)
        {
            if (packetDataGrid.ItemsSource != packetListViewModel.PacketList)
            {
                packetDataGrid.ItemsSource = packetListViewModel.PacketList;
                shownPacketsLabel.Content = "Shown Packets: ";
            }
        }

        private void PacketListFilter_ShownPacketCountUpdate(object sender, UpdateShownPacketsCounterEventArgs e)
        {
            shownPacketsLabel.Content = "Shown Packets: " + e.Count; 
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

            packetListViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetListViewModel.SearchResultList;

            searchTextBox.Text = filterString;

        }

        private void MenuItem_TcpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as TcpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["dst"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["src"];

            packetListViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetListViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_Ip4FilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"];

            packetListViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetListViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_EthernetFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ethDestinationMacAdr = " + dataGridPacket.EthDestinationMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "|" + "ethDestinationMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthDestinationMacAdr;

            packetListViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetListViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }
    }
}
