#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaEditorService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Memento;
    using Catel.Services;
    using Messages;
    using Models;
    using Operations;

    public class GraphAreaEditorService : IGraphAreaEditorService
    {
        #region Fields
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public GraphAreaEditorService(IMementoService mementoService, IMessageService messageService)
        {
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);

            _mementoService = mementoService;
            _messageService = messageService;
        }
        #endregion

        #region IGraphAreaEditorService Members
        public void SaveChanges(GraphArea area)
        {
            Argument.IsNotNull(() => area);

            var dataSaver = area.GraphDataSaver;

            if (dataSaver == null)
            {
                return;
            }

            dataSaver.SaveChanges(area.Logic.Graph);

            area.IsInEditing = false;           
        }

        public void AddVertex(GraphArea area, DataVertex dataVertex, Point point)
        {
            Argument.IsNotNull(() => area);
            Argument.IsNotNull(() => dataVertex);

            var operation = new AddVertexOperation(area, dataVertex, point);
            _mementoService.Do(operation);

            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void AddEdge(GraphArea area, DataVertex startVertex, DataVertex endVertex)
        {
            Argument.IsNotNull(() => area);
            Argument.IsNotNull(() => startVertex);
            Argument.IsNotNull(() => endVertex);

            var edge = new DataEdge(startVertex, endVertex);
            var operation = new AddEdgeOperation(area, edge);
            _mementoService.Do(operation);
        }

        public void RemoveEdge(GraphArea area, DataEdge edge)
        {
            Argument.IsNotNull(() => area);
            Argument.IsNotNull(() => edge);

            var operation = new RemoveEdgeOperation(area, edge);
            _mementoService.Do(operation);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void RemoveVertex(GraphArea area, DataVertex vertex)
        {
            Argument.IsNotNull(() => area);
            Argument.IsNotNull(() => vertex);

            _mementoService.ClearRedoBatches();
            var operations = new OperationsBatch {Description = "remove vertex"};
            var graph = area.Logic.Graph;
            foreach (var edge in graph.InEdges(vertex).Concat(graph.OutEdges(vertex)).ToArray())
            {
                operations.AddOperation(new RemoveEdgeOperation(area, edge));
            }

            operations.AddOperation(new RemoveVertexOperation(area, vertex));
            _mementoService.Do(operations);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public async Task<bool> TryExitEditMode(GraphArea area)
        {
            Argument.IsNotNull(() => area);

            if (_mementoService.CanUndo)
            {
                var messageResult = await _messageService.Show("Do you want to save changes?", "Confirmation", MessageButton.YesNoCancel, MessageImage.Question);
                if (messageResult == MessageResult.Yes)
                {
                    SaveChanges(area);
                }
                else if (messageResult == MessageResult.Cancel)
                {
                    area.IsInEditing = true;
                    return false;
                }
                else
                {
                    while (_mementoService.CanUndo)
                    {
                        _mementoService.Undo();
                    }
                }
            }
            _mementoService.Clear();

            GraphChangedMessage.SendWith(_mementoService.CanUndo);

            StatusMessage.SendWith("Exit Edit Mode");
            return true;
        }
        #endregion
    }
}