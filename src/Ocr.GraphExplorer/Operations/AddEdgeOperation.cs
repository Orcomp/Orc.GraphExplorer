#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="AddEdgeOperation.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Operations
{
    using Catel.Memento;

    using Orc.GraphExplorer.Models;

    public class AddEdgeOperation : IMementoSupport
    {
        private readonly GraphArea _graphArea;

        private readonly DataEdge _edge;

        public AddEdgeOperation(GraphArea graphArea, DataEdge edge)
        {
            _graphArea = graphArea;
            _edge = edge;
            CanRedo = true;
        }

        public void Undo()
        {
            _graphArea.Logic.Graph.RemoveEdge(_edge);
        }

        public void Redo()
        {
            Do();
        }

        public object Target { get; private set; }

        public string Description { get; set; }

        public object Tag { get; set; }

        public bool CanRedo { get; private set; }

        public void Do()
        {            
            _graphArea.Logic.Graph.AddEdge(_edge);
        }
    }
}