using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using richClosure.Commands;

namespace richClosure.Tests.Commands_Tests
{
    [TestFixture]
    class RelayCommandTests
    {
        private bool isExecuted;

        [SetUp]
        public void SetUp()
        {
            isExecuted = false;
        }

        [Test]
        public void ExecuteMethod_MethodFired()
        {
            var command = new RelayCommand( x => TestMethod(), x => true);
            command.Execute(null);

            Assert.IsTrue(isExecuted);
        }

        [Test]
        public void ExecuteMethod_CantExecute()
        {
            var command = new RelayCommand(x => TestMethod(), x => false);
            command.Execute(null);

            Assert.IsFalse(isExecuted);
        }

        private void TestMethod()
        {
            isExecuted = true;
        }
    }
}
