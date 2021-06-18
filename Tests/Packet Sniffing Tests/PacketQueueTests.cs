namespace richClosure.Tests.Packet_Sniffing_Tests
{
    [TestFixture]
    class PacketQueueTests
    {
        private PacketQueue _queue;

        [SetUp]
        public void SetUp()
        {
            _queue = new PacketQueue();
        }

        [Test]
        public void AddAndRemoveItem_ReturnsItem()
        {
            byte[] byteArr = {1, 2};

            _queue.EnqueuePacket(byteArr);
            byte[] result = _queue.DequeuePacket();

            Assert.AreEqual(byteArr, result);
        }
    }
}
