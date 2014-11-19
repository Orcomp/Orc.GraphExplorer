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
            Logic.PrepareGraphReloading();

            var graph = new Graph(GraphDataGetter);

            graph.ReloadGraph();
            Logic.ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, Graph>(graph, 1.5, offsetY: offsetY);

            Logic.ResumeGraphReloading(graph);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataGetter GraphDataGetter
        {
            get { return GetValue<IGraphDataGetter>(GraphDataGetterProperty); }
            set { SetValue(GraphDataGetterProperty, value); }
        }

        /// <summary>
        /// Register the GraphDataGetter property so it is known in the class.
        /// </summary>
        public static readonly PropertyData GraphDataGetterProperty = RegisterProperty("GraphDataGetter", typeof(IGraphDataGetter), null, (sender, e) => ((GraphArea)sender).OnGraphDataGetterChanged());

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
        public IGraphDataSaver GraphDataSaver
        {
            get { return GetValue<IGraphDataSaver>(GraphDataSaverProperty); }
            set { SetValue(GraphDataSaverProperty, value); }
        }

        /// <summary>
        /// Register the GraphDataSaver property so it is known in the class.
        /// </summary>
        public static readonly PropertyData GraphDataSaverProperty = RegisterProperty("GraphDataSaver", typeof(IGraphDataSaver), null, (sender, e) => ((GraphArea)sender).OnGraphDataSaverChanged());

        private void OnGraphDataSaverChanged()
        {
            CanEdit = GraphDataSaver != null;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanEdit
        {
            get { return GetValue<bool>(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        /// <summary>
        /// Register the CanEdit property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanEditProperty = RegisterProperty("CanEdit", typeof(bool), () => false);

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
        public string ToolsetName
        {
            get { return GetValue<string>(ToolsetNameProperty); }
            set { SetValue(ToolsetNameProperty, value); }
        }

        /// <summary>
        /// Register the ToolsetName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ToolsetNameProperty = RegisterProperty("ToolsetName", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsDragEnabled
        {
            get { return GetValue<bool>(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsDragEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDragEnabledProperty = RegisterProperty("IsDragEnabled", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), () => false, (sender, e) => ((GraphArea)sender).OnIsInEditingChanged());

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
            var operation = new AddVertexOperation(this, dataVertex, point);
            _mementoService.Do(operation);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void AddEdge(DataVertex startVertex, DataVertex endVertex)
        {
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
            var operation = new RemoveEdgeOperation(this, edge);
            _mementoService.Do(operation);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void RemoveVertex(DataVertex vertex)
        {
            _mementoService.ClearRedoBatches();
            var operations = new OperationsBatch {Description = "remove vertex"};
            foreach (var edge in Logic.Graph.InEdges(vertex).Concat(Logic.Graph.OutEdges(vertex)).ToArray())
            {
                operations.AddOperation(new RemoveEdgeOperation(this, edge));
            }

            operations.AddOperation(new RemoveVertexOperation(this, vertex));
            _mementoService.Do(operations);
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }
    }
}