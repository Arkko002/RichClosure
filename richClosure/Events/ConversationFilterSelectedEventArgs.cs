using System;

namespace richClosure.Events
{
    public class ConversationFilterSelectedEventArgs : EventArgs
    {
        public string FilterString { get; set; }
    }
}