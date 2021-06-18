using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace richClosure.Avalonia.Models
{
    public class PacketTreeItem
    {
        public string Header { get; set; }
        public ObservableCollection<PacketTreeItem> ChildItems { get; set; }

        public void AddChild(PacketTreeItem child)
        {
            Dispatcher.UIThread.InvokeAsync(() => ChildItems.Add(child));
        }
    }
}