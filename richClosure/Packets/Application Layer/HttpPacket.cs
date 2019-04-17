using System.Collections.Generic;
using richClosure.Packets.TransportLayer;

namespace richClosure.Packets.ApplicationLayer
{
    class HttpPacket : TcpPacket
    {
        public Dictionary<string, string> HttpFieldsDict { get; private set; }

        public HttpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetHttpPacketValues(valuesDictionary);
            SetDisplayedProtocol("HTTP");
        }

        private void SetHttpPacketValues(Dictionary<string, object> valuesDictionary)
        {
            HttpFieldsDict = (Dictionary<string, string>)valuesDictionary["HttpFieldsDict"];
        }
    }
}
