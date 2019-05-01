using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using richClosure.ViewModels;

namespace richClosure
{
    public interface IWindowManager
    {
        void ShowWindow(string windowName, IViewModel viewModel);
        void CloseWindow(string windowName);
    }
}
