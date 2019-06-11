using System.Windows;
using richClosure.ViewModels;

namespace richClosure.Views
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
            vm.WindowManager.ClosingRequest += (sender, e) => Close();
        }
    }
}
