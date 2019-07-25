using System;
using richClosure.ViewModels;

namespace richClosure.Window_Services
{
    public interface IWindowManager
    {
        event EventHandler ClosingRequest;

        void ShowWindow(Type windowType, IViewModel viewModel);
        void CloseWindow();
    }
}
