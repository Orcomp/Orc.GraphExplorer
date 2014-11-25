#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphToolsetViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using Behaviors;

    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;
    using GraphX.Controls;
    using Models.Data;
    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.Models;
    using Services;

    public class GraphToolsetViewModel : ViewModelBase
    {
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;
        private readonly IGraphAreaEditorService _graphAreaEditorService;

        private readonly IGraphAreaLoadingService _graphAreaLoadingService;

        public GraphToolsetViewModel()
        {
            
        }
        public GraphToolsetViewModel(GraphToolset toolset, IMementoService mementoService, IMessageService messageService, IGraphAreaEditorService graphAreaEditorService, IGraphAreaLoadingService graphAreaLoadingService)
        {
            _mementoService = mementoService;
            _messageService = messageService;
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

        private void OnAreaChangedMessage(GraphChangedMessage message)
        {
            IsChanged = message.Data;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gets the SaveToXml command.
        /// </summary>
        public Command SaveToXml { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveToXml command is executed.
        /// </summary>
        private void OnSaveToXmlExecute()
        {
            Toolset.SaveToXml();
        }

        /// <summary>
        /// Gets the LoadFromXml command.
        /// </summary>
        public Command LoadFromXml { get; private set; }

        /// <summary>
        /// Method to invoke when the LoadFromXml command is executed.
        /// </summary>
        private void OnLoadFromXmlExecute()
        {
            Toolset.LoadFromXml();
        }

        /// <summary>
        /// Gets the SaveToImage command.
        /// </summary>
        public Command SaveToImage { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveToImage command is executed.
        /// </summary>
        private void OnSaveToImageExecute()
        {
            Toolset.SaveToImage();
        }

        /// <summary>
        /// Gets the RefreshCommand command.
        /// </summary>
        public Command RefreshCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the RefreshCommand command is executed.
        /// </summary>
        private void OnRefreshCommandExecute()
        {
            _graphAreaLoadingService.TryRefresh(Area);
        }

        /// <summary>
        /// Gets the SaveChangesCommand command.
        /// </summary>
        public Command SaveChangesCommand { get; private set; }

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
        /// Gets the UndoCommand command.
        /// </summary>
        public Command UndoCommand { get; private set; }

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
            Toolset.Undo();           
        }

        /// <summary>
        /// Gets the RedoCommand command.
        /// </summary>
        public Command RedoCommand { get; private set; }

        /// <summary>
        /// Method to check whether the RedoCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRedoCommandCanExecute()
        {
            return Toolset.CanRedo;
        }

        /// <summary>
        /// Method to invoke when the RedoCommand command is executed.
        /// </summary>
        private void OnRedoCommandExecute()
        {
            Toolset.Redo();            
        }

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

        /// <summary>
        /// Called when the IsAddingNewEdge property has changed.
        /// </summary>
        private void OnIsAddingNewEdgeChanged()
        {
            StatusMessage.SendWith(IsAddingNewEdge ? "Select Source Node" : "Exit Create Link");
        }

        public GraphExplorerViewModel GraphExplorer {
            get { return ParentViewModel as GraphExplorerViewModel; }
        }
    }
}