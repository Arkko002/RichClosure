using System;
using System.Windows;
using richClosure.ViewModels;

namespace richClosure.Window_Services
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
