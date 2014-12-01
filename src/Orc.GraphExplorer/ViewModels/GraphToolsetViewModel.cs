#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphToolsetViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System.ComponentModel;
    using Catel;
    using Catel.Fody;
    using Catel.Memento;
    using Catel.MVVM;
    using GraphX;
    using GraphX.Controls;
    using Messages;
    using Microsoft.Win32;
    using Models;
    using Services;

    public class GraphToolsetViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMementoService _mementoService;
        private readonly IGraphAreaEditorService _graphAreaEditorService;

        private readonly IGraphAreaLoadingService _graphAreaLoadingService;
        #endregion

        #region Constructors
        public GraphToolsetViewModel()
        {
        }

        public GraphToolsetViewModel(GraphToolset toolset, IMementoService mementoService, IGraphAreaEditorService graphAreaEditorService, IGraphAreaLoadingService graphAreaLoadingService)
        {
            Argument.IsNotNull(() => toolset);
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => graphAreaEditorService);
            Argument.IsNotNull(() => graphAreaLoadingService);

            _mementoService = mementoService;
            _graphAreaEditorService = graphAreaEditorService;
            _graphAreaLoadingService = graphAreaLoadingService;
            Toolset = toolset;

            SaveToXml = new Command(OnSaveToXmlExecute);
            LoadFromXml = new Command(OnLoadFromXmlExecute);
            SaveToImage = new Command(OnSaveToImageExecute);

            UndoCommand = new Command(OnUndoCommandExecute, OnUndoCommandCanExecute);
            RedoCommand = new Command(OnRedoCommandExecute, OnRedoCommandCanExecute);

            SaveChangesCommand = new Command(OnSaveChangesCommandExecute, OnSaveChangesCommandCanExecute);
            RefreshCommand = new Command(OnRefreshCommandExecute);

            GraphChangedMessage.Register(this, OnAreaChangedMessage);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the SaveToXml command.
        /// </summary>
        public Command SaveToXml { get; private set; }

        /// <summary>
        /// Gets the LoadFromXml command.
        /// </summary>
        public Command LoadFromXml { get; private set; }

        /// <summary>
        /// Gets the SaveToImage command.
        /// </summary>
        public Command SaveToImage { get; private set; }

        /// <summary>
        /// Gets the RefreshCommand command.
        /// </summary>
        public Command RefreshCommand { get; private set; }

        /// <summary>
        /// Gets the SaveChangesCommand command.
        /// </summary>
        public Command SaveChangesCommand { get; private set; }

        /// <summary>
        /// Gets the UndoCommand command.
        /// </summary>
        public Command UndoCommand { get; private set; }

        /// <summary>
        /// Gets the RedoCommand command.
        /// </summary>
        public Command RedoCommand { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [Expose("ToolsetName")]
        public GraphToolset Toolset { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Toolset")]
        [Model]
        [Expose("CanEdit")]
        [Expose("IsDragEnabled")]
        [Expose("IsInEditing")]
        public GraphArea Area { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ZoomControlModes ZoomMode { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Toolset")]
        public bool IsChanged { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsAddingNewEdge { get; set; }

        public GraphExplorerViewModel GraphExplorer
        {
            get { return ParentViewModel as GraphExplorerViewModel; }
        }
        #endregion

        #region Methods
        private void OnAreaChangedMessage(GraphChangedMessage message)
        {
            UpdateEditionState(message.Data);
        }

        private void UpdateEditionState(bool isChanged)
        {
            IsChanged = isChanged;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Method to invoke when the SaveToXml command is executed.
        /// </summary>
        private void OnSaveToXmlExecute()
        {
            var dlg = new SaveFileDialog {Filter = "All files|*.xml", Title = "Select layout file name", FileName = "overrall_layout.xml"};
            if (dlg.ShowDialog() == true)
            {
                SaveToXmlMessage.SendWith(dlg.FileName, Toolset.ToolsetName);
            }
        }

        /// <summary>
        /// Method to invoke when the LoadFromXml command is executed.
        /// </summary>
        private void OnLoadFromXmlExecute()
        {
            var dlg = new OpenFileDialog {Filter = "All files|*.xml", Title = "Select layout file", FileName = "overrall_layout.xml"};
            if (dlg.ShowDialog() == true)
            {
                LoadFromXmlMessage.SendWith(dlg.FileName, Toolset.ToolsetName);
            }
        }

        /// <summary>
        /// Method to invoke when the SaveToImage command is executed.
        /// </summary>
        private void OnSaveToImageExecute()
        {
            SaveToImageMessage.SendWith(ImageType.PNG, Toolset.ToolsetName);
        }

        /// <summary>
        /// Method to invoke when the RefreshCommand command is executed.
        /// </summary>
        private void OnRefreshCommandExecute()
        {
            _graphAreaLoadingService.TryRefresh(Area);
        }

        /// <summary>
        /// Method to check whether the SaveChangesCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnSaveChangesCommandCanExecute()
        {
            return _mementoService.CanUndo;
        }

        /// <summary>
        /// Method to invoke when the SaveChangesCommand command is executed.
        /// </summary>
        private void OnSaveChangesCommandExecute()
        {
            _graphAreaEditorService.SaveChanges(Area);
        }

        /// <summary>
        /// Method to check whether the UndoCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnUndoCommandCanExecute()
        {
            return _mementoService.CanUndo;
        }

        /// <summary>
        /// Method to invoke when the UndoCommand command is executed.
        /// </summary>
        private void OnUndoCommandExecute()
        {
            _mementoService.Undo();
            UpdateEditionState(_mementoService.CanUndo);
        }

        /// <summary>
        /// Method to check whether the RedoCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRedoCommandCanExecute()
        {
            return _mementoService.CanRedo;
        }

        /// <summary>
        /// Method to invoke when the RedoCommand command is executed.
        /// </summary>
        private void OnRedoCommandExecute()
        {
            _mementoService.Redo();
            UpdateEditionState(_mementoService.CanUndo);
        }

        /// <summary>
        /// Called when the IsAddingNewEdge property has changed.
        /// </summary>
        private void OnIsAddingNewEdgeChanged()
        {
            StatusMessage.SendWith(IsAddingNewEdge ? "Select Source Node" : "Exit Create Link");
        }
        #endregion
    }
}