using GraphX;
using System;

namespace Orc.GraphExplorer
{
    using Models;

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
        protected GraphArea _graph;

        public EdgeOperation(GraphArea graph, DataVertex source, DataVertex target, Action<EdgeControl> callback = null, Action<EdgeControl> undoCallback = null)
        {
            _graph = graph;
            _callback = callback;
            _undoCallback = undoCallback;
            _source = source;
            _target = target;
        }

        //dispose operation, release reference
        public virtual void Dispose()
        {
            _Edge.Dispose();
            _Edge = null;
            _eCtrl = null;
            _callback = null;
            _undoCallback = null;
            _graph = null;
        }

        public Status Status
        {
            get;
            protected set;
        }
    }
}
