namespace Orc.GraphExplorer.Models
{
    using System;

    public class VertexObserverable : IDisposable
    {
        #region Fields
        private readonly Guid _observerId;

        private DataVertex _vertex;
        #endregion

        #region Constructors
        public VertexObserverable(DataVertex vertex, Guid observerId)
        {
            _vertex = vertex;
            _observerId = observerId;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _vertex.RemoveObserver(_observerId);
            _vertex = null;
        }
        #endregion
    }
}