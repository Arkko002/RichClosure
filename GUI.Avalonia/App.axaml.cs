using System.Reflection;
using Avalonia;
using Avalonia.Markup.Xaml;
using PacketSniffer.Services;
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
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            var mainWindow = new MainView()
            {
                DataContext = new MainWindowViewModel(Locator.Current.GetService<PacketDataGridViewModel>(),
                    Locator.Current.GetService<InterfaceSelectionViewModel>(), Locator.Current.GetService<IPacketSniffer>())
            };
            mainWindow.Show();
        }
    }
}
