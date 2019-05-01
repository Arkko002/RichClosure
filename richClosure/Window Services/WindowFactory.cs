using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using richClosure.ViewModels;

namespace richClosure
{
    public class WindowFactory : IWindowFactory
    {
        public Window CreateWindow(string windowName, IViewModel viewModel)
        {
            string windowClass = "richClosure.Views." + windowName;
            Type type = Assembly.GetExecutingAssembly().GetType(windowClass);

            Window window = (Window)Activator.CreateInstance(type);
            window.DataContext = viewModel;

            return window;
        }
    }
}
