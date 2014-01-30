using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public abstract class PropertyOperation : IOperation
    {
        private PropertyViewmodel _property;

        public PropertyViewmodel Property
        {
            get { return _property; }
            protected set { _property = value; }
        }

        protected DataVertex _vertex;
        public PropertyOperation(DataVertex vertex,PropertyViewmodel property)
        {
            _property = property;
            _vertex = vertex;
        }

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

        public Status Status
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
