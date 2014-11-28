#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataLocationSettingsViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System.Linq;
    using Catel;
    using Catel.Configuration;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Messages;
    using Models;
    using Services;

    public class DataLocationSettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDataLocationSettingsService _dataLocationSettingsService;
        private readonly IOpenFileService _openFileService;
        #endregion

        #region Constructors
        public DataLocationSettingsViewModel(DataLocationSettings dataLocationSettings, IDataLocationSettingsService dataLocationSettingsService, IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => dataLocationSettings);
            Argument.IsNotNull(() => dataLocationSettingsService);
            Argument.IsNotNull(() => openFileService);

            _dataLocationSettingsService = dataLocationSettingsService;
            _openFileService = openFileService;
            DataLocationSettings = dataLocationSettings;
            ChangeRelationships = new Command(OnChangeRelationshipsExecute);
            ChangeProperties = new Command(OnChangePropertiesExecute, () => EnableProperty ?? false);
            Save = new Command(OnSaveExecute);
        }

        #endregion

        #region Commands

        #region Properties
        /// <summary>
        /// Gets the ChangeRelationships command.
        /// </summary>
        public Command ChangeRelationships { get; private set; }

        /// <summary>
        /// Gets the ChangeProperties command.
        /// </summary>
        public Command ChangeProperties { get; private set; }

        /// <summary>
        /// Gets the Save command.
        /// </summary>
        public new Command Save { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the ChangeRelationships command is executed.
        /// </summary>
        private void OnChangeRelationshipsExecute()
        {
            var rellationshipsFile = OpenRelationshipsFile();
            if (string.IsNullOrEmpty(rellationshipsFile))
            {
                return;
            }

            DataLocationSettings.RelationshipsFile = rellationshipsFile;
        }

        private string OpenRelationshipsFile()
        {
            return OpenFile("Select Relationship File");
        }

        private string OpenPropertiesFile()
        {
            return OpenFile("Select Properties File");
        }

        private string OpenFile(string title)
        {
            Argument.IsNotNullOrEmpty(() => title);

            _openFileService.IsMultiSelect = false;
            _openFileService.Filter = "All files|*.csv";
            _openFileService.Title = title;

            return _openFileService.DetermineFile() ? _openFileService.FileNames.Single() : null;
        }

        /// <summary>
        /// Method to invoke when the ChangeProperties command is executed.
        /// </summary>
        private void OnChangePropertiesExecute()
        {
            var propertiesFile = OpenPropertiesFile();
            if (string.IsNullOrEmpty(propertiesFile))
            {
                return;
            }

            DataLocationSettings.PropertiesFile = propertiesFile;
        }

        /// <summary>
        /// Method to invoke when the Save command is executed.
        /// </summary>
        private void OnSaveExecute()
        {
            _dataLocationSettingsService.Save(DataLocationSettings);
            SettingsChangedMessage.SendWith(true);
        }
        #endregion

        #endregion // Commands

        #region Properties
        [Model]
        [Expose("RelationshipsFile")]
        [Expose("PropertiesFile")]
        [Expose("EnableProperty")]
        public DataLocationSettings DataLocationSettings { get; private set; }

        [ViewModelToModel("DataLocationSettings")]
        public bool? EnableProperty { get; set; }
        #endregion
    }
}