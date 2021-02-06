using GUI.WPF.ViewModels;

namespace GUI.WPF.Services.Window_Services
{
    public interface IWindowManager
    {
        event EventHandler ClosingRequest;

        void ShowWindow(Type windowType, IViewModel viewModel);
        void CloseWindow();
    }
}
