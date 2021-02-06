using GUI.WPF.ViewModels;

namespace GUI.WPF.Views
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
