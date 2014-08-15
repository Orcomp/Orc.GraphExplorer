using GraphX;
using Orc.GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Orc.GraphExplorer
{
    using QuickGraph;

    /// <summary>
    /// base class for encapsulating vertex management
    /// </summary>
    public abstract class VertexOperation : IOperation
    {
        public virtual string Sammary
        {
            get;
            protected set;
        }

        ///Summary
        ///    perform an operation
        public abstract void Do();

        ///Summary
        ///    undo an operation
        public abstract void UnDo();

        bool _isUnDoable = true;
        public virtual bool IsUnDoable
        {
            get
            {
                return _isUnDoable;
            }
            protected set
            {
                _isUnDoable = value;
            }
        }

        protected DataVertex _vertex;
        protected VertexControl _vCtrl;
        protected Action<DataVertex, VertexControl> _callback;
        protected Action<DataVertex> _undoCallback;

        private readonly GraphArea _area;

        private readonly GraphLogic _logic;

        public VertexOperation(GraphArea area, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
        {
            _area = area;
            _logic = _area.Logic;
            _callback = callback;
            _undoCallback = undoCallback;

            if (data != null)
                _vertex = data;
            else
                _vertex = DataVertex.Create();

            Status = Orc.GraphExplorer.Status.Init;
        }

        protected BidirectionalGraph<DataVertex, DataEdge> Graph
        {
            get
            {
                return _logic.Graph;
            }
        }

        protected void AddEdge(DataEdge dataEdge, EdgeControl edgeControl)
        {
            _logic.Graph.AddEdge(dataEdge);
            _area.AddEdge(dataEdge, edgeControl);
        }

        protected void RemoveEdge(DataEdge dataEdge)
        {
            _logic.Graph.RemoveEdge(dataEdge);
            _area.RemoveEdge(dataEdge);
        }

        protected void AddVertex(DataVertex dataVertex, VertexControl vertexControl)
        {
            _logic.Graph.AddVertex(dataVertex);
            _area.AddVertex(dataVertex, vertexControl);
        }

        protected void RemoveVertex(DataVertex dataVertex)
        {
            _logic.Graph.RemoveVertex(dataVertex);
            _area.RemoveVertex(dataVertex);
        }

        protected List<IGraphControl> GetRelatedControls(IGraphControl ctrl, GraphControlType controlType, EdgesType edgesType)
        {
            return _area.GetRelatedControls(ctrl, controlType, edgesType);
        }

        protected IDictionary<DataVertex, VertexControl> VertexList
        {
            get
            {
                return _area.VertexList;
            }
        }

        public Status Status
        {
            get;
            protected set;
        }

        //dispose operation, release reference
        public virtual void Dispose()
        {
            _vertex.Dispose();
            _vertex = null;
            _vCtrl = null;
            _callback = null;
            _undoCallback = null;
         //   _area = null;
        }


        public static void RunCodeInUiThread<T>(Action<T> action, T parameter, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority, parameter);
            }
            else
            {
                action.Invoke(parameter);
            }
        }

        public static void RunCodeInUiThread(Action action, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher == null && Application.Current != null)
                dispatcher = Application.Current.Dispatcher;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority);
            }
            else
            {
                action.Invoke();
            }
        }

    }
}
