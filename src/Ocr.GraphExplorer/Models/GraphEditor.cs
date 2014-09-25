#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphEditor.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;
    using Catel.IoC;

    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services.Interfaces;

    public class GraphEditor : ModelBase
    {
        public GraphEditor()
        {
            var tag = GraphExplorerSection.Current.DefaultGraphDataService.ToString();
            var graphDataService = ServiceLocator.Default.ResolveType<IGraphDataService>(tag);
            Logic = new GraphLogic(graphDataService);
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