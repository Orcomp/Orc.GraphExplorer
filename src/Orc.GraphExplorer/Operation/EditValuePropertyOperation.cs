using Orc.GraphExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class EditValuePropertyOperation : PropertyOperation
    {
        string _value;
        Tuple<string, string> _tuple;
        public EditValuePropertyOperation(PropertyViewmodel property = null, string value = "")
            : base(null, property)
        {
            base.Sammary = "Edit Property Value";
            _value = value;
        }

        public override void Do()
        {
            _tuple = Property.UpdateValue(_value);
        }

        public override void UnDo()
        {
            _tuple = Property.UpdateValue(_tuple.Item1);
        }
    }
}
