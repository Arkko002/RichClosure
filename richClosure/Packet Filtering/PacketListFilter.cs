using richClosure.Packets;
using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using richClosure.Packet_Filtering;

namespace richClosure
{
    class PacketListFilter
    {
        //TODO Write unit tests for this

        private List<IPacket> _mainPacketList;
        private List<IPacket> _resultList;

        public PacketListFilter(List<IPacket> mainPacketList)
        {
            _mainPacketList = mainPacketList;
            _resultList = new List<IPacket>();
        }

        public void SearchList(string conditionString)
        {           
            List<SearchQuery> OrSearchQueries = SearchStringParser.ParseOrString(conditionString);

            foreach (var query in OrSearchQueries)
            {
                SearchListWithSingleCondition(query);
            }
            
            List<SearchQuery> AndSearchQueries = SearchStringParser.ParseAndString(conditionString);

            if (AndSearchQueries.Count > 0)
            {
                SearchListWithAnd(AndSearchQueries);
            }
   
        }

        private void SearchListWithSingleCondition(SearchQuery searchQuery)
        {
            List<PropertyInfo> propertyInfos = GetPacketTypeProperties(searchQuery.SearchedProperty);
            LookupSinglePacket(searchQuery, propertyInfos);
        }

        private void SearchListWithAnd(List<SearchQuery> searchQueries)
        {
            List<PropertyInfo> searchedProperties = new List<PropertyInfo>();
            int boolNum = searchQueries.Count;
            List<string[]> conditionStringsList = new List<string[]>();

            foreach (var query in searchQueries)
            {
                GetPacketTypeProperties(query.SearchedProperty, searchedProperties);              
            }

            LookupSeveralPackets(conditionStringsList, searchedProperties, searchedProperties.Count);
        }

        //TODO Break this down into shared code, then do "LookupSinglePacket" and "LookupSeveralPackets"
        private void LookupSinglePacket(SearchQuery searchQuery, List<PropertyInfo> searchedProperties)
                                   
        {
            foreach (IPacket packet in _mainPacketList)
            {
                bool isFound = false;

                for (int i = 0; i < searchedProperties.Count; i++)
                {
                    if (!searchedProperties[i].Name.ToLower().Contains("ip"))
                    {
                        if (searchedProperties[i].DeclaringType != packet.GetType())
                        {
                            isFound = false;
                            continue;
                        }
                    }

                    if(searchedProperties[i].PropertyType == typeof(Dictionary<string, string>))
                    {
                        isFound = CheckIfExistsInDictionary(searchedProperties[i], packet, searchQuery.SearchedValue);
                    }
                    else if (searchedProperties[i].PropertyType == typeof(string) || searchedProperties[i].PropertyType.IsEnum)
                    {                        
                        string packetValue = searchedProperties[i].GetValue(packet).ToString();

                        isFound = CompareStr(searchQuery.OperatorStr, packetValue, searchQuery.SearchedValue);                   
                    }
                    else if (searchedProperties[i].PropertyType == typeof(CustomBool))
                    {
                        string boolStr = searchedProperties[i].GetType().ToString();

                        isFound = CompareStr(searchQuery.OperatorStr, boolStr, searchQuery.SearchedValue);
                    }
                    else
                    {
                        UInt32 packetValue = UInt32.Parse(searchedProperties[i].GetValue(packet).ToString());
                        UInt32 conditionValue = UInt32.Parse(searchQuery.SearchedValue);

                        isFound = CompareNum(searchQuery.OperatorStr, packetValue, conditionValue);
                    }
                }

                if (isFound)
                {
                    _resultList.Add(packet);
                }

                //if (boolList.Count == boolNum && boolList.All(e => e))
                //{
                //    if (!_resultList.Contains(packet))
                //    {
                //        _resultList.Add(packet);
                //    }
                //}
            }
        }

        //TODO
        private void LookupSeveralPackets(List<SearchQuery> searchQueries, List<PropertyInfo> searchedProperties)
        {
            foreach (IPacket packet in _mainPacketList)
            {
                bool isFound = false;

                for (int i = 0; i < searchedProperties.Count; i++)
                {
                    
                }
            }
        }

        private bool CheckIfExistsInDictionary(PropertyInfo propertyInfo, IPacket packet, string value)
        {
            Dictionary<string, string> dictObject = propertyInfo.GetValue(packet, null) as Dictionary<string, string>;

            foreach (var item in dictObject)
            {
                if (item.Value == value)
                {
                    return true;
                }
            }

            return false;
        }

        private List<PropertyInfo> GetPacketTypeProperties(string searchedProperty)
        {
            //Find all of properties of packets in current assembly
            var ipacType = typeof(IPacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => ipacType.IsAssignableFrom(p) && !p.IsInterface);


            List<PropertyInfo> SearchedProperties = new List<PropertyInfo>();
            foreach (var type in types)
            {
                PropertyInfo[] assemblyPacketProperties = type.GetProperties();
                SearchedProperties = FindSearchedProperties(searchedProperty, assemblyPacketProperties);
            }

            return SearchedProperties;
        }

        private List<PropertyInfo> FindSearchedProperties(string searchedProperty, PropertyInfo[] assemblyPacketProperties)
        {
            if(searchedProperty.Contains('.'))
            {
                //FindNestedProperties(outputList, searchedPropertyStr, assemblyPacketProperties);
            }

            List<PropertyInfo> outputList = new List<PropertyInfo>();
            foreach (var property in assemblyPacketProperties)
            {
                if (property.Name.ToLower() == searchedProperty.ToLower())
                {
                    outputList.Add(property);
                }
            }

            return outputList;
        }

        //TODO This isn't finished
        //private List<PropertyInfo> FindNestedProperties(string searchedPropertyStr, PropertyInfo[] assemblyPacketProperties)
        //{
        //    string[] propertyParts = searchedPropertyStr.Split('.');
        //    PropertyInfo currentProperty = null;
           
        //    foreach (var property in assemblyPacketProperties)
        //    {
        //        if (property.Name.ToLower() == propertyParts[0].ToLower())
        //        {
        //            currentProperty = property;
        //        }
        //    }

        //    for (int i = 1; i < propertyParts.Length; i++)
        //    {
        //        Type obj = currentProperty.DeclaringType;
        //        var nestedInfo = obj.GetProperties();
        //    }          
        //}

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