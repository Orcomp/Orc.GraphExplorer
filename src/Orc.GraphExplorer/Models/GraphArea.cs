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
        public GraphArea(string toolsetName)
        {
            Argument.IsNotNullOrEmpty(() => toolsetName);

            ToolsetName = toolsetName;

            Logic = new GraphLogic();   
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataGetter GraphDataGetter { get; set; }        

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
    }
}