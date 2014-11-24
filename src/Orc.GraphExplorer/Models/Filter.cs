#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Catel;
    using Catel.Data;
    using Data;

    public class Filter : ModelBase
    {
        #region Constructors
        public Filter(GraphLogic graphLogic)
        {
            Argument.IsNotNull(() => graphLogic);

            GraphLogic = graphLogic;

            FilterableEntities = new ObservableCollection<FilterableEntity>();
            FilteredEntities = new ObservableCollection<FilterableEntity>();
        }
        #endregion

        #region Properties
        public GraphLogic GraphLogic { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsFilterEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsHideVertexes { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsFilterApplied { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilterableEntities { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilteredEntities { get; set; }
        #endregion
    }
}