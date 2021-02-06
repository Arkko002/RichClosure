using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            //TODO
            ViewModel = new MainWindowViewModel();
            
            // TODO MVVM multi-threading

            AvaloniaXamlLoader.Load(this);
        }
    }
}
