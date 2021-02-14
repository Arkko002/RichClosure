using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    public class PacketTreeUserControl : ReactiveWindow<PacketTreeViewModel>
    {
        public TreeView PacketTreeView => this.FindControl<TreeView>("PacketTreeView");
        
        public PacketTreeUserControl()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.TreeViewItems,
                    view => view.PacketTreeView.Items)
                    .DisposeWith(disposable);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}