using GraphX;
using Orc.GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Orc.GraphExplorer
{
    public class VertexPositionChangeOperation : VertexOperation
    {
        double _offsetX;
        double _offsetY;
        VertexControl _vc;
        public VertexPositionChangeOperation(GraphArea graph, VertexControl vc, double offsetX, double offsetY, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
            : base(graph, data, callback, undoCallback)
        {
            _vc = vc;
            _offsetX = offsetX;
            _offsetY = offsetY;

            Status = Orc.GraphExplorer.Status.Done;
        }

        public override void Do()
        {
            if (Status != Orc.GraphExplorer.Status.Done)
            {
                UpdateCoordinates(_vc, _offsetX, _offsetY);
                Status = Orc.GraphExplorer.Status.Redo;
            }

            if (_callback != null)
            {
                _callback.Invoke(_vertex, _vCtrl);
            }
        }

        public override void UnDo()
        {
            UpdateCoordinates(_vc, -_offsetX, -_offsetY);
            Status = Orc.GraphExplorer.Status.Undo;

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
