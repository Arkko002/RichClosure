using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void ShowWindow(string windowName)
        {
            var window = _windowFactory.CreateWindow(windowName);
            window.Show();
        }
    }
}
