namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GraphX;
    using Models;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Views;

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
            foreach (var item in GetRelatedControls(_vCtrl, GraphControlType.Edge, EdgesType.All))
            {
                var ec = item as EdgeControl;

                _relatedEdges.Add(new Tuple<DataEdge, DataVertex, DataVertex>(ec.Edge as DataEdge, ec.Source.Vertex as DataVertex, ec.Target.Vertex as DataVertex));
                
                RemoveEdge(ec.Edge as DataEdge);
            }

            RemoveVertex(_vertex);

            if (_callback != null)
            {
                _callback.Invoke(_vertex,_vCtrl);
            }
        }

        public override void UnDo()
        {
            _vCtrl = AddVertex(_vertex);
           // AddVertex(_vertex, _vCtrl);

            HighlightBehaviour.SetIsHighlightEnabled(_vCtrl, false);

            foreach (var edge in _relatedEdges)
            {
                var edgeCtrl = AddEdge(edge.Item1);

                HighlightBehaviour.SetIsHighlightEnabled(edgeCtrl, false);
            }

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_vertex);
            }
        }

        public DeleteVertexOperation(EditorData editor, GraphArea area, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
            : base(editor, area, data, callback, undoCallback)
        {
            if (area.VertexList.ContainsKey(_vertex))
                _vCtrl = area.VertexList[_vertex] as VertexControl;
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
