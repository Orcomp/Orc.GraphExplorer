using GraphX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    using Models;

    public class DeleteVertexOperation : VertexOperation
    {
        List<Tuple<DataEdge, DataVertex, DataVertex>> _relatedEdges = new List<Tuple<DataEdge, DataVertex, DataVertex>>();
        const string DeleteVertex = "Delete Vertex";
        public override string Sammary
        {
            get { return DeleteVertex; }
        }

        public override void Do()
        {
            _relatedEdges.Clear();
            foreach (var item in _graph.GetRelatedControls(_vCtrl, GraphControlType.Edge, EdgesType.All))
            {
                var ec = item as EdgeControl;

                _relatedEdges.Add(new Tuple<DataEdge, DataVertex, DataVertex>(ec.Edge as DataEdge, ec.Source.Vertex as DataVertex, ec.Target.Vertex as DataVertex));

                _graph.Graph.RemoveEdge(ec.Edge as DataEdge);
                _graph.RemoveEdge(ec.Edge as DataEdge);
            }

            _graph.Graph.RemoveVertex(_vertex);
            _graph.RemoveVertex(_vertex);

            if (_callback != null)
            {
                _callback.Invoke(_vertex,_vCtrl);
            }
        }

        public override void UnDo()
        {
            _graph.Graph.AddVertex(_vertex);
            _vCtrl = new VertexControl(_vertex);
            _graph.AddVertex(_vertex, _vCtrl);

            HighlightBehaviour.SetIsHighlightEnabled(_vCtrl, false);

            foreach (var edge in _relatedEdges)
            {

                var source = _graph.VertexList.FirstOrDefault(v=>v.Key.Id == edge.Item2.Id);
                var target = _graph.VertexList.FirstOrDefault(v=>v.Key.Id == edge.Item3.Id);

                if(source.Value==null||target.Value==null)
                    throw new Exception("source or target vertex not found");

                var edgeCtrl = new EdgeControl(source.Value, target.Value, edge.Item1)
                {
                    ShowArrows = true,
                    ShowLabel = true
                };

                _graph.Graph.AddEdge(edge.Item1);
                _graph.AddEdge(edge.Item1, edgeCtrl);

                HighlightBehaviour.SetIsHighlightEnabled(edgeCtrl, false);
            }

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_vertex);
            }
        }

        public DeleteVertexOperation(GraphArea graph, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
            : base(graph, data, callback, undoCallback)
        {
            if (graph.VertexList.ContainsKey(_vertex))
                _vCtrl = graph.VertexList[_vertex];
            else
            {
                //throw new ArgumentNullException("Vertex Control");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _relatedEdges.Clear();
            _relatedEdges = null;
        }
    }
}
