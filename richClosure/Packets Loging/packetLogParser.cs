//using System.Collections.Generic;
//using System.IO;

//namespace richClosure
//{
//    class PacketLogParser
//    {
//        public void ReadLogFile(string logFileLocation, ref List<Dictionary<string, string>> packetList)
//        {
//            using (StreamReader sr = new StreamReader(logFileLocation))
//            {
//                while (!sr.EndOfStream)
//                {
//                    string line;
//                    Dictionary<string, string> dict = new Dictionary<string, string>();

//                    while ((line = sr.ReadLine()) != "--------------------------------")
//                    {
//                        if (line.Contains("udpData:"))
//                        {
//                            var udpData = line.Split(':');
//                            dict.Add(udpData[0], udpData[1]);

//                            while ((line = sr.ReadLine()) != "--------------------------------")
//                            {
//                                dict["udpData"] += line;
//                            }
//                            break;
//                        }
//                        else if (line.Contains("tcpData:"))
//                        {

//                            var tcpData = line.Split(':');
//                            dict.Add(tcpData[0], tcpData[1]);

//                            while ((line = sr.ReadLine()) != "--------------------------------")
//                            {
//                                dict["tcpData"] += line;
//                            }
//                            break;
//                        }

//                        var items = line.Split(':');
//                        dict.Add(items[0], items[1]);
//                    }

//                    packetList.Add(dict);
//                }
//            }
//        }

//        public void ReadFromPacketId(string logFileLocation, uint packetId)
//        {

//        }
//    }
//}
