#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Models;

    public class SettingViewModel : ViewModelBase
    {
        #region Constructors
        public SettingViewModel(Settings settings)
        {
            Argument.IsNotNull(() => settings);

            Settings = settings;
            CloseSettingsCommand = new Command(OnCloseSettingsCommandExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the CloseSettingsCommand command.
        /// </summary>
        public Command CloseSettingsCommand { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Settings")]
        public bool IsSettingsVisible { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the CloseSettingsCommand command is executed.
        /// </summary>
        private void OnCloseSettingsCommandExecute()
        {
            IsSettingsVisible = false;
        }
        #endregion
    }
}