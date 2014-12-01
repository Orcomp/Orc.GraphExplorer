#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataLocationSettings.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using Catel.Data;

    public class DataLocationSettings : ModelBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string RelationshipsFile { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string PropertiesFile { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool? EnableProperty { get; set; }
        #endregion
    }
}