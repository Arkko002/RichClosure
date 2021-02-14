using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using richClosure.Avalonia.ViewModels;
using Splat;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : ReactiveWindow<MainWindowViewModel>
    {
        public MainView()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var interfaceDialog = new InterfaceSelectionView();
            interfaceDialog.ShowDialog(this);
            //TODO
            ViewModel.StartSniffing();
        }
    }
}
