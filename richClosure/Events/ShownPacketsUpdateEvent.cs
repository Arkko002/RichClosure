using System;

namespace richClosure
{
    public class UpdateShownPacketsCounterEventArgs : EventArgs
    {
        public uint Count { get; set; }

        public UpdateShownPacketsCounterEventArgs(uint count)
        {
            Count = count;
        }
    }
}
