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
            
            _eCtrl = AddEdge(_Edge);

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

        public CreateEdgeOperation(Editor editor, GraphArea area, DataVertex source, DataVertex target, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null)
            :base(editor, area, source,target,callback,undoCallback)
        {

        }
    }
}
