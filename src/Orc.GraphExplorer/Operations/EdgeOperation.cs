namespace Orc.GraphExplorer.Operations
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Enums;
    using GraphX;
    using Models;
    using Operations.Interfaces;
    using QuickGraph;

    /// <summary>
    /// base class for encapsulating edge management
    /// </summary>
    public abstract class EdgeOperation : IOperation
    {
        string Edge = "Operate Edge";
        public virtual string Sammary
        {
            get { return Edge; }
            protected set { Edge = value; }
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

        protected DataEdge _Edge;
        protected EdgeControl _eCtrl;
        protected VertexControl _sourceVC;
        protected VertexControl _targetVC;
        protected DataVertex _source;
        protected DataVertex _target;
        protected Action<EdgeControl> _callback;
        protected Action<EdgeControl> _undoCallback;

        private readonly GraphArea _area;

        private readonly IGXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>> _logic;

        public EdgeOperation(GraphArea area, DataVertex source, DataVertex target, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null)
        {
            _area = area;
            _logic = _area.LogicCore;
            _callback = callback;
            _undoCallback = undoCallback;
            _source = source;
            _target = target;
        }

        protected void RemoveEdge(DataEdge dataEdge)
        {
            _logic.Graph.RemoveEdge(dataEdge);
            _area.RemoveEdge(dataEdge);
        }

        protected void AddEdge(DataEdge dataEdge, EdgeControl edgeControl)
        {
            _logic.Graph.AddEdge(dataEdge);
            _area.AddEdge(dataEdge, edgeControl);
        }

        protected IDictionary<DataVertex, VertexControl> VertexList 
        {
            get
            {
                return _area.VertexList;
            }
        }

        //dispose operation, release reference
        public virtual void Dispose()
        {
            _Edge.Dispose();
            _Edge = null;
            _eCtrl = null;
            _callback = null;
            _undoCallback = null;
        }

        public OperationStatus OperationStatus
        {
            get;
            protected set;
        }
    }
}
