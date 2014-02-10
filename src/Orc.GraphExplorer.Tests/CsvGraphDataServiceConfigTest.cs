using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests
{
    [TestClass]
    public class CsvGraphDataServiceConfigTest
    {
        [TestMethod]
        public void CsvGraphDataServiceConfig_EnableProperty_Test()
        {
            //var svc = new CsvGraphDataService();

            var config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            Assert.IsNotNull(config);

            config.EnableProperty = true;

            GraphExplorerSection.Current.Save();

            //svc = new CsvGraphDataService();

            config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            Assert.IsTrue(config.EnableProperty);

            config.EnableProperty = false;

            GraphExplorerSection.Current.Save();

            //svc = new CsvGraphDataService();

            config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            Assert.IsFalse(config.EnableProperty);
        }

        [TestMethod]
        public void ConfigurationChanged_Test()
        {
            var config = CsvGraphDataServiceConfig.Current;

            Assert.IsNotNull(config);



        }
    }
}
