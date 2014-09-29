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

    using Catel.Data;
    using Catel.IoC;
    using Csv.Services;
    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services.Interfaces;
    using Services;

    public class GraphArea : ModelBase
    {
        private readonly IGraphDataService _graphDataService;

        public GraphArea()
        {
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
    }
}