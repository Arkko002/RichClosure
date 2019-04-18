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
using richClosure.ViewModels;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        PacketCollectionViewModel packetCollectionViewModel = new PacketCollectionViewModel();
        
        object _packetListLockObject = new object();
        

        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => tokenSource.Cancel();

            BindingOperations.EnableCollectionSynchronization(packetCollectionViewModel.PacketObservableCollection, _packetListLockObject);

            packetCollectionViewModel.PacketObservableCollection.CollectionChanged += PacketList_totalPacketsCountUpdate;

            stopButton.IsEnabled = false;
        }

        private void PacketList_totalPacketsCountUpdate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!tokenSource.IsCancellationRequested)
            {
                Dispatcher.Invoke(() =>
                {
                    totalPacketsLabel.Content = "Total packets: " + packetCollectionViewModel.PacketObservableCollection.Count;
                });
            }
        }

        private void AdapterSelection_adapterSelected(object sender, AdapterSelectedEventArgs e)
        {
            NetworkInterface selectedInterface = e.Adapter;
            //packetCollectionViewModel.StartSniffingPackets(selectedInterface);       
        }


        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            packetCollectionViewModel.PacketObservableCollection.Clear();

            AdapterSelectionWindow adapterSelection = new AdapterSelectionWindow();
            adapterSelection.AdapterSelected += AdapterSelection_adapterSelected;

            if (adapterSelection.ShowDialog() == true)
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    packetCollectionViewModel.SearchPacketList(searchTextBox.Text);
                    packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;
                    break;

                case Key.Escape:
                    searchTextBox.Clear();
                    break;            
            }
        }

        private void Button_SearchClearClick(object sender, RoutedEventArgs e)
        {
            if (packetDataGrid.ItemsSource != packetCollectionViewModel.PacketObservableCollection)
            {
                packetDataGrid.ItemsSource = packetCollectionViewModel.PacketObservableCollection;
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

            packetCollectionViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;

        }

        private void MenuItem_TcpFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as TcpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["dst"] +
                "&" + "tcpPorts = " + dataGridPacket.TcpPorts["src"];

            packetCollectionViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_Ip4FilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ip4Adrs = " + dataGridPacket.Ip4Adrs["dst"] +
                "&" + "ip4Adrs = " + dataGridPacket.Ip4Adrs["src"];

            packetCollectionViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }

        private void MenuItem_EthernetFilterClick(object sender, RoutedEventArgs e)
        {
            var dataGridPacket = packetDataGrid.SelectedItem as IpPacket;

            string filterString = "ethDestinationMacAdr = " + dataGridPacket.EthDestinationMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "|" + "ethDestinationMacAdr = " + dataGridPacket.EthSourceMacAdr +
                "&" + "ethSourceMacAdr = " + dataGridPacket.EthDestinationMacAdr;

            packetCollectionViewModel.SearchPacketList(filterString);
            packetDataGrid.ItemsSource = packetCollectionViewModel.SearchResultList;

            searchTextBox.Text = filterString;
        }
    }
}
