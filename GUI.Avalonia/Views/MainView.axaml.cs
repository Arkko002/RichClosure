using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using richClosure.Avalonia.ViewModels;
using Splat;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
