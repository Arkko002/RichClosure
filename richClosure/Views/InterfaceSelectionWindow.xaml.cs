using System.Windows;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System;
using richClosure.ViewModels;

namespace richClosure
{
    /// <summary>
    /// Interaction logic for InterfaceSelectionWindow.xaml
    /// </summary>
    public partial class InterfaceSelectionWindow : Window
    {
        public InterfaceSelectionWindow(InterfaceSelectionViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            vm.ClosingRequest += (sender, e) => Close();
        }
    }
}
