#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphToolset.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using Catel;
    using Catel.Data;

    public class GraphToolset : ModelBase
    {
        #region Constructors
        public GraphToolset(string toolsetName, bool isFilterEnabled)
        {
            Argument.IsNotNullOrEmpty(() => toolsetName);

            ToolsetName = toolsetName;
            Area = new GraphArea(toolsetName);
            Filter = new Filter(Area.Logic) { IsFilterEnabled = isFilterEnabled };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of toolset
        /// </summary>
        public string ToolsetName { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphArea Area { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Filter Filter { get; set; }
        #endregion
    }
}