using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace richClosure
{
    public class WindowFactory : IWindowFactory
    {
        public Window CreateWindow(string windowName)
        {
            string windowClass = "richClosure.Views." + windowName;
            Type type = Assembly.GetExecutingAssembly().GetType(windowClass);

            var window = Activator.CreateInstance(type);

            return (Window)window;
        }
    }
}
