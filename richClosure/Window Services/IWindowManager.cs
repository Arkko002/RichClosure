using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using richClosure.ViewModels;

namespace richClosure
{
    public interface IWindowManager
    {
        void ShowWindow(Type windowType, IViewModel viewModel);
        void CloseWindow(Window window);
    }
}
