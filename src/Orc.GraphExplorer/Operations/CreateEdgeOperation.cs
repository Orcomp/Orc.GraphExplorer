namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Linq;

    using GraphX;
    using GraphX.GraphSharp;
    using Models;

    using Orc.GraphExplorer.ObjectModel;

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

            _sourceVC = VertexList.Where(pair => pair.Key.Id == _source.Id).Select(pair => pair.Value).FirstOrDefault();
            _targetVC = VertexList.Where(pair => pair.Key.Id == _target.Id).Select(pair => pair.Value).FirstOrDefault();

            if (_sourceVC == null || _targetVC == null)
                throw new ArgumentNullException("Vertex Control");

            _eCtrl = new EdgeControl(_sourceVC, _targetVC, _Edge)
            {
                ShowArrows = true,
                ShowLabel = true
            };

            AddEdge(_Edge, _eCtrl);

            if (_callback != null)
            {
                _callback.Invoke(_eCtrl);
            }
        }

        public override void UnDo()
        {
            RemoveEdge(_Edge);

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_eCtrl);
            }
        }

        public CreateEdgeOperation(GraphArea area, DataVertex source, DataVertex target, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null):base(area, source,target,callback,undoCallback)
        {

        }
    }
}
