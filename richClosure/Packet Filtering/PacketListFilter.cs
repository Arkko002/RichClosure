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
            LookupSingleProperty(searchQuery, propertyInfos);
        }

        private void SearchListWithAnd(List<SearchQuery> searchQueries)
        {
            List<PropertyInfo> searchedProperties = new List<PropertyInfo>();
            List<string[]> conditionStringsList = new List<string[]>();

            foreach (var query in searchQueries)
            {
                searchedProperties.AddRange(GetPacketTypeProperties(query.SearchedProperty));              
            }

            LookupSeveralProperties(searchQueries, searchedProperties);
        }

        //TODO Break this down into shared code, then do "LookupSingleProperty" and "LookupSeveralProperties"
        private void LookupSingleProperty(SearchQuery searchQuery, List<PropertyInfo> searchedProperties)                               
        {
            foreach (IPacket packet in _mainPacketList)
            {
                foreach (var searchedProperty in searchedProperties)
                {
                    if (CheckIfDeclaringTypeMatches(searchedProperty, packet))
                    {
                        continue;
                    }
                    
                    if (ComparePacketsAndSearchedValues(searchedProperty, searchQuery, packet))
                    {
                        _resultList.Add(packet);
                    }
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
        private void LookupSeveralProperties(List<SearchQuery> searchQueries, List<PropertyInfo> searchedProperties)
        {
            foreach (IPacket packet in _mainPacketList)
            {
                List<bool> propertiesMatched = new List<bool>();

                foreach (var searchQuery in searchQueries)
                {
                    foreach (var searchedProperty in searchedProperties)
                    {
                        if (!CheckIfDeclaringTypeMatches(searchedProperty, packet))
                        {
                            propertiesMatched.Add(false);
                            continue;
                        }

                        propertiesMatched.Add(ComparePacketsAndSearchedValues(searchedProperty, searchQuery, packet));
                    }
                }

                if (propertiesMatched.Count == searchQueries.Count && propertiesMatched.All(p => p == true))
                {
                    if (!_resultList.Contains(packet))
                    {
                        _resultList.Add(packet);
                    }
                }
            }
        }

        //TODO Get rid of ifs
        private bool ComparePacketsAndSearchedValues(PropertyInfo searchedProperty, SearchQuery searchQuery, IPacket packet)
        {
            if (searchedProperty.PropertyType == typeof(Dictionary<string, string>))
            {
                return CheckIfExistsInDictionary(searchedProperty, packet, searchQuery.SearchedValue);
            }

            if (searchedProperty.PropertyType == typeof(string) || searchedProperty.PropertyType.IsEnum ||
                searchedProperty.PropertyType == typeof(CustomBool))
            {
                string packetValue = searchedProperty.GetValue(packet).ToString();

                return CompareStr(searchQuery.OperatorStr, packetValue, searchQuery.SearchedValue);
            }

            if (IsNumericType(searchedProperty.PropertyType))
            {
                //Convert all values into ulong to prevent unnecessary CompareNum overloads
                ulong packetValue = (ulong)searchedProperty.GetValue(packet);
                ulong conditionValue = ulong.Parse(searchQuery.SearchedValue);

                return CompareNum(searchQuery.OperatorStr, packetValue, conditionValue);
            }

            return false;
        }

        private bool CheckIfDeclaringTypeMatches(PropertyInfo searchedProperty, IPacket packet)
        {
            if (!searchedProperty.Name.ToLower().Contains("ip"))
            {
                if (searchedProperty.DeclaringType != packet.GetType())
                {
                    return false;
                }

                return true;
            }

            return true;
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
            List<Type> types = GetAllPacketTypesInAssembly();

            List<PropertyInfo> SearchedProperties = new List<PropertyInfo>();
            foreach (var type in types)
            {
                PropertyInfo[] assemblyPacketProperties = type.GetProperties();
                SearchedProperties = FindSearchedProperties(searchedProperty, assemblyPacketProperties);
            }

            return SearchedProperties;
        }

        private List<Type> GetAllPacketTypesInAssembly()
        {
            var iPacType = typeof(IPacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => iPacType.IsAssignableFrom(p) && !p.IsInterface).ToList();

            return types;
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

        private bool CompareNum(string op, ulong x, ulong y)
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

        private bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }
}