#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphArea.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Memento;
    using Catel.Services;
    using Csv.Services;
    using Messages;
    using Operations;
    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Views;

    using Services;

    public class GraphArea : ModelBase
    {
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;

        public GraphArea(string toolsetName, IMementoService mementoService, IMessageService messageService)
        {
            Argument.IsNotNullOrEmpty(() => toolsetName);

            _mementoService = mementoService;
            _messageService = messageService;
            ToolsetName = toolsetName;

            Logic = new GraphLogic();   
        }

        public void ReloadGraphArea(double offsetY)
        {
            if (GraphDataGetter == null)
            {
                return;
            }
            var logic = Logic;

            logic.PrepareGraphReloading();

            var graph = new Graph(GraphDataGetter);

            graph.ReloadGraph();
            logic.ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, Graph>(graph, 1.5, offsetY: offsetY);

            logic.ResumeGraphReloading(graph);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataGetter GraphDataGetter { get; set; }

        /// <summary>
        /// Called when the GraphDataGetter property has changed.
        /// </summary>
        private void OnGraphDataGetterChanged()
        {            
            if (GraphDataGetter == null)
            {
                return;
            }
            ReloadGraphArea(600);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataSaver GraphDataSaver { get; set; }

        private void OnGraphDataSaverChanged()
        {
            CanEdit = GraphDataSaver != null;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphLogic Logic
        {
            get { return GetValue<GraphLogic>(LogicProperty); }
            set { SetValue(LogicProperty, value); }
        }

        /// <summary>
        /// Register the Logic property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LogicProperty = RegisterProperty("Logic", typeof(GraphLogic), () => new GraphLogic(), (sender, e) => ((GraphArea)sender).OnLogicChanged());

        /// <summary>
        /// Called when the Logic property has changed.
        /// </summary>
        private void OnLogicChanged()
        {
            //Logic.GraphReloaded += Logic_GraphReloaded;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string ToolsetName { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDragEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsInEditing { get; set; }

        /// <summary>
        /// Called when the IsInEditing property has changed.
        /// </summary>
        private async Task OnIsInEditingChanged()
        {
            if (IsInEditing)
            {
                StatusMessage.SendWith("Edit Mode");
            }
            else
            {
                if (_mementoService.CanUndo)
                {
                    var messageResult = await _messageService.Show("Do you want to save changes?", "Confirmation", MessageButton.YesNoCancel, MessageImage.Question);
                    if (messageResult == MessageResult.Yes)
                    {
                        SaveChanges();
                    }
                    else if (messageResult == MessageResult.Cancel)
                    {
                        IsInEditing = true;
                        return;
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
            }
            EditingStartStopMessage.SendWith(IsInEditing, ToolsetName);
        }

        public void AddVertex(DataVertex dataVertex, Point point)
        {
            Argument.IsNotNull(() => dataVertex);

            var operation = new AddVertexOperation(this, dataVertex, point);
            _mementoService.Do(operation);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void AddEdge(DataVertex startVertex, DataVertex endVertex)
        {
            Argument.IsNotNull(() => startVertex);
            Argument.IsNotNull(() => endVertex);

            var edge = new DataEdge(startVertex, endVertex);
            var operation = new AddEdgeOperation(this, edge);
            _mementoService.Do(operation);
        }

        public void SaveChanges()
        {
            if (GraphDataSaver == null)
            {
                return;
            }

            GraphDataSaver.SaveChanges(Logic.Graph);

            IsInEditing = false;
        }

        public void RemoveEdge(DataEdge edge)
        {
            Argument.IsNotNull(() => edge);

            var operation = new RemoveEdgeOperation(this, edge);
            _mementoService.Do(operation);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void RemoveVertex(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            _mementoService.ClearRedoBatches();
            var operations = new OperationsBatch {Description = "remove vertex"};
            var graph = Logic.Graph;
            foreach (var edge in graph.InEdges(vertex).Concat(graph.OutEdges(vertex)).ToArray())
            {
                operations.AddOperation(new RemoveEdgeOperation(this, edge));
            }

            operations.AddOperation(new RemoveVertexOperation(this, vertex));
            _mementoService.Do(operations);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }
    }
}