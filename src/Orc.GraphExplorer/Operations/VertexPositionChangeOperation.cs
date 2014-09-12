namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Windows;

    using Enums;
    using GraphX;
    using Models;

    using Orc.GraphExplorer.ObjectModel;

    public class VertexPositionChangeOperation : VertexOperation
    {
        double _offsetX;
        double _offsetY;
        VertexControl _vc;
        public VertexPositionChangeOperation(GraphArea area, VertexControl vc, double offsetX, double offsetY, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
            : base(area, data, callback, undoCallback)
        {
            _vc = vc;
            _offsetX = offsetX;
            _offsetY = offsetY;

            OperationStatus = OperationStatus.Done;
        }

        public override void Do()
        {
            if (OperationStatus != OperationStatus.Done)
            {
                UpdateCoordinates(_vc, _offsetX, _offsetY);
                OperationStatus = OperationStatus.Redo;
            }

            if (_callback != null)
            {
                _callback.Invoke(_vertex, _vCtrl);
            }
        }

        public override void UnDo()
        {
            UpdateCoordinates(_vc, -_offsetX, -_offsetY);
            OperationStatus = OperationStatus.Undo;

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_vertex);
            }
        }

        private static void UpdateCoordinates(DependencyObject obj, double horizontalChange, double verticalChange)
        {
            if (double.IsNaN(GraphAreaBase.GetX(obj)))
                GraphAreaBase.SetX(obj, 0, true);
            if (double.IsNaN(GraphAreaBase.GetY(obj)))
                GraphAreaBase.SetY(obj, 0, true);

            //move the object
            var x = GraphAreaBase.GetX(obj) + horizontalChange;
            GraphAreaBase.SetX(obj, x, true);
            var y = GraphAreaBase.GetY(obj) + verticalChange;
            GraphAreaBase.SetY(obj, y, true);
        }
    }
}
