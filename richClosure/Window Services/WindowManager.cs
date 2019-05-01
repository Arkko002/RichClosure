using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void CloseWindow(string windowName)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(string windowName, IViewModel viewModel)
        {
            var window = _windowFactory.CreateWindow(windowName, viewModel);
            window.Show();
        }
    }
}
