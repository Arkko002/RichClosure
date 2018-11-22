//using System.Collections.Generic;
//using System.Reflection;

//namespace richClosure
//{
//    class PacketLogWriter
//    {
//        public void WritePacketListLog(List<IPacket> packets, string fileToWrite)
//        {
//            foreach(IPacket packet in packets)
//            {
//                PropertyInfo[] propertyInfo = packet.GetType().GetProperties();

//                foreach(var property in propertyInfo)
//                {
//                    string dataName = property.Name.ToString();
//                    string data = packet.GetType().GetProperty(dataName).GetValue(packet).ToString() + "\n";
//                    data = data.Replace(":", ".");

//                    System.IO.File.AppendAllText(fileToWrite, dataName + ": " + data);
//                }

//                System.IO.File.AppendAllText(fileToWrite, "--------------------------------\n");
//            }
//        }
//    }
//}
