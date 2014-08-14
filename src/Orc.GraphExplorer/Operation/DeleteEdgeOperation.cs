using GraphX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    using Models;

    public class DeleteEdgeOperation : EdgeOperation
    {
        public DeleteEdgeOperation(GraphArea graph, DataVertex source, DataVertex target,DataEdge edge, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null)
            : base(graph, source, target, callback, undoCallback)
        {
            _Edge = edge;
            base.Sammary = "Delete Edge";
        }

        public override void Do()
        {
            _graph.Graph.RemoveEdge(_Edge);
            _graph.RemoveEdge(_Edge);

            if (_callback != null)
            {
                _callback.Invoke(_eCtrl);
            }
        }

        public override void UnDo()
        {
            _Edge = new DataEdge(_source, _target);

            _sourceVC = _graph.VertexList.Where(pair => pair.Key == _source).Select(pair => pair.Value).FirstOrDefault();
            _targetVC = _graph.VertexList.Where(pair => pair.Key == _target).Select(pair => pair.Value).FirstOrDefault();

            if (_sourceVC == null || _targetVC == null)
                throw new ArgumentNullException("VertexControl");

            _eCtrl = new EdgeControl(_sourceVC, _targetVC, _Edge)
            {
                ShowArrows = true,
                ShowLabel = true
            };

            _graph.Graph.AddEdge(_Edge);
            _graph.AddEdge(_Edge, _eCtrl);

            HighlightBehaviour.SetIsHighlightEnabled(_eCtrl, false);

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_eCtrl);
            }
        }
    }
}
