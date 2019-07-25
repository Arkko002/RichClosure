using System;
using richClosure.ViewModels;

namespace richClosure.Window_Services
{
    public class WindowManager : IWindowManager
    {
        private readonly IWindowFactory _windowFactory;

        public event EventHandler ClosingRequest;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void CloseWindow()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }

        public void ShowWindow(Type windowType, IViewModel viewModel)
        {
            var window = _windowFactory.CreateWindow(windowType, viewModel);
            window.Show();
        }
    }
}
