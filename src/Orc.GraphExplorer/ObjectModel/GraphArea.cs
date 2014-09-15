namespace Orc.GraphExplorer.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Catel.IoC;
    using Events;
    using GraphX;
    using GraphX.Controls.Models;

    public class GraphArea : GraphArea<DataVertex, DataEdge, Graph>
    {
        public GraphArea()
        {
            var serviceLocator = ServiceLocator.Default;
            ControlFactory = serviceLocator.ResolveType<IGraphControlFactory>();
            ControlFactory.FactoryRootArea = this;
        }

        public override VertexControl[] GetAllVertexControls()
        {
            return base.GetAllVertexControls();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property != LogicCoreProperty)
            {
                return;
            }

            var oldLogic = e.OldValue as GraphLogic;
            if (oldLogic != null)
            {
                UnSubscribeOnGraphEvents(oldLogic);
            }
            SubscribeOnGraphEvents();
        }

        public void CreateGraphArea(IEnumerable<DataVertex> vertexes, IEnumerable<DataEdge> edges, double offsetY)
        {
            if (LogicCore.Graph != null)
            {
                UnSubscribeOnGraphEvents();
            }

            ClearLayout();

            var graph = new Graph();

            graph.AddVertexRange(vertexes);

            graph.AddEdgeRange(edges);

            ((GraphLogic)LogicCore).ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, Graph>(graph, 1.5, offsetY: offsetY);

            GenerateGraph(graph, true);

            SubscribeOnGraphEvents();
        }

        private void SubscribeOnGraphEvents()
        {
            if (LogicCore == null)
            {
                return;
            }
            LogicCore.Graph.VertexAdded += GraphVertexAdded;
            LogicCore.Graph.VertexRemoved += GraphVertexRemoved;
            LogicCore.Graph.EdgeAdded += GraphEdgeAdded;
            LogicCore.Graph.EdgeRemoved += GraphEdgeRemoved;
        }
        
        private void UnSubscribeOnGraphEvents(GraphLogic logic = null)
        {
            if (logic == null)
            {
                logic = LogicCore as GraphLogic;
            }

            if (logic == null)
            {
                return;
            }

            logic.Graph.VertexAdded -= GraphVertexAdded;
            logic.Graph.VertexRemoved -= GraphVertexRemoved;
            logic.Graph.EdgeAdded -= GraphEdgeAdded;
            logic.Graph.EdgeRemoved -= GraphEdgeRemoved;
        }

        void GraphEdgeRemoved(DataEdge e)
        {
            RemoveEdge(e);
        }

        private void GraphEdgeAdded(DataEdge e)
        {
            if (EdgesList.ContainsKey(e))
            {
                return;
            }
            var source = VertexList[e.Source];
            var target = e.Target.Id == -1 ? null : VertexList[e.Target];
            var edgeControl = ControlFactory.CreateEdgeControl(source, target, e);
            edgeControl.ShowArrows = true;
            edgeControl.ShowLabel = true;
            edgeControl.ManualDrawing = target == null;
            AddEdge(e, edgeControl);
            if (target == null && TemporaryEdgeCreated != null)
            {
                TemporaryEdgeCreated(this, new EdgeControlCreatedAventArgs(edgeControl));
            }
        }

        void GraphVertexRemoved(DataVertex vertex)
        {
            RemoveVertex(vertex);
        }

        void GraphVertexAdded(DataVertex vertex)
        {
            if (vertex.ID == -666)
            {
                return;
            }
            var vertexControl = ControlFactory.CreateVertexControl(vertex);
            AddVertex(vertex, vertexControl);
        }

        public event EventHandler<EdgeControlCreatedAventArgs> TemporaryEdgeCreated;
    }

}
