namespace Orc.GraphExplorer.Operations
{
    using Enums;
    using Models;
    using Operations.Interfaces;

    using Orc.GraphExplorer.ObjectModel;

    using ViewModels;

    public abstract class PropertyOperation : IOperation
    {
        private PropertyViewModel _property;

        public PropertyViewModel Property
        {
            get { return _property; }
            protected set { _property = value; }
        }

        protected DataVertex _vertex;
        public PropertyOperation(Editor editor, DataVertex vertex,PropertyViewModel property)
        {
            Editor = editor;
            _property = property;
            _vertex = vertex;
        }

        public Editor Editor { get; private set; }

        public virtual string Sammary
        {
            get;
            protected set;
        }

        public abstract void Do();

        public abstract void UnDo();

        public virtual bool IsUnDoable
        {
            get { return true; }
        }

        public OperationStatus OperationStatus
        {
            get;
            protected set;
        }

        public virtual void Dispose()
        {
            _property = null;
            _vertex = null;
        }
    }
}
