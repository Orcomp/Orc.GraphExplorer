using GraphX;
using Orc.GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class CreateEdgeOperation : EdgeOperation
    {
        public const string CreateEdge = "Create Edge";
        public override string Sammary
        {
            get { return CreateEdge; }
        }

        public override void Do()
        {
            _Edge = new DataEdge(_source, _target);

            _sourceVC = _graph.VertexList.Where(pair => pair.Key.Id == _source.Id).Select(pair => pair.Value).FirstOrDefault();
            _targetVC = _graph.VertexList.Where(pair => pair.Key.Id == _target.Id).Select(pair => pair.Value).FirstOrDefault();

            if (_sourceVC == null || _targetVC == null)
                throw new ArgumentNullException("Vertex Control");

            _eCtrl = new EdgeControl(_sourceVC, _targetVC, _Edge)
            {
                ShowArrows = true,
                ShowLabel = true
            };

            _graph.Graph.AddEdge(_Edge);
            _graph.AddEdge(_Edge, _eCtrl);

            if (_callback != null)
            {
                _callback.Invoke(_eCtrl);
            }
        }

        public override void UnDo()
        {
            _graph.Graph.RemoveEdge(_Edge);
            _graph.RemoveEdge(_Edge);

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_eCtrl);
            }
        }

        public CreateEdgeOperation(GraphArea graph, DataVertex source, DataVertex target, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null):base(graph,source,target,callback,undoCallback)
        {

        }
    }
}
