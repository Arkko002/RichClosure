using GUI.WPF.ViewModels;

namespace GUI.WPF.Services.Window_Services
{
    public class WindowFactory : IWindowFactory
    {
        public Window CreateWindow(Type windowType, IViewModel viewModel)
        {
            Window window = (Window)Activator.CreateInstance(windowType, viewModel);
            window.DataContext = viewModel;

            return window;
        }
    }
}
