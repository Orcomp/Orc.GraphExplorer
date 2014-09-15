namespace Orc.GraphExplorer.Operations
{
    using System;
    using Models;
    using ViewModels;

    public class EditValuePropertyOperation : PropertyOperation
    {
        string _value;
        Tuple<string, string> _tuple;
        public EditValuePropertyOperation(EditorData editor, PropertyViewModel property = null, string value = "")
            : base(editor, null, property)
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
