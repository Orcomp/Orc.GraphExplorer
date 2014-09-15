namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;

    using Enums;
    using GraphX;
    using Models;
    using Operations.Interfaces;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Views;

    using QuickGraph;

    /// <summary>
    /// base class for encapsulating vertex management
    /// </summary>
    public abstract class VertexOperation : IOperation
    {
        public EditorData Editor { get; private set; }

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

        private readonly IGXLogicCore<DataVertex, DataEdge, Graph> _logic;

        public VertexOperation(EditorData editor, GraphArea area, DataVertex data = null, Action<DataVertex, VertexControl> callback = null, Action<DataVertex> undoCallback = null)
        {
            Editor = editor;
            _area = area;
            _logic = _area.LogicCore;
            _callback = callback;
            _undoCallback = undoCallback;

            if (data != null)
                _vertex = data;
            else
                _vertex = DataVertex.Create();

            OperationStatus = OperationStatus.Init;
        }

        protected Graph Graph
        {
            get
            {
                return _logic.Graph;
            }
        }

        protected EdgeControl AddEdge(DataEdge dataEdge)
        {
            _logic.Graph.AddEdge(dataEdge);
            return _area.EdgesList[dataEdge];
        }

        protected void RemoveEdge(DataEdge dataEdge)
        {
            _logic.Graph.RemoveEdge(dataEdge);            
        }

        protected VertexControl AddVertex(DataVertex dataVertex/*, VertexControl vertexControl*/)
        {
            _logic.Graph.AddVertex(dataVertex);
            return _area.VertexList[dataVertex];
        }

        protected void RemoveVertex(DataVertex dataVertex)
        {
            _logic.Graph.RemoveVertex(dataVertex);
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

        public OperationStatus OperationStatus
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
