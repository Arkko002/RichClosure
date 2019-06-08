using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace richClosure.Packet_Sniffing
{
    class SnifferThreads
    {
        private Thread enqueueThread;
        private Thread dequeueThread;

        public SnifferThreads(ThreadStart enqueueMethod, ThreadStart dequeueMethod)
        {
            enqueueThread = new Thread(enqueueMethod);
            dequeueThread = new Thread(dequeueMethod);
        }

        public void StartThreads()
        {
            enqueueThread.Start();
            dequeueThread.Start();
        }
    }
}
