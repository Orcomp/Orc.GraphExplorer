using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class AddPropertyOperation : PropertyOperation
    {
        public AddPropertyOperation(DataVertex vertex, PropertyViewmodel property = null)
            : base(vertex, property)
        {
            base.Sammary = "Add Property";
        }

        public override void Do()
        {
            Property = _vertex.AddProperty(Property);
        }

        public override void UnDo()
        {
            Property = _vertex.RemoveProperty(Property);
        }
    }
}
