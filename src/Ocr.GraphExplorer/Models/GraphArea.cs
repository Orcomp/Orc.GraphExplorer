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
    using System.Windows;

    using Catel.Data;
    using Catel.IoC;
    using Catel.Memento;

    using Csv.Services;
    using Helpers;
    using Operations;
    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services.Interfaces;
    using Orc.GraphExplorer.Views;

    using Services;

    public class GraphArea : ModelBase
    {
        private readonly IMementoService _mementoService;

        public GraphArea(string toolsetName, IMementoService mementoService)
        {
            _mementoService = mementoService;
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
        public ObservableCollection<FilterableEntity> FilterableEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilterableEntitiesProperty); }
            set { SetValue(FilterableEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilterableEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterableEntitiesProperty = RegisterProperty("FilterableEntities", typeof(ObservableCollection<FilterableEntity>), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilteredEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilteredEntitiesProperty); }
            set { SetValue(FilteredEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilteredEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredEntitiesProperty = RegisterProperty("FilteredEntities", typeof(ObservableCollection<FilterableEntity>), null);


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
            Logic.GraphReloaded += Logic_GraphReloaded;
        }

        void Logic_GraphReloaded(object sender, Events.GraphEventArgs e)
        {
            //throw new NotImplementedException();
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
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), () => false);


        public void AddVertex(DataVertex dataVertex, Point point)
        {
            var operation = new AddVertexOperation(this, dataVertex, point);
            _mementoService.Do(operation);
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
        }

        public void RemoveEdge(DataEdge edge)
        {
            var operation = new RemoveEdgeOperation(this, edge);
            _mementoService.Do(operation);
        }

        public void RemoveVertex(DataVertex vertex)
        {
            _mementoService.ClearRedoBatches();
            var operations = new OperationsBatch();
            foreach (var edge in Logic.Graph.InEdges(vertex).Concat(Logic.Graph.OutEdges(vertex)).ToArray())
            {
                operations.AddOperation(new RemoveEdgeOperation(this, edge));
            }

            operations.AddOperation(new RemoveVertexOperation(this, vertex));
            _mementoService.Do(operations);
        }
    }
}