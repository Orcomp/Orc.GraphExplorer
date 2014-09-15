namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Linq;

    using GraphX;
    using Models;

    using Orc.GraphExplorer.ObjectModel;

    public class DeleteEdgeOperation : EdgeOperation
    {
        public DeleteEdgeOperation(EditorData editor, GraphArea area, DataVertex source, DataVertex target,DataEdge edge, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null)
            : base(editor, area,  source, target, callback, undoCallback)
        {
            _Edge = edge;
            base.Sammary = "Delete Edge";
        }

        public override void Do()
        {            
            RemoveEdge(_Edge);

            if (_callback != null)
            {
                _callback.Invoke(_eCtrl);
            }
        }

        public override void UnDo()
        {
            _Edge = new DataEdge(_source, _target);

            _eCtrl = AddEdge(_Edge);

            HighlightBehaviour.SetIsHighlightEnabled(_eCtrl, false);

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_eCtrl);
            }
        }
    }
}
