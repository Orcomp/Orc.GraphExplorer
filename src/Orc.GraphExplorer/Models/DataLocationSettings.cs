#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePicker.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System.Configuration;
    using Catel.Data;
    using Catel.MVVM;
    using Messages;
    using Microsoft.Win32;
    using Services;

    public class DataLocationSettings : ModelBase
    {
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
    }
}