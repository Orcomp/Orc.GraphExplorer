#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePickerViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Windows;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Microsoft.Win32;
    using Models;

    using Orc.GraphExplorer.Services.Interfaces;

    public class ConfigLocationViewModel : ViewModelBase
    {
    
        #region Constructors
        public ConfigLocationViewModel(ConfigLocation configLocation)
        {
            ConfigLocation = configLocation;
            ChangeRelationships = new Command(OnChangeRelationshipsExecute);
            ChangeProperties = new Command(OnChangePropertiesExecute, () => EnableProperty ?? false);
            Save = new Command(OnSaveExecute);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the ChangeRelationships command.
        /// </summary>
        public Command ChangeRelationships { get; private set; }

        /// <summary>
        /// Method to invoke when the ChangeRelationships command is executed.
        /// </summary>
        private void OnChangeRelationshipsExecute()
        {
            ConfigLocation.ChangeRelationshipsFileLocation();
        }


        /// <summary>
        /// Gets the ChangeProperties command.
        /// </summary>
        public Command ChangeProperties { get; private set; }

        /// <summary>
        /// Method to invoke when the ChangeProperties command is executed.
        /// </summary>
        private void OnChangePropertiesExecute()
        {
            ConfigLocation.ChangePropertiesFileLocation();            
        }

        /// <summary>
        /// Gets the Save command.
        /// </summary>
        public Command Save { get; private set; }

        /// <summary>
        /// Method to invoke when the Save command is executed.
        /// </summary>
        private void OnSaveExecute()
        {
            ConfigLocation.Save();
        }

        #endregion // Commands

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public ConfigLocation ConfigLocation
        {
            get { return GetValue<ConfigLocation>(FilePickerProperty); }
            private set { SetValue(FilePickerProperty, value); }
        }

        /// <summary>
        /// Register the ConfigLocation property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilePickerProperty = RegisterProperty("ConfigLocation", typeof(ConfigLocation));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("ConfigLocation")]
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
        [ViewModelToModel("ConfigLocation")]
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
        [ViewModelToModel("ConfigLocation")]
        public bool? EnableProperty
        {
            get { return GetValue<bool?>(EnablePropertyProperty); }
            set { SetValue(EnablePropertyProperty, value); }
        }

        /// <summary>
        /// Register the EnableProperty property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EnablePropertyProperty = RegisterProperty("EnableProperty", typeof(bool?), null);
        #endregion
    }
}