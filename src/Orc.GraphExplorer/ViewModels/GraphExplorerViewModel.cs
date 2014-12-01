#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;
    using Factories;
    using Messages;
    using Models;
    using Services;

    public class GraphExplorerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IGraphDataService _graphDataService;
        private readonly IGraphExplorerFactory _graphExplorerFactory;

        private readonly INavigationService _navigationService;
        #endregion

        #region Constructors
        public GraphExplorerViewModel(IGraphDataService graphDataService, IGraphExplorerFactory graphExplorerFactory, INavigationService navigationService)
        {
            Argument.IsNotNull(() => graphDataService);
            Argument.IsNotNull(() => graphExplorerFactory);
            Argument.IsNotNull(() => navigationService);

            _graphDataService = graphDataService;
            _graphExplorerFactory = graphExplorerFactory;
            _navigationService = navigationService;

            Explorer = _graphExplorerFactory.CreateExplorer();

            CloseNavTabCommand = new Command(OnCloseNavTabCommandExecute);

            OpenSettingsCommand = new Command(OnOpenSettingsCommandExecute);

            EditingStartStopMessage.Register(this, OnEditingStartStopMessage, Explorer.EditorToolset.ToolsetName);
            ReadyToLoadGraphMessage.Register(this, OnReadyToLoadGraphMessage);
            NavigationMessage.Register(this, OnNavigationMessage);
        }


        #endregion

        #region Properties
        /// <summary>
        /// Gets the OpenSettingsCommand command.
        /// </summary>
        public Command OpenSettingsCommand { get; private set; }

        /// <summary>
        /// Gets the CloseNavTabCommand command.
        /// </summary>
        public Command CloseNavTabCommand { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Explorer Explorer { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [ViewModelToModel("Explorer")]
        public Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Settings")]
        public bool IsSettingsVisible { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [ViewModelToModel("Explorer")]
        public GraphToolset EditorToolset { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("EditorToolset")]
        public bool IsChanged { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Explorer")]
        public bool IsNavTabVisible { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Explorer")]
        public bool IsNavTabSelected { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsEditorTabSelected { get; set; }
        #endregion

        #region Methods
        private void OnNavigationMessage(NavigationMessage message)
        {
            _navigationService.NavigateTo(Explorer, message.Data);
        }

        private void OnReadyToLoadGraphMessage(ReadyToLoadGraphMessage message)
        {
            var editorArea = Explorer.EditorToolset.Area;
            if (string.Equals(message.Data, "Editor") && editorArea.GraphDataGetter == null)
            {
                editorArea.GraphDataGetter = _graphDataService;
                editorArea.GraphDataSaver = _graphDataService;
            }

            var navigatorArea = Explorer.NavigatorToolset.Area;
            if (string.Equals(message.Data, "Navigator") && navigatorArea.GraphDataGetter == null)
            {
                navigatorArea.GraphDataGetter = new NavigatorGraphDataGetter();
            }
        }

        private void OnEditingStartStopMessage(EditingStartStopMessage message)
        {
            if (message.Data)
            {
                IsNavTabVisible = false;
            }
        }

        protected override async Task Initialize()
        {
            await base.Initialize();
            IsEditorTabSelected = true;
        }

        /// <summary>
        /// Method to invoke when the OpenSettingsCommand command is executed.
        /// </summary>
        private void OnOpenSettingsCommandExecute()
        {
            IsSettingsVisible = !IsSettingsVisible;
        }

        /// <summary>
        /// Method to invoke when the CloseNavTabCommand command is executed.
        /// </summary>
        private void OnCloseNavTabCommandExecute()
        {
            IsNavTabVisible = false;
            IsNavTabSelected = false;
            IsEditorTabSelected = true;
        }
        #endregion
    }
}