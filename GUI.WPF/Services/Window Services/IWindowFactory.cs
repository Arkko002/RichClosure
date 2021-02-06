using GUI.WPF.ViewModels;

namespace GUI.WPF.Services.Window_Services
{
    public interface IWindowFactory
    {
        Window CreateWindow(Type windowType, IViewModel viewModel);
    }
}
