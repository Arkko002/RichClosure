using System.Reflection;
using Avalonia;
using Avalonia.Markup.Xaml;
using PacketSniffer;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;
using richClosure.Avalonia.Views;
using Splat;

namespace richClosure.Avalonia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.Register(() => new InterfaceSelectionViewModel(), typeof(IViewFor<InterfaceSelectionView>));
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            AppBootstrapper bootstrapper = new();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            var mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel( Locator.Current.GetService<IPacketSniffer>())
            };
            mainWindow.Show();
        }
    }
}
