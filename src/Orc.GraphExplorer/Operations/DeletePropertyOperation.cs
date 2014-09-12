namespace Orc.GraphExplorer.Operations
{
    using System.Collections.Generic;

    using Models;

    using Orc.GraphExplorer.ObjectModel;

    using ViewModels;

    public class DeletePropertyOperation :PropertyOperation
    {
        IEnumerable<PropertyViewModel> _list;

        public DeletePropertyOperation(DataVertex vertex)
            : base(vertex, null)
        {
            base.Sammary = "Delete Property";
        }

        public override void Do()
        {
            _list = _vertex.RemoveSelectedProperties();
        }

        public override void UnDo()
        {
            _vertex.AddPropertyRange(_list);
        }

        public override void Dispose()
        {
            base.Dispose();
            _list = null;
        }
    }
}
