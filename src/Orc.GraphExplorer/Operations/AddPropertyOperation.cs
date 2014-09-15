namespace Orc.GraphExplorer.Operations
{
    using Models;

    using Orc.GraphExplorer.ObjectModel;

    using ViewModels;

    public class AddPropertyOperation : PropertyOperation
    {
        public AddPropertyOperation(EditorData editor, DataVertex vertex, PropertyViewModel property = null)
            : base(editor, vertex, property)
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
