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
    using Catel.MVVM;
    using Microsoft.Win32;
    using Models;

    public class FilePickerViewModel : ViewModelBase
    {
        #region Constants
        /// <summary>
        /// Register the FilePicker property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilePickerProperty = RegisterProperty("FilePicker", typeof (FilePicker));

        /// <summary>
        /// Register the RelationshipsText property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RelationshipsTextProperty = RegisterProperty("RelationshipsText", typeof (string), null);

        /// <summary>
        /// Register the PropertiesText property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesTextProperty = RegisterProperty("PropertiesText", typeof (string), null);

        /// <summary>
        /// Register the EnableProperty property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EnablePropertyProperty = RegisterProperty("EnableProperty", typeof (bool?), null);
        #endregion

        #region Constructors
        public FilePickerViewModel()
        {
            ChangeRelationships = new Command(OnChangeRelationshipsExecute);
            ChangeProperties = new Command(OnChangePropertiesExecute, () => EnableProperty ?? false);
            Save = new Command(OnSaveExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public FilePicker FilePicker
        {
            get { return GetValue<FilePicker>(FilePickerProperty); }
            private set { SetValue(FilePickerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("FilePicker")]
        public string RelationshipsText
        {
            get { return GetValue<string>(RelationshipsTextProperty); }
            set { SetValue(RelationshipsTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("FilePicker")]
        public string PropertiesText
        {
            get { return GetValue<string>(PropertiesTextProperty); }
            set { SetValue(PropertiesTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("FilePicker")]
        public bool? EnableProperty
        {
            get { return GetValue<bool?>(EnablePropertyProperty); }
            set { SetValue(EnablePropertyProperty, value); }
        }

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
        public Command Save { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the ChangeRelationships command is executed.
        /// </summary>
        private void OnChangeRelationshipsExecute()
        {
            var dlg = new OpenFileDialog {Filter = "All files|*.csv", Title = "Select Relationship File"};
            if (dlg.ShowDialog() == true)
            {
                RelationshipsText = dlg.FileName;
            }
        }

        /// <summary>
        /// Method to invoke when the ChangeProperties command is executed.
        /// </summary>
        private void OnChangePropertiesExecute()
        {
            var dlg = new OpenFileDialog {Filter = "All files|*.csv", Title = "Select Properties File"};
            if (dlg.ShowDialog() == true)
            {
                PropertiesText = dlg.FileName;
            }
        }

        /// <summary>
        /// Method to invoke when the Save command is executed.
        /// </summary>
        private void OnSaveExecute()
        {
            try
            {
                FilePicker.Save();
                // TODO: this should be changed:
                //RaiseSettingAppliedEvent(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        protected override void Initialize()
        {
            FilePicker = new FilePicker();
            FilePicker.Load();
            base.Initialize();
        }
        #endregion
    }
}