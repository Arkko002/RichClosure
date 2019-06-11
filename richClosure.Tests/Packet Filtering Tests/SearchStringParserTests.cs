using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using richClosure.Packet_Filtering;

namespace richClosure.Tests.Packet_Filtering_Tests
{
    [TestFixture]
    class SearchStringParserTests
    {
        private SearchStringParser parser = new SearchStringParser();

        [Test]
        public void ParseOrString_ReturnsSearchQueries()
        {
            string testStr = "One = 1 | Two =< 2";

            var result = parser.ParseString(testStr);

            Assert.IsNotEmpty(result.OrQueries);
            Assert.IsTrue(result.OrQueries[0].SearchedProperty == "One" && result.OrQueries[0].OperatorStr == "=" && result.OrQueries[0].SearchedValue == "1");
            Assert.IsTrue(result.OrQueries[1].SearchedProperty == "Two" && result.OrQueries[1].OperatorStr == "=<" && result.OrQueries[1].SearchedValue == "2");
        }

        [Test]
        public void ParseString_SingleQuery()
        {
            string testStr = "One = 1";

            var result = parser.ParseString(testStr);

            Assert.IsNotEmpty(result.OrQueries);
            Assert.IsTrue(result.OrQueries[0].SearchedProperty == "One" && result.OrQueries[0].OperatorStr == "=" && result.OrQueries[0].SearchedValue == "1");
        }

        [Test]
        public void ParseString_ReturnsSearchQueries()
        {
            string testStr = "One = 1 & Two =< 2";

            var result = parser.ParseString(testStr);

            Assert.IsNotEmpty(result.AndQueries);
            Assert.IsTrue(result.AndQueries[0].SearchedProperty == "One" && result.AndQueries[0].OperatorStr == "=" && result.AndQueries[0].SearchedValue == "1");
            Assert.IsTrue(result.AndQueries[1].SearchedProperty == "Two" && result.AndQueries[1].OperatorStr == "=<" && result.AndQueries[1].SearchedValue == "2");
        }
    }
}
