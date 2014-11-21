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

    public class ConfigLocation : ModelBase
    {
        private readonly IConfigLocationService _configLocationService;

        public ConfigLocation(IConfigLocationService configLocationService)
        {
            _configLocationService = configLocationService;
        }

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

        public void Save()
        {
            _configLocationService.Save(this);
            SettingsChangedMessage.SendWith(true);
        }

        public void ChangeRelationshipsFileLocation()
        {
            var rellationshipsFile = _configLocationService.OpenRelationshipsFile();
            if (string.IsNullOrEmpty(rellationshipsFile))
            {
                return;
            }

            RelationshipsFile = rellationshipsFile;
        }

        public void ChangePropertiesFileLocation()
        {
            var propertiesFile = _configLocationService.OpenPropertiesFile();
            if (string.IsNullOrEmpty(propertiesFile))
            {
                return;
            }

            PropertiesFile = propertiesFile;           
        }
    }
}