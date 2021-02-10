using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PacketSniffer.Packet_Sniffing;

namespace richClosure.Tests.Packet_Sniffing_Tests
{
    [TestFixture]
    class SnifferThreadsTests
    {
        private SnifferThreads _threads;

        private volatile bool firstBool;
        private volatile bool secondBool;

        [Test]
        public void LaunchingThreads_ThreadsOperateCorrectly()
        {
            _threads = new SnifferThreads();
            _threads.AssignMethodsToThreads(ChangeFirstBool, ChangeSecondBool);

            _threads.StartThreads();

            Assert.IsTrue(firstBool);
            Assert.IsTrue(secondBool);
        }

        private void ChangeFirstBool()
        {
            firstBool = true;
        }

        private void ChangeSecondBool()
        {
            secondBool = true;
        }
    }
}
