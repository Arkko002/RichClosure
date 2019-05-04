using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using richClosure.ViewModels;

namespace richClosure
{
    public class WindowManager : IWindowManager
    {
        private IWindowFactory _windowFactory;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void CloseWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(Type windowType, IViewModel viewModel)
        {
            var window = _windowFactory.CreateWindow(windowType, viewModel);
            window.Show();
        }
    }
}
