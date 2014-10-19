#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="AddVertexOperation.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Operations
{
    using System.Windows;
    using Catel.Memento;
    using Interfaces;
    using Models;

    using Orc.GraphExplorer.Messages;

    public class AddVertexOperation : IOperation
    {
        private readonly GraphArea _graphArea;
        private readonly DataVertex _dataVertex;
        private readonly Point _point;

        public AddVertexOperation(GraphArea graphArea, DataVertex dataVertex, Point point)
        {
            _graphArea = graphArea;
            _dataVertex = dataVertex;
            _point = point;

            Target = _graphArea;
            CanRedo = true;

            Description = "add vertex";
        }

        public void Undo()
        {
            _graphArea.Logic.Graph.RemoveVertex(_dataVertex);
            StatusMessage.SendWith(string.Format("Undo {0}", Description));
        }

        public void Redo()
        {
            Do();
            StatusMessage.SendWith(string.Format("Redo {0}", Description));
        }

        public object Target { get; private set; }
        public string Description { get; set; }
        public object Tag { get; set; }
        public bool CanRedo { get; private set; }

        public void Do()
        {
            _dataVertex.Tag = _point;
            _graphArea.Logic.Graph.AddVertex(_dataVertex);
        }
    }
}