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
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;
    using GraphX.Controls;
    using Models.Data;
    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.Models;

    public class GraphToolsetViewModel : ViewModelBase, IGraphNavigator
    {
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;

        public GraphToolsetViewModel(GraphToolset toolset, IMementoService mementoService, IMessageService messageService)
        {
            _mementoService = mementoService;
            _messageService = messageService;
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
            Toolset.Refresh();
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
            Area.SaveChanges();                        
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
        // TODO: Rename it
        public GraphToolset Toolset
        {
            get
            {
                return GetValue<GraphToolset>(ToolsetProperty);
            }
            set
            {
                SetValue(ToolsetProperty, value);
            }
        }

        /// <summary>
        /// Register the Toolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ToolsetProperty = RegisterProperty("Toolset", typeof(GraphToolset), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Toolset")]
        public string ToolsetName
        {
            get
            {
                return GetValue<string>(ToolsetNameProperty);
            }
            set
            {
                SetValue(ToolsetNameProperty, value);
            }
        }

        /// <summary>
        /// Register the ToolsetName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ToolsetNameProperty = RegisterProperty("ToolsetName", typeof(string));


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Toolset")]
        [Model]
        public GraphArea Area
        {
            get
            {                
                return GetValue<GraphArea>(AreaProperty);
            }
            set
            {
                SetValue(AreaProperty, value);
            }
        }

        /// <summary>
        /// Register the Area property so it is known in the class.
        /// </summary>
        public static readonly PropertyData AreaProperty = RegisterProperty("Area", typeof(GraphArea));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool CanEdit
        {
            get { return GetValue<bool>(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        /// <summary>
        /// Register the CanEdit property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanEditProperty = RegisterProperty("CanEdit", typeof(bool));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsDragEnabled
        {
            get
            {
                return GetValue<bool>(IsDragEnabledProperty);
            }
            set
            {
                SetValue(IsDragEnabledProperty, value);
            }
        }

        /// <summary>
        /// Register the IsDragEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDragEnabledProperty = RegisterProperty("IsDragEnabled", typeof(bool));


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ZoomControlModes ZoomMode
        {
            get
            {
                return GetValue<ZoomControlModes>(ZoomModeProperty);
            }
            set
            {
                SetValue(ZoomModeProperty, value);
            }
        }

        /// <summary>
        /// Register the ZoomMode property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ZoomModeProperty = RegisterProperty("ZoomMode", typeof(ZoomControlModes), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsInEditing
        {
            get
            {
                return GetValue<bool>(IsInEditingProperty);
            }
            set
            {
                SetValue(IsInEditingProperty, value);
            }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool));       

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Toolset")]
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
        public bool IsAddingNewEdge
        {
            get { return GetValue<bool>(IsAddingNewEdgeProperty); }
            set { SetValue(IsAddingNewEdgeProperty, value); }
        }

        /// <summary>
        /// Register the IsAddingNewEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsAddingNewEdgeProperty = RegisterProperty("IsAddingNewEdge", typeof(bool), () => false, (sender, e) => ((GraphToolsetViewModel)sender).OnIsAddingNewEdgeChanged());

        /// <summary>
        /// Called when the IsAddingNewEdge property has changed.
        /// </summary>
        private void OnIsAddingNewEdgeChanged()
        {
            if (IsAddingNewEdge)
            {
                StatusMessage.SendWith("Select Source Node");
            }
            else
            {
                StatusMessage.SendWith("Exit Create Link");
            }
        }        


        public void NavigateTo(DataVertex dataVertex)
        {
            Argument.IsNotNull(() => dataVertex);

            var graphExplorer = GraphExplorer;
            if(graphExplorer != null)
            {
                graphExplorer.NavigateTo(dataVertex); 
            }
        }

        public GraphExplorerViewModel GraphExplorer {
            get { return ParentViewModel as GraphExplorerViewModel; }
        }
    }
}