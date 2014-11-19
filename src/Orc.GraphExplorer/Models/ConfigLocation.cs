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
        public string RelationshipsFile
        {
            get { return GetValue<string>(RelationshipsFileProperty); }
            set { SetValue(RelationshipsFileProperty, value); }
        }

        /// <summary>
        /// Register the RelationshipsFile property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RelationshipsFileProperty = RegisterProperty("RelationshipsFile", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string PropertiesFile
        {
            get { return GetValue<string>(PropertiesFileProperty); }
            set { SetValue(PropertiesFileProperty, value); }
        }

        /// <summary>
        /// Register the PropertiesFile property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesFileProperty = RegisterProperty("PropertiesFile", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool? EnableProperty
        {
            get { return GetValue<bool?>(EnablePropertyProperty); }
            set { SetValue(EnablePropertyProperty, value); }
        }

        /// <summary>
        /// Register the EnableProperty property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EnablePropertyProperty = RegisterProperty("EnableProperty", typeof(bool?), null);

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