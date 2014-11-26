#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Explorer.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System.ComponentModel;

    using Catel.Data;

    public class Explorer : ModelBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset EditorToolset { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset NavigatorToolset { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsNavTabVisible { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsNavTabSelected { get; set; }
        #endregion
    }
}