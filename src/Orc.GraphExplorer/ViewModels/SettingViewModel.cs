#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM;
    using Models;

    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel(Settings settings)
        {
            Settings = settings;
            CloseSettingsCommand = new Command(OnCloseSettingsCommandExecute);
        }

        /// <summary>
        /// Gets the CloseSettingsCommand command.
        /// </summary>
        public Command CloseSettingsCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CloseSettingsCommand command is executed.
        /// </summary>
        private void OnCloseSettingsCommandExecute()
        {
            IsSettingsVisible = false;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Settings Settings
        {
            get { return GetValue<Settings>(SettingsProperty); }
            private set { SetValue(SettingsProperty, value); }
        }

        /// <summary>
        /// Register the Settings property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SettingsProperty = RegisterProperty("Settings", typeof(Settings));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Settings")]
        public bool IsSettingsVisible
        {
            get { return GetValue<bool>(IsSettingsVisibleProperty); }
            set { SetValue(IsSettingsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsSettingsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsSettingsVisibleProperty = RegisterProperty("IsSettingsVisible", typeof(bool));

    }
}