using System;
using PacketSniffer.Packets;
using ReactiveUI;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketHexViewModel : ViewModelBase 
    {
        public string AsciiData { get; }
        public string HexData { get; }

        
        public PacketHexViewModel(IPacket packet)
        {
            AsciiData = GetAsciiPacketData(packet);
            HexData = GetHexPacketData(packet);
        }
        
        private string GetAsciiPacketData(IPacket packet)
        {
            string[] hexString = packet.PacketData.Split('-');

            string tempAscii = string.Empty;

            foreach (string hexval in hexString)
            {
                uint decval = Convert.ToUInt32(hexval, 16);

                if (decval >= 33 && decval <= 126)
                {
                    char ch = Convert.ToChar(decval);
                    tempAscii += ch;
                }
                else
                {
                    tempAscii += ".";
                }
            }

            string resString = string.Empty;

            for (int x = 1; x <= hexString.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += tempAscii[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += tempAscii[x - 1] + "   ";
                }
                else
                {
                    resString += tempAscii[x - 1] + " ";
                }
            }

            return resString;
        }

        private string GetHexPacketData(IPacket packet)
        {
            string[] hexTempStr = packet.PacketData.Split('-');
            string resString = string.Empty;

            for (int x = 1; x <= hexTempStr.Length; x++)
            {
                if (x % 16 == 0 && x != 0)
                {
                    resString += hexTempStr[x - 1] + "\n";
                }
                else if (x % 8 == 0 && x != 0)
                {
                    resString += hexTempStr[x - 1] + "   ";
                }
                else
                {
                    resString += hexTempStr[x - 1] + " ";
                }
            }

            return resString;
        }

    }
}