﻿using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class EditKeyPropertyOperation : PropertyOperation
    {
        string _key;
        Tuple<string, string> _tuple;
        public EditKeyPropertyOperation(PropertyViewmodel property = null,string key = "")
            : base(null, property)
        {
            base.Sammary = "Edit Property Key";
            _key = key;
        }

        public override void Do()
        {
            _tuple = Property.UpdateKey(_key);
        }

        public override void UnDo()
        {
            _tuple = Property.UpdateKey(_tuple.Item1);
        }
    }
}
