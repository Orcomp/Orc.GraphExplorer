using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests
{
    [TestClass]
    public class GraphExplorerSectionTests
    {
        [TestMethod]
        public void GraphExplorerSection_ConfigChanged_Test()
        {
            bool handlerCalled = false;

            GraphExplorerSection.ConfigurationChanged += (s, e) =>
            {
                handlerCalled = true;
            };

            GraphExplorerSection.Current.Save();

            Assert.IsTrue(handlerCalled);
        }
    }
}
