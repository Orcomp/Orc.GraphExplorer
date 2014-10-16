#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphToolsetViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using Behaviors.Interfaces;
    using Catel.Data;
    using Catel.Memento;
    using Catel.MVVM;

    using GraphX.Controls;
    using Models.Data;
    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.Models;
    using Services.Interfaces;

    public class GraphToolsetViewModel : ViewModelBase, IGraphNavigator
    {
        private readonly IMementoService _mementoService;

        public GraphToolsetViewModel()
        {
            
        }
        public GraphToolsetViewModel(GraphToolset toolset, IMementoService mementoService)
        {
            _mementoService = mementoService;
            Toolset = toolset;

            SaveToXml = new Command(OnSaveToXmlExecute);
            LoadFromXml = new Command(OnLoadFromXmlExecute);
            SaveToImage = new Command(OnSaveToImageExecute);

            UndoCommand = new Command(OnUndoCommandExecute, OnUndoCommandCanExecute);
            RedoCommand = new Command(OnRedoCommandExecute, OnRedoCommandCanExecute);

            SaveChangesCommand = new Command(OnSaveChangesCommandExecute, OnSaveChangesCommandCanExecute);
            RefreshCommand = new Command(OnRefreshCommandExecute);
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
            if (IsInEditing && _mementoService.CanUndo)
            {
                if (MessageBox.Show("Refresh view in edit mode will discard changes you made, will you want to continue?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IsInEditing = false;
                    IsDragEnabled = false;
                  //  IsFilterApplied = false;
                    Area.ReloadGraphArea(600);
                }
            }
            else
            {
                Area.ReloadGraphArea(600);
            }
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
            _mementoService.Clear();
            IsInEditing = false;
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
            _mementoService.Undo();
            RedoCommand.RaiseCanExecuteChanged();
            SaveChangesCommand.RaiseCanExecuteChanged();
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
            return _mementoService.CanRedo;
        }

        /// <summary>
        /// Method to invoke when the RedoCommand command is executed.
        /// </summary>
        private void OnRedoCommandExecute()
        {
            _mementoService.Redo();
            UndoCommand.RaiseCanExecuteChanged();
            SaveChangesCommand.RaiseCanExecuteChanged();
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
        [ViewModelToModel("Area")]
        public ObservableCollection<FilterableEntity> FilterableEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilterableEntitiesProperty); }
            set { SetValue(FilterableEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilterableEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterableEntitiesProperty = RegisterProperty("FilterableEntities", typeof(ObservableCollection<FilterableEntity>));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public ObservableCollection<FilterableEntity> FilteredEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilteredEntitiesProperty); }
            set { SetValue(FilteredEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilteredEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredEntitiesProperty = RegisterProperty("FilteredEntities", typeof(ObservableCollection<FilterableEntity>));

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
        /// Gets or sets the property value.
        /// </summary>
        public bool IsAddingNewEdge
        {
            get
            {
                return GetValue<bool>(IsAddingNewEdgeProperty);
            }
            set
            {
                SetValue(IsAddingNewEdgeProperty, value);
            }
        }

        /// <summary>
        /// Register the IsAddingNewEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsAddingNewEdgeProperty = RegisterProperty("IsAddingNewEdge", typeof(bool), () => false);

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), () => false);


        public void NavigateTo(DataVertex dataVertex)
        {
            if(GraphExplorer != null)
            {
                GraphExplorer.NavigateTo(dataVertex); 
              } 
        }

        public GraphExplorerViewModel GraphExplorer {
            get { return ParentViewModel as GraphExplorerViewModel; }
        }
    }
}