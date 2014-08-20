namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using DomainModel;
    using GraphX;
    using Models;

    public class CreateVertexOperation : VertexOperation
    {
        double _x;
        double _y;
        const double GapX = 150;
        const double GapY = 150;
        const string CreateVertex = "Create Vertex";
        public override string Sammary
        {
            get { return CreateVertex; }
        }

        public override void Do()
        {
            _vCtrl = new VertexControl(_vertex);
            AddVertex(_vertex, _vCtrl);
            //ArrangeVertexPosition();

            if (_x != double.MinValue && _y != double.MinValue)
            {
                SetPositon();
            }

            if (_callback != null)
            {
                _callback.Invoke(_vertex, _vCtrl);
            }

        }

        private void SetPositon()
        {
            var app = Application.Current;

            if (app != null)
            {
                RunCodeInUiThread(() =>
                {
                    if (_x != double.MinValue)
                    { GraphArea.SetX(_vCtrl, _x, true); }
                    if (_y != double.MinValue)
                    { GraphArea.SetY(_vCtrl, _y, true); }
                }, app.Dispatcher, DispatcherPriority.Loaded);
            }
            else
            {
                if (_x != double.MinValue)
                { GraphArea.SetX(_vCtrl, _x, true); }
                if (_y != double.MinValue)
                { GraphArea.SetY(_vCtrl, _y, true); }
            }
        }

        //private void ArrangeVertexPosition()
        //{
        //    double maxY
        //    //throw new NotImplementedException();
        //}

        public override void UnDo()
        {            
            RemoveVertex(_vertex);

            if (_undoCallback != null)
            {
                _undoCallback.Invoke(_vertex);
            }
        }

        public CreateVertexOperation(GraphArea area, DataVertex data = null, double x = double.MinValue, double y = double.MinValue, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
            : base(area, data, callback, undoCallback)
        {
            _vCtrl = new VertexControl(_vertex);

            if (x != double.MinValue)
                _x = x;

            if (y != double.MinValue)
                _y = y;
        }
    }
}
