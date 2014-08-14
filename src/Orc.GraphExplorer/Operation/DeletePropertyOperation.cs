using Orc.GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class DeletePropertyOperation :PropertyOperation
    {
        IEnumerable<PropertyViewmodel> _list;

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
