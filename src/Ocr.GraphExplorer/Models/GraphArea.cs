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

        private readonly IGraphDataService _graphDataService;

        public GraphArea(string toolsetName, IMementoService mementoService)
        {
            _mementoService = mementoService;
            ToolsetName = toolsetName;

            _graphDataService = new CsvGraphDataService();

            Logic = new GraphLogic();            
        }

        public void CreateGraphArea(double offsetY)
        {
            Logic.PrepareGraphReloading();

            var graph = new Graph(_graphDataService);

            graph.ReloadGraph().Subscribe(x =>
            {
                Logic.ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, Graph>(graph, 1.5, offsetY: offsetY);

                Logic.ResumeGraphReloading(graph);
            }, ex =>
            { });
        }

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
        public static readonly PropertyData LogicProperty = RegisterProperty("Logic", typeof(GraphLogic), null);

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
            operation.Do();

            _mementoService.ClearRedoBatches();

            _mementoService.Add(operation);
        }

        
    }
}