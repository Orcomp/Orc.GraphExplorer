using GraphX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests
{
    [TestClass]
    public class OperationTests
    {
        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void CreateEdgeOperation_Do_Test()
        {
            var graph = new Model.GraphArea();

            var source = new DataVertex(100) { };
            var sourceVC = new VertexControl(source);
            graph.Graph.AddVertex(source);
            graph.AddVertex(source, sourceVC);

            var target = new DataVertex(101) { };
            var targetVC = new VertexControl(target);
            graph.Graph.AddVertex(target);
            graph.AddVertex(target, targetVC);

            bool called = false;
            EdgeControl ec = null;
            var ceo = new CreateEdgeOperation(graph, source, target, (e) =>
            {
                called = true;
                ec = e;
            },
            (e) =>
            {

            });

            ceo.Do();

            Assert.IsTrue(called);
            Assert.IsNotNull(ec);
            Assert.AreEqual<VertexControl>(ec.Source, sourceVC, "source are not equal");
            Assert.AreEqual<VertexControl>(ec.Target, targetVC, "target are not equal");
        }

        [TestMethod]
        public void VertexOperation_Undo_Redo_Test()
        {

            var graph = new Model.GraphArea();

            var source = new DataVertex(100) { };

            var target = new DataVertex(101) { };

            bool createEdgeCalled = false;
            bool undoCreateEdgeCalled = false;

            bool createSourceCalled = false;
            bool undoCreateSourceCalled = false;

            bool createTargetCalled = false;
            bool undoCreateTargetCalled = false;

            EdgeControl ec = null;
            VertexControl sourceVC = null;
            VertexControl targetVC = null;

            var ceo = new CreateEdgeOperation(graph, source, target, (e) =>
            {
                createEdgeCalled = true;
                ec = e;
            },
            (e) =>
            {
                createEdgeCalled = false;
                undoCreateEdgeCalled = true;
                ec = e;
            });

            var cvoSource = new CreateVertexOperation(graph, source, callback:(sv, svc) =>
            {
                createSourceCalled = true;
                sourceVC = svc;
            },undoCallback:
            (v) =>
            {
                undoCreateSourceCalled = true;
            });

            var cvoTarget = new CreateVertexOperation(graph, target, callback:(sv, svc) =>
            {
                createTargetCalled = true;
                targetVC = svc;
            },undoCallback:(v) =>
            {
                undoCreateTargetCalled = true;
            });

            cvoSource.Do();

            Assert.IsTrue(createSourceCalled);
            Assert.IsNotNull(sourceVC);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.AreEqual(source, sourceVC.Vertex, "source are not equal");

            cvoTarget.Do();

            Assert.IsTrue(createTargetCalled);
            Assert.IsNotNull(targetVC);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == target));
            Assert.AreEqual(target, targetVC.Vertex, "target are not equal");

            ceo.Do();

            Assert.IsTrue(createEdgeCalled);
            Assert.IsNotNull(ec);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == target));
            Assert.IsTrue(graph.Graph.Edges.Any(e => e == ec.Edge));
            Assert.AreEqual<VertexControl>(ec.Source, sourceVC, "source vertex control are not equal");
            Assert.AreEqual<VertexControl>(ec.Target, targetVC, "target vertex control are not equal");

            ceo.UnDo();

            Assert.IsTrue(undoCreateEdgeCalled);
            Assert.IsNotNull(ec);
            Assert.IsNull(ec.Edge);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == target));
            Assert.IsFalse(graph.Graph.Edges.Any(e => e == ec.Edge));
            Assert.AreEqual<VertexControl>(ec.Source, null, "source sould be null");
            Assert.AreEqual<VertexControl>(ec.Target, null, "target sould be null");

            //redo
            ceo.Do();

            Assert.IsTrue(createEdgeCalled);
            Assert.IsNotNull(ec);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == target));
            Assert.IsTrue(graph.Graph.Edges.Any(e => e == ec.Edge));
            Assert.AreEqual<VertexControl>(ec.Source, sourceVC, "source are not equal");
            Assert.AreEqual<VertexControl>(ec.Target, targetVC, "target are not equal");

            ceo.UnDo();

            Assert.IsTrue(undoCreateEdgeCalled);
            Assert.IsNotNull(ec);
            Assert.IsNull(ec.Edge);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == target));
            Assert.IsFalse(graph.Graph.Edges.Any(e => e == ec.Edge));
            Assert.AreEqual<VertexControl>(ec.Source, null, "source sould be null");
            Assert.AreEqual<VertexControl>(ec.Target, null, "target sould be null");

            cvoSource.UnDo();

            Assert.IsTrue(undoCreateSourceCalled);
            Assert.IsNotNull(sourceVC);
            Assert.IsFalse(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsNull(sourceVC.Vertex, "source sould be null");

            cvoTarget.UnDo();

            Assert.IsTrue(undoCreateTargetCalled);
            Assert.IsNotNull(targetVC);
            Assert.IsFalse(graph.Graph.Vertices.Any(v => v == target));
            Assert.IsNull(targetVC.Vertex, "target sould be null");
        }

        [TestMethod]
        public void Delete_Vertex_With_Related_Edges_Test()
        {
            //begin setup
            var graph = new Model.GraphArea();

            var v1 = new DataVertex(100) { };
            var v2 = new DataVertex(103) { };
            var v3 = new DataVertex(105) { };
            var vc1 = new VertexControl(v1);
            var vc2 = new VertexControl(v2);
            var vc3 = new VertexControl(v3);

            var e1 = new DataEdge(v1, v2);
            var ec1 = new EdgeControl(vc1, vc2, e1);
            var e2 = new DataEdge(v1, v3);
            var ec2 = new EdgeControl(vc1, vc3, e2);

            graph.Graph.AddVertex(v1);
            graph.AddVertex(v1, vc1);
            graph.Graph.AddVertex(v2);
            graph.AddVertex(v2, vc2);
            graph.Graph.AddVertex(v3);
            graph.AddVertex(v3, vc3);

            graph.Graph.AddEdge(e1);
            graph.AddEdge(e1, ec1);
            graph.Graph.AddEdge(e2);
            graph.AddEdge(e2, ec2);
            //end setup
            bool doCalled = false;
            bool undoCalled = false;
            var dvo = new DeleteVertexOperation(graph, v1, (dv, vc) =>
            {
                doCalled = true;
            },
            (dv) =>
            {
                undoCalled = true;
            });

            dvo.Do();

            Assert.IsTrue(doCalled);
            Assert.IsFalse(graph.VertexList.Any(v => v.Key.Id == v1.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v2.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v3.Id));
            Assert.IsFalse(graph.EdgesList.Any(e => e.Key == e1));
            Assert.IsFalse(graph.EdgesList.Any(e => e.Key == e2));

            dvo.UnDo();

            Assert.IsTrue(undoCalled);
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v1.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v2.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v3.Id));
            Assert.IsTrue(graph.EdgesList.Any(e => e.Key == e1));
            Assert.IsTrue(graph.EdgesList.Any(e => e.Key == e2));

            dvo.Do();

            Assert.IsTrue(doCalled);
            Assert.IsFalse(graph.VertexList.Any(v => v.Key.Id == v1.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v2.Id));
            Assert.IsTrue(graph.VertexList.Any(v => v.Key.Id == v3.Id));
            Assert.IsFalse(graph.EdgesList.Any(e => e.Key == e1));
            Assert.IsFalse(graph.EdgesList.Any(e => e.Key == e2));
        }

        [TestMethod]
        public void Vertex_AddPropertyOperation_Test()
        {
            var vertex = new DataVertex();

            var apo = new AddPropertyOperation(vertex);

            apo.Do();

            Assert.IsNotNull(apo.Property);
            Assert.AreEqual(vertex.Properties.Count, 1);

            apo.UnDo();

            Assert.AreEqual(vertex.Properties.Count, 0);
        }

        [TestMethod]
        public void Vertex_DeletePropertyOperation_Test()
        {
            var vertex = new DataVertex();
            var p1 = new Model.PropertyViewmodel(1, "p1", "p1", vertex);
            vertex.Properties.Add(p1);
            var p2 = new Model.PropertyViewmodel(2, "p2", "p2", vertex) { IsSelected = true };
            vertex.Properties.Add(p2);
            var p3 = new Model.PropertyViewmodel(3, "p3", "p3", vertex) { IsSelected = true };
            vertex.Properties.Add(p3);

            var dpo = new DeletePropertyOperation(vertex);

            dpo.Do();

            Assert.AreEqual(vertex.Properties.Count, 1);

            dpo.UnDo();

            Assert.AreEqual(vertex.Properties.Count, 3);
            Assert.AreEqual(vertex.Properties[0], p1);
            Assert.AreEqual(vertex.Properties[1], p2);
            Assert.AreEqual(vertex.Properties[2], p3);
        }

        [TestMethod]
        public void Vertex_Set_Of_DeleteAddOperatrion_Test()
        {
            var vertex = new DataVertex();

            var p1 = new Model.PropertyViewmodel(1, "p1", "p1", vertex);
            vertex.Properties.Add(p1);
            var p2 = new Model.PropertyViewmodel(2, "p2", "p2", vertex) { IsSelected = true };
            vertex.Properties.Add(p2);
            var p3 = new Model.PropertyViewmodel(3, "p3", "p3", vertex) { IsSelected = true };
            vertex.Properties.Add(p3);

            var apo = new AddPropertyOperation(vertex);
            var dpo = new DeletePropertyOperation(vertex);

            apo.Do();
            dpo.Do();

            Assert.AreEqual(vertex.Properties.Count, 2);
            Assert.AreEqual(vertex.Properties[0], p1);
            Assert.AreEqual(vertex.Properties[1], apo.Property);

            dpo.UnDo();
            apo.UnDo();

            Assert.AreEqual(vertex.Properties.Count, 3);
            Assert.AreEqual(vertex.Properties[0], p1);
            Assert.AreEqual(vertex.Properties[1], p2);
            Assert.AreEqual(vertex.Properties[2], p3);
        }

        [TestMethod]
        public void EditValuePropertyOperation_Test()
        {
            var vertex = new DataVertex();
            var property = new PropertyViewmodel(1, "key", "value", vertex);
            vertex.AddProperty(property);
            var evpo = new EditValuePropertyOperation(property, "value changed");

            evpo.Do();

            Assert.AreEqual(property.Value, "value changed");

            evpo.UnDo();

            Assert.AreEqual(property.Value, "value");
        }

        [TestMethod]
        public void EditKeyPropertyOperation_Test()
        {
            var vertex = new DataVertex();
            var property = new PropertyViewmodel(1, "key", "value", vertex);
            vertex.AddProperty(property);
            var ekpo = new EditKeyPropertyOperation(property, "key changed");

            ekpo.Do();

            Assert.AreEqual(property.Key, "key changed");

            ekpo.UnDo();

            Assert.AreEqual(property.Key, "key");
        }

        [TestMethod]
        public void VertexPositionChanged_Test()
        {
            var graph = new Model.GraphArea();

            var vertex = new DataVertex();

            var vctrol = new VertexControl(vertex);

            graph.Graph.AddVertex(vertex);

            graph.AddVertex(vertex, vctrol);

            bool called = false;
            bool undoCalled = false;

            var vpco = new VertexPositionChangeOperation(graph, vctrol, 10, 10, vertex, (v, vc) =>
            {
                called = true;
            }, (vc) =>
            {
                undoCalled = true;
            });

            vpco.Do();

            Assert.IsTrue(called);

            vpco.UnDo();

            Assert.IsTrue(undoCalled);
        }
    }
}
