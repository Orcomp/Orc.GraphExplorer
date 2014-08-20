using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests
{
    using Config;
    using Services;

    [TestClass]
    public class CsvGraphDataServiceTests
    {
        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void CsvGraphDataService_Constructor_Test()
        {
            var withDefaultCtor = new CsvGraphDataService();
            Assert.AreEqual(withDefaultCtor.Config, CsvGraphDataServiceConfig.Current);

            var config = new CsvGraphDataServiceConfig()
            {
                VertexesFilePath = "PATH1",
                EdgesFilePath = "PATH2"
            };

            var withConfigPassedInCtor = new CsvGraphDataService(config);
            Assert.AreEqual(withConfigPassedInCtor.Config, config);
        }

        [TestMethod]
        public void GetVertexes_From_File_Test()
        {
            var service = new CsvGraphDataService();
            bool callSuccess = false;
            bool callFail = false;

            service.GetVertexes((data) => { callSuccess = true; }, (e) => { callFail = true; });

            Assert.IsFalse(callFail);
            Assert.IsTrue(callSuccess);
        }

        [TestMethod]
        public void PopulateVertexes_Test()
        {
            var listData = new List<CsvGraphDataService.PropertyData>();

            listData.Add(new CsvGraphDataService.PropertyData()
            {
                ID = 1,
                Property = "First Name",
                Value = "ABC11"
            });
            listData.Add(new CsvGraphDataService.PropertyData()
            {
                ID = 1,
                Property = "Last Name",
                Value = "XYZ11"
            });
            listData.Add(new CsvGraphDataService.PropertyData()
            {
                ID = 2,
                Property = "First Name",
                Value = "ABC22"
            });
            listData.Add(new CsvGraphDataService.PropertyData()
            {
                ID = 2,
                Property = "Last Name",
                Value = "XYZ22"
            });

            var vlist = CsvGraphDataService.PopulateVertexes(listData);

            Assert.IsNotNull(vlist);

            Assert.AreEqual(vlist.Count, 2);

            var vertex1 = vlist.First();

            Assert.AreEqual(vertex1.ID, 1);
            Assert.AreEqual(vertex1.Properties.Count(), 2);
            Assert.AreEqual(vertex1.Properties.First(p => p.Key == "First Name").Value, "ABC11");
        }

        [TestMethod]
        public void GetEdges_From_File_Test()
        {
            var service = new CsvGraphDataService();
            bool callSuccess = false;
            bool callFail = false;
            List<DataEdge> resultData = null;
            service.GetEdges((data) =>
            {
                callSuccess = true; resultData = new List<DataEdge>(data);
            },
            (e) =>
            {
                callFail = true;
            });

            Assert.IsFalse(callFail);
            Assert.IsTrue(callSuccess);

            Assert.IsNotNull(resultData);
        }

        [TestMethod]
        public void UpdateVertexes_Test()
        {
            var service = new CsvGraphDataService();
            bool callSuccess = false;
            Exception er;
            ObservableCollection<DataVertex> list = new ObservableCollection<DataVertex>();
            var item1 = new DataVertex(1);
            var listV = new ObservableCollection<Model.PropertyViewmodel>();
            item1.Properties = listV;

            listV.Add(new Model.PropertyViewmodel(1,"First Name", "Payton", item1));
            listV.Add(new Model.PropertyViewmodel(2,"Last Name", "Li", item1));
            listV.Add(new Model.PropertyViewmodel(3,"Age", "29", item1));

            list.Add(item1);

            service.UpdateVertexes(list, (sucess, error) =>
            {
                er = error;
                callSuccess = sucess;
            });

            DataVertex item1FromSource = null;
            service.GetVertexes((data) =>
            {
                item1FromSource = data.FirstOrDefault();

                callSuccess = true;

            }, (e) => { });

            Assert.IsTrue(callSuccess);
            Assert.IsNotNull(item1FromSource);
            Assert.AreEqual(item1.Id, item1FromSource.Id);
        }


        [TestMethod]
        public void UpdateVertex_Test()
        {

            //below are data tobe tested

            //ID,Property,Value
            //11,First Name,ABC11
            //11,Last Name,XYZ11
            //11,Age,13
            //12,First Name,ABC12
            //12,Last Name,XYZ12
            //12,Age,34
            //12,First Name1,ABC12
            //12,Last Name1,XYZ12
            //12,Age1,34
            //12,First Name2,ABC12
            //12,Last Name2,XYZ12
            //12,Age2,34

            var service = new CsvGraphDataService();
            bool callSuccess = false;
            bool callFail = false;
            IEnumerable<DataVertex> resultList = null;
            service.GetVertexes((data) => { callSuccess = true; resultList = data; }, (e) => { callFail = true; });

            Assert.IsNotNull(resultList);
            Assert.IsFalse(callFail);
            Assert.IsTrue(callSuccess);

            var item1 = resultList.FirstOrDefault();

            var list = new ObservableCollection<Model.PropertyViewmodel>();
            item1.Properties = list;

            list.Add(new Model.PropertyViewmodel(1,"First Name", "Payton", item1));
            list.Add(new Model.PropertyViewmodel(2,"Last Name", "Li", item1));
            list.Add(new Model.PropertyViewmodel(3,"Age", "29", item1));

            service.UpdateVertex(item1, (r,data, error) =>
            {
                Assert.IsTrue(r);
                Assert.AreEqual(item1, data);
            });
        }

        [TestMethod]
        public void UpdateEdges_Test()
        {
            var service = new CsvGraphDataService();
            bool callSuccess = false;
            Exception er;
            List<DataEdge> list = new List<DataEdge>();
            var item1 = new DataVertex(1);
            var item2 = new DataVertex(2);
            var edge1 = new DataEdge(item1, item2);

            list.Add(edge1);

            service.UpdateEdges(list, (sucess, error) =>
            {
                er = error;
                callSuccess = sucess;
            });

            DataEdge item1FromSource = null;
            service.GetEdges((data) =>
            {
                item1FromSource = data.FirstOrDefault();

                callSuccess = true;

            }, (e) => { });

            Assert.IsTrue(callSuccess);
            Assert.IsNotNull(item1FromSource);
            Assert.AreEqual(edge1.Source.Id, item1FromSource.Source.Id);
            Assert.AreEqual(edge1.Target.Id, item1FromSource.Target.Id);
        }
    }
}
