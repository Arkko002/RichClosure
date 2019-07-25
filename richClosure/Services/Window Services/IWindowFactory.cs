using System;
using System.Windows;
using richClosure.ViewModels;

namespace richClosure.Window_Services
{
    public interface IWindowFactory
    {
        Window CreateWindow(Type windowType, IViewModel viewModel);
    }
}
