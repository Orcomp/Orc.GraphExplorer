using GraphX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.GraphExplorer.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests
{
    using Operations;
    using ViewModels;

    [TestClass]
    public class GraphExplorerViewmodelTests
    {
        [TestMethod]
        public void Constructor_Test()
        {
            var graphVM = new GraphExplorerViewModel();

            Assert.IsNotNull(graphVM.Operations);
            Assert.IsNotNull(graphVM.OperationsRedo);

            Assert.IsFalse(graphVM.HasChange);
            Assert.IsFalse(graphVM.HasRedoable);
            Assert.IsFalse(graphVM.HasUndoable);
        }

        [TestMethod]
        public void Do_CreateVertex_Operation_Test()
        {
            var graphVM = new GraphExplorerViewModel();

            Assert.IsNotNull(graphVM.Operations);
            Assert.IsNotNull(graphVM.OperationsRedo);

            Assert.IsFalse(graphVM.HasChange);
            Assert.IsFalse(graphVM.HasRedoable);
            Assert.IsFalse(graphVM.HasUndoable);

            bool createSourceCalled = false;
            bool undoCreateSourceCalled = false;
            var graph = new Model.GraphArea();
            var source = new DataVertex(100) { Title = "Test" };
            VertexControl sourceVC = null;

            var cvoSource = new CreateVertexOperation(graph, source, callback:(sv, svc) =>
            {
                createSourceCalled = true;
                sourceVC = svc;
            },
            undoCallback:(v) =>
            {
                 undoCreateSourceCalled = true;
            });

            graphVM.Do(cvoSource);

            Assert.IsTrue(createSourceCalled);
            Assert.IsNotNull(sourceVC);
            Assert.IsTrue(graph.Graph.Vertices.Any(v => v == source));
            Assert.AreEqual(source, sourceVC.Vertex, "source are not equal");

            Assert.IsTrue(graphVM.HasChange);
            //Assert.IsTrue(graphVM.HasRedoable);
            Assert.IsTrue(graphVM.HasUndoable);

            graphVM.UndoCommand.Execute();

            Assert.IsTrue(undoCreateSourceCalled);
            Assert.IsNotNull(sourceVC);
            Assert.IsFalse(graph.Graph.Vertices.Any(v => v == source));
            Assert.IsNull(sourceVC.Vertex, "source sould be null");

            Assert.IsTrue(graphVM.HasChange);
            Assert.IsTrue(graphVM.HasRedoable);
            //Assert.IsTrue(graphVM.HasUndoable);
        }

        [TestMethod]
        public void Commit_Operation_Test()
        {
            var op1 = new MockOperation();
            var op2 = new MockOperation();

            var vm = new GraphExplorerViewModel();

            vm.Do(op1);

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vm.Operations.Count, 1);
            Assert.IsTrue(op1.DoCalled);

            vm.Do(op2);

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vm.Operations.Count, 2);
            Assert.IsTrue(op2.DoCalled);

            vm.Commit();

            Assert.IsFalse(vm.HasUndoable);
            Assert.AreEqual(vm.Operations.Count, 0);
        }

        [TestMethod]
        public void Observe_Vertex_AddPropertyOperation_Test()
        {
            var vertex = new DataVertex();

            var vm = new GraphExplorerViewModel();

            vm.OnVertexLoaded(new DataVertex[] { vertex }, true);

            vertex.AddCommand.Execute();

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vertex.Properties.Count, 1);

            vm.UndoCommand.Execute();

            Assert.IsTrue(vm.HasRedoable);
            Assert.AreEqual(vertex.Properties.Count, 0);

            vm.RedoCommand.Execute();

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vertex.Properties.Count, 1);
        }

        [TestMethod]
        public void Observe_Vertex_DeletePropertyOperation_Test()
        {
            var vertex = new DataVertex();

            vertex.AddProperty(new Model.PropertyViewmodel(0, "", "", vertex) { IsSelected = true});

            var vm = new GraphExplorerViewModel();

            vm.OnVertexLoaded(new DataVertex[] { vertex }, true);

            vertex.DeleteCommand.Execute();

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vertex.Properties.Count, 0);

            vm.UndoCommand.Execute();

            Assert.IsTrue(vm.HasRedoable);
            Assert.AreEqual(vertex.Properties.Count, 1);

            vm.RedoCommand.Execute();

            Assert.IsTrue(vm.HasUndoable);
            Assert.AreEqual(vertex.Properties.Count, 0);
        }
    }
}
