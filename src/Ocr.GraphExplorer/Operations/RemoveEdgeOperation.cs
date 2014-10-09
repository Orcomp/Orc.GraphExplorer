#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveEdgeOperation.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Operations
{
    using Catel.Memento;
    using Interfaces;
    using Models;

    public class RemoveEdgeOperation : IOperation
    {
        private readonly GraphArea _graphArea;
        private readonly DataEdge _edge;

        public RemoveEdgeOperation(GraphArea graphArea, DataEdge edge)
        {
            _graphArea = graphArea;
            _edge = edge;

            Target = _graphArea;
            CanRedo = true;
        }

        public void Undo()
        {
            _graphArea.Logic.Graph.AddEdge(_edge);
        }

        public void Redo()
        {
            Do();
        }

        public void Do()
        {
            _graphArea.Logic.Graph.RemoveEdge(_edge);
        }

        public object Target { get; private set; }
        public string Description { get; set; }
        public object Tag { get; set; }
        public bool CanRedo { get; private set; }
    }
}