using System.Windows;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for AdapterSelectionWindow.xaml
    /// </summary>
    public partial class AdapterSelectionWindow : Window
    {
        public delegate void AdapterSelectedEventHandler(object sender, AdapterSelectedEventArgs e);
        public event AdapterSelectedEventHandler AdapterSelected;

        //TODO VM for this view
        public AdapterSelectionWindow()
        {
            InitializeComponent();

            List<NetworkInterface> adapterList = new List<NetworkInterface>();

            foreach(NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                adapterList.Add(adapter);
            }

            AdapterDataGrid.ItemsSource = adapterList;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            AdapterSelectedEventArgs eventArgs = new AdapterSelectedEventArgs(AdapterDataGrid.SelectedItem as NetworkInterface);
            OnAdapterSelected(eventArgs);
            DialogResult = true;
        }

        protected void OnAdapterSelected(AdapterSelectedEventArgs e)
        {
            AdapterSelected?.Invoke(this, e);
        }
    }
}
