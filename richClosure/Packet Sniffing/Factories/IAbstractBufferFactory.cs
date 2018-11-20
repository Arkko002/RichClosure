using System.IO;


namespace richClosure.Packet_Sniffing.Factories
{
    interface IAbstractBufferFactory
    {
        IPacket CreatePacket(byte[] buffer, BinaryReader binaryReader);
    }
}
