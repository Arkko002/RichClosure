using System;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;

namespace richClosure.Avalonia.ViewModels
{
    public class ViewModelBase : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; set;  }

        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        public ReactiveWindow<T> GetView<T>(T viewModel) where T : ViewModelBase
        {
            var view = Locator.Current.GetService<IViewFor<T>>();
            view.ViewModel = viewModel;
            
            var window = view as ReactiveWindow<T>;
            if (window == null)
            {
                throw new TypeAccessException("View doesn't implement IViewFor");
            }

            return window;

        }
    }
}