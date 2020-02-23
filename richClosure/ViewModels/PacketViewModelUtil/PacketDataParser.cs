using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace richClosure.ViewModels.PacketViewModelUtil
{
    class PacketDataParser
    {
        private string PacketData;

        public PacketDataParser(string packetData)
        {
            PacketData = packetData;
        }

        public string GetAsciiPacketData()
        {
            string[] hexString = PacketData.Split('-');

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

        public string GetHexPacketData()
        {
            string[] hexTempStr = PacketData.Split('-');
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
