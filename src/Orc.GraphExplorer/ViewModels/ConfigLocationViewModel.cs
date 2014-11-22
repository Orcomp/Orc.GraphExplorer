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

    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.IoC;
    using Catel.MVVM;
    using Messages;
    using Microsoft.Win32;
    using Models;
    using Services;

    public class ConfigLocationViewModel : ViewModelBase
    {
        private readonly IConfigLocationService _configLocationService;

        #region Constructors
        public ConfigLocationViewModel(ConfigLocation configLocation, IConfigLocationService configLocationService)
        {
            _configLocationService = configLocationService;
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
            _configLocationService.ChangeRelationshipsFileLocation(ConfigLocation);
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
            _configLocationService.ChangePropertiesFileLocation(ConfigLocation);           
        }

        /// <summary>
        /// Gets the Save command.
        /// </summary>
        public new Command Save { get; private set; }

        /// <summary>
        /// Method to invoke when the Save command is executed.
        /// </summary>
        private void OnSaveExecute()
        {
            _configLocationService.Save(ConfigLocation);
            SettingsChangedMessage.SendWith(true);
        }

        #endregion // Commands

        #region Properties
        [Model]
        [Expose("RelationshipsFile")]
        [Expose("PropertiesFile")]
        [Expose("EnableProperty")]
        public ConfigLocation ConfigLocation { get; private set; }

        [ViewModelToModel("ConfigLocation")]
        public bool? EnableProperty { get; set; }
        #endregion
    }
}