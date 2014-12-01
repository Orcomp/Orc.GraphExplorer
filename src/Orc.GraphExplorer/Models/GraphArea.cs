#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphArea.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System.ComponentModel;

    using Catel;
    using Catel.Data;

    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services;

    public class GraphArea : ModelBase
    {
        #region Constructors
        public GraphArea(string toolsetName)
        {
            Argument.IsNotNullOrEmpty(() => toolsetName);

            ToolsetName = toolsetName;

            Logic = new GraphLogic();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataGetter GraphDataGetter { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataSaver GraphDataSaver { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphLogic Logic { get; set; }

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
        #endregion
    }
}