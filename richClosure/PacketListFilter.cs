using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace richClosure
{
    class PacketListFilter
    {
        public void SearchList(List<IPacket> mainPacketList, List<IPacket> resultList, string conditionString)
        {
            SearchStringParser stringParser = new SearchStringParser();
            string[] parsedOrStrings = stringParser.ParseOrString(conditionString);

            if(parsedOrStrings != null)
            {
                for (int i = 0; i < parsedOrStrings.Length; i++)
                {
                    List<string[]> conStrSegments = new List<string[]>();
                    List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
                    conStrSegments.Add(parsedOrStrings[i].Split(' '));
                    GetPacketTypeProperties(conStrSegments[0][0], propertyInfos);
                    LookupPackets(mainPacketList, resultList, conStrSegments, propertyInfos, 1);
                }
            }

            string[] parsedAndStrings = stringParser.ParseAndString(conditionString);

            if(parsedAndStrings != null)
            {
                SearchListWithAnd(mainPacketList, resultList, parsedAndStrings);
            }
     
        }

        private void SearchListWithAnd(List<IPacket> mainPacketList, List<IPacket> resultList, string[] conditionStrings)
        {
            List<PropertyInfo> searchedProperties = new List<PropertyInfo>();
            int boolNum = conditionStrings.Length;
            List<string[]> conditionStringsList = new List<string[]>();

            foreach (string str in conditionStrings)
            {
                string[] conditionStrSegments = str.Split(' ');
                conditionStringsList.Add(conditionStrSegments);

                GetPacketTypeProperties(conditionStrSegments[0], searchedProperties);              
            }

            LookupPackets(mainPacketList, resultList, conditionStringsList, searchedProperties, searchedProperties.Count);
        }

        private void LookupPackets(List<IPacket> mainPacketList, List<IPacket> resultList, List<string[]> conditionStrSegments, List<PropertyInfo> searchedProperties,
                                   int boolNum)
        {
            foreach (IPacket packet in mainPacketList)
            {
                bool isFound = true;
                List<bool> boolList = new List<bool>();

                for (int i = 0; i < searchedProperties.Count; i++)
                {
                    if (!searchedProperties[i].Name.ToString().ToLower().Contains("ip"))
                    {
                        if (searchedProperties[i].DeclaringType != packet.GetType())
                        {
                            isFound = false;
                            boolList.Add(isFound);
                            continue;
                        }
                    }

                    if(searchedProperties[i].PropertyType == typeof(Dictionary<string, string>))
                    {
                        Dictionary<string, string> dictObject = searchedProperties[i].GetValue(packet, null) as Dictionary<string, string>;

                        foreach(var item in dictObject)
                        {
                            if (item.Value == conditionStrSegments.ElementAt(i)[2])
                            {
                                boolList.Add(isFound);
                            }
                        }
                    }
                    else if (searchedProperties[i].PropertyType == typeof(string) || searchedProperties[i].PropertyType.IsEnum)
                    {                        
                        string packetValue = searchedProperties[i].GetValue(packet).ToString();
                        string conditionValue = conditionStrSegments.ElementAt(i)[2];

                        isFound = CompareStr(conditionStrSegments.ElementAt(i)[1], packetValue, conditionValue);
                        boolList.Add(isFound);                        
                    }
                    else
                    {
                        UInt32 packetValue = UInt32.Parse(searchedProperties[i].GetValue(packet).ToString());
                        UInt32 conditionValue = UInt32.Parse(conditionStrSegments.ElementAt(i)[2]);

                        isFound = CompareNum(conditionStrSegments.ElementAt(i)[1], packetValue, conditionValue);
                        boolList.Add(isFound);
                    }
                }

                if (boolList.Count == boolNum && boolList.All(e => e == true))
                {
                    if (!resultList.Contains(packet))
                    {
                        resultList.Add(packet);
                    }
                }
            }
        }

        private void GetPacketTypeProperties(string command, List<PropertyInfo> searchedProperties)
        {
            if (command.ToLower().Contains("ip"))
            {
                PropertyInfo[] packetProperties = typeof(IpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);

            }
            else if (command.ToLower().Contains("tcp"))
            {
                PropertyInfo[] packetProperties = typeof(TcpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if (command.ToLower().Contains("udp"))
            {
                PropertyInfo[] packetProperties = typeof(UdpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if (command.ToLower().Contains("icmp"))
            {
                PropertyInfo[] packetProperties = typeof(IcmpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if(command.ToLower().Contains("dns"))
            {
                PropertyInfo[] packetProperties = typeof(DnsTcpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if (command.ToLower().Contains("dhcp"))
            {
                PropertyInfo[] packetProperties = typeof(DhcpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if (command.ToLower().Contains("http"))
            {
                PropertyInfo[] packetProperties = typeof(HttpPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
            else if (command.ToLower().Contains("tls"))
            {
                PropertyInfo[] packetProperties = typeof(TlsPacket).GetProperties();

                FindSearchedProperties(searchedProperties, command, packetProperties);
            }
        }

        private void FindSearchedProperties(List<PropertyInfo> outputList, string searchedPropertyStr, PropertyInfo[] packetTypeProperties)
        {
            foreach (var property in packetTypeProperties)
            {
                if (property.Name.ToLower() == searchedPropertyStr.ToLower())
                {
                    outputList.Add(property);
                }
            }
        }

        private bool CompareNum(string op, UInt32 x, UInt32 y)
        {
            switch(op)
            {
                case ">": return x > y;
                case ">=": return x >= y;
                case "<": return x < y;
                case "<=": return x <= y;
                case "=": return x == y;
                case "!=": return x != y;
                default: return false;
            }
        }

        private bool CompareStr(string op, string x, string y)
        {
            switch (op)
            {
                case "has": return x.Contains(y);
                case "=": return x == y;
                case "!=": return x != y;
                default: return false;
            }
        }
    }
}