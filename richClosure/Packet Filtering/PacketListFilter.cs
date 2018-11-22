using richClosure.Packets;
using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace richClosure
{
    class PacketListFilter
    {
        public void SearchList(List<IPacket> mainPacketList, List<IPacket> resultList, string conditionString)
        {           
            string[] parsedOrStrings = SearchStringParser.ParseOrString(conditionString);

            if(!(parsedOrStrings is null))
            {
                for (int i = 0; i < parsedOrStrings.Length; i++)
                {
                    SearchListWithSingleCondition(mainPacketList, resultList, parsedOrStrings[i]);                                    
                }
            }

            string[] parsedAndStrings = SearchStringParser.ParseAndString(conditionString);

            if(!(parsedAndStrings is null))
            {
                SearchListWithAnd(mainPacketList, resultList, parsedAndStrings);
            }
     
            if(!(conditionString is null))
            {
                SearchListWithSingleCondition(mainPacketList, resultList, conditionString);
            }
        }

        private void SearchListWithSingleCondition(List<IPacket> mainPacketList, List<IPacket> resultList, string conditionString)
        {
            List<string[]> conStrSegments = new List<string[]>();
            conStrSegments.Add(SearchStringParser.ParseStringSegments(conditionString));
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            GetPacketTypeProperties(conStrSegments[0][0], propertyInfos);
            LookupPackets(mainPacketList, resultList, conStrSegments, propertyInfos, 1);
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
                    else if (searchedProperties[i].PropertyType == typeof(CustomBool))
                    {
                        string boolStr = searchedProperties[i].GetType().ToString();
                        string conditionValue = conditionStrSegments.ElementAt(i)[2];

                        isFound = CompareStr(conditionStrSegments.ElementAt(i)[1], boolStr, conditionValue);
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
            var ipacType = typeof(IPacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => ipacType.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
            {
                PropertyInfo[] packetProperties = type.GetProperties();

                foreach (var prop in packetProperties)
                {
                    if(prop.Name.ToLower() == command.ToLower())
                    {
                        FindSearchedProperties(searchedProperties, command, packetProperties);
                    }
                }
            }
        }

        private void FindSearchedProperties(List<PropertyInfo> outputList, string searchedPropertyStr, PropertyInfo[] packetTypeProperties)
        {
            if(searchedPropertyStr.Contains('.'))
            {
                FindNestedProperties(outputList, searchedPropertyStr, packetTypeProperties);
            }

            foreach (var property in packetTypeProperties)
            {
                if (property.Name.ToLower() == searchedPropertyStr.ToLower())
                {
                    outputList.Add(property);
                }
            }
        }

        private void FindNestedProperties(List<PropertyInfo> outputList, string searchedPropertyStr, PropertyInfo[] packetTypeProperties)
        {
            string[] propertyParts = searchedPropertyStr.Split('.');
            PropertyInfo currentProperty = null;
           
            foreach (var property in packetTypeProperties)
            {
                if (property.Name.ToLower() == propertyParts[0].ToLower())
                {
                    currentProperty = property;
                }
            }

            for (int i = 1; i < propertyParts.Length; i++)
            {
                Type obj = currentProperty.DeclaringType;
                var nestedInfo = obj.GetProperties();
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