using System;

namespace richClosure.Avalonia.Events
{
    public class ConversationFilterSelectedEventArgs : EventArgs
    {
        public string FilterString { get; set; }
    }
}