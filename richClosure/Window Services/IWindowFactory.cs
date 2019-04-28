using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace richClosure
{
    public interface IWindowFactory
    {
        Window CreateWindow(string windowName);
    }
}
