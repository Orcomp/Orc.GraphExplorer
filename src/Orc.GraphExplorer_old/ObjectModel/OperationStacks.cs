namespace Orc.GraphExplorer.ObjectModel
{
    using System.Collections.Generic;

    using Orc.GraphExplorer.Operations.Interfaces;

    public class OperationStacks
    {
        public OperationStacks()
        {
            ForRedo = new Stack<IOperation>();
            ForUndo = new Stack<IOperation>();
        }
        public Stack<IOperation> ForUndo { get; private set; }

        public Stack<IOperation> ForRedo { get; private set; }
    }
}