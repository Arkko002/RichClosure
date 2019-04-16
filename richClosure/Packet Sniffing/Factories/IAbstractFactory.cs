using System.IO;


namespace richClosure.Packet_Sniffing.Factories
{
    interface IAbstractFactory
    {
        IPacket CreatePacket();
    }
}
