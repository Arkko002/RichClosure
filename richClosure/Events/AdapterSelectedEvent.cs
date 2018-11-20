using System;
using System.Net.NetworkInformation;

namespace richClosure
{
    public class AdapterSelectedEventArgs : EventArgs
    {
        public NetworkInterface Adapter { get; set; }

        public AdapterSelectedEventArgs(NetworkInterface adapterSelected)
        {
            Adapter = adapterSelected;
        }
    }
}
