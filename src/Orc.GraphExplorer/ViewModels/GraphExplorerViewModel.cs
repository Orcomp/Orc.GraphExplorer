#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using Behaviors;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;
    using Messages;
    using Models.Data;
    using Orc.GraphExplorer.Models;
    using Services;

    public class GraphExplorerViewModel : ViewModelBase, IGraphNavigator
    {

        private readonly IMementoService _mementoService;

        public GraphExplorerViewModel(IMementoService mementoService, IConfigLocationService configLocationService, IMessageService messageService)
        {
            _mementoService = mementoService;
            Explorer = new Explorer(_mementoService, configLocationService, messageService);

            CloseNavTabCommand = new Command(OnCloseNavTabCommandExecute);

            OpenSettingsCommand = new Command(OnOpenSettingsCommandExecute);

            EditingStartStopMessage.Register(this, OnEditingStartStopMessage, Explorer.EditorToolset.ToolsetName);
        }

        private void OnEditingStartStopMessage(EditingStartStopMessage message)
        {
            if (message.Data)
            {
                IsNavTabVisible = false;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsEditorTabSelected = true;
        }

        /// <summary>
        /// Gets the OpenSettingsCommand command.
        /// </summary>
        public Command OpenSettingsCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenSettingsCommand command is executed.
        /// </summary>
        private void OnOpenSettingsCommandExecute()
        {
            IsSettingsVisible = !IsSettingsVisible;
        }

        /// <summary>
        /// Gets the CloseNavTabCommand command.
        /// </summary>
        public Command CloseNavTabCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CloseNavTabCommand command is executed.
        /// </summary>
        private void OnCloseNavTabCommandExecute()
        {
            IsNavTabVisible = false;
            IsNavTabSelected = false;
            IsEditorTabSelected = true;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Explorer Explorer
        {
            get { return GetValue<Explorer>(ExplorerProperty); }
            private set { SetValue(ExplorerProperty, value); }
        }

        /// <summary>
        /// Register the Explorer property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ExplorerProperty = RegisterProperty("Explorer", typeof (Explorer));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [ViewModelToModel("Explorer")]
        public Settings Settings
        {
            get { return GetValue<Settings>(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
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

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [ViewModelToModel("Explorer")]
        public GraphToolset EditorToolset
        {
            get { return GetValue<GraphToolset>(EditorToolsetProperty); }
            set { SetValue(EditorToolsetProperty, value); }
        }

        /// <summary>
        /// Register the EditoToolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorToolsetProperty = RegisterProperty("EditorToolset", typeof(GraphToolset));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("EditorToolset")]
        public bool IsChanged
        {
            get { return GetValue<bool>(IsChangedProperty); }
            set { SetValue(IsChangedProperty, value); }
        }

        /// <summary>
        /// Register the IsChanged property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsChangedProperty = RegisterProperty("IsChanged", typeof(bool));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabVisible
        {
            get { return GetValue<bool>(IsNavTabVisibleProperty); }
            set { SetValue(IsNavTabVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsNavTabVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabVisibleProperty = RegisterProperty("IsNavTabVisible", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabSelected
        {
            get { return GetValue<bool>(IsNavTabSelectedProperty); }
            set { SetValue(IsNavTabSelectedProperty, value); }
        }

        /// <summary>
        /// Register the IsNavTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabSelectedProperty = RegisterProperty("IsNavTabSelected", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsEditorTabSelected
        {
            get { return GetValue<bool>(IsEditorTabSelectedProperty); }
            set { SetValue(IsEditorTabSelectedProperty, value); }
        }

        /// <summary>
        /// Register the IsEditorTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsEditorTabSelectedProperty = RegisterProperty("IsEditorTabSelected", typeof (bool), () => false);

        public void NavigateTo(DataVertex dataVertex)
        {
            IsNavTabVisible = true;
            IsNavTabSelected = true;
            Explorer.NavigateTo(dataVertex);
        }

    }
}