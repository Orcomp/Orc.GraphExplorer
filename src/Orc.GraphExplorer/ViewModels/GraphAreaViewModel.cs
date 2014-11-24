#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using Catel;
    using Catel.Fody;
    using Catel.MVVM;

    using Orc.GraphExplorer.Behaviors;
    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services;

    public class GraphAreaViewModel : ViewModelBase, IDropable, IGraphNavigator, IGraphNavigationController, IFilterable, IGraphLogicProvider, IEdgeDrawer
    {
        #region Fields
        private readonly IViewModelManager _viewModelManager;

        private readonly IGraphAreaEditorService _graphAreaEditorService;

        private readonly IEdgeDrawingService _edgeDrawingService;

        private readonly IGraphAreaLoadingService _graphAreaLoadingService;

        #endregion

        #region Constructors
        public GraphAreaViewModel()
        {
        }

        public GraphAreaViewModel(GraphArea area, IViewModelManager viewModelManager, IGraphAreaEditorService graphAreaEditorService, IEdgeDrawingService edgeDrawingService, IGraphAreaLoadingService graphAreaLoadingService)
        {
            _viewModelManager = viewModelManager;
            _graphAreaEditorService = graphAreaEditorService;
            _edgeDrawingService = edgeDrawingService;
            _graphAreaLoadingService = graphAreaLoadingService;
            Area = area;

            SettingsChangedMessage.Register(this, OnSettingsChangedMessage);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [Expose("GraphDataSaver")]
        public GraphArea Area { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public IGraphDataGetter GraphDataGetter { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public string ToolsetName { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsDragEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsInEditing { get; set; }

        public GraphToolsetViewModel ToolSetViewModel
        {
            get
            {
                return ParentViewModel as GraphToolsetViewModel;
            }
        }
        #endregion

        #region IDropable Members
        public Type DataTypeFormat
        {
            get
            {
                return typeof(DataVertex);
            }
        }

        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        public void Drop(IDataObject dataObject, Point position)
        {
            Argument.IsNotNull(() => dataObject);

            _graphAreaEditorService.AddVertex(Area, (DataVertex)dataObject.GetData(typeof(DataVertex)), position);
        }
        #endregion

        #region IEdgeDrawer Members
        public bool TryStartEdgeDrawing(DataVertex startVertex, Point startPoint, Point lastPoint)
        {
            Argument.IsNotNull(() => startVertex);

            if (!IsInEditing || !ToolSetViewModel.IsAddingNewEdge || _edgeDrawingService.IsInDrawing())
            {
                return false;
            }

            _edgeDrawingService.StartEdgeDrawing(Logic.Graph, startVertex, startPoint, lastPoint);
            return true;
        }

        public bool TryFinishEdgeDrawing(DataVertex endVertex)
        {
            Argument.IsNotNull(() => endVertex);

            if (!_edgeDrawingService.IsInDrawing() || _edgeDrawingService.IsStartVertex(endVertex))
            {
                return false;
            }

            var startVertex = _edgeDrawingService.GetStartVertex();
            _graphAreaEditorService.AddEdge(Area, startVertex, endVertex);

            _edgeDrawingService.FinishEdgeDrawing(endVertex);

            ToolSetViewModel.IsAddingNewEdge = false;

            return true;
        }

        public void MoveBrush(Point point)
        {
            if (!ToolSetViewModel.IsAddingNewEdge)
            {
                return;
            }

            _edgeDrawingService.MoveBrush(point);
        }

        public PathGeometry GetEdgeGeometry()
        {
            return _edgeDrawingService.GetEdgeGeometry();
        }
        #endregion

        #region IFilterable Members
        public void UpdateFilterSource()
        {
            if (ToolSetViewModel != null)
            {
                var filter = ToolSetViewModel.Toolset.Filter;
                filter.ChangeFilterSource(filter.GraphLogic.Graph.Vertices);
            }
        }
        #endregion

        #region IGraphLogicProvider Members
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public GraphLogic Logic { get; set; }
        #endregion

        #region IGraphNavigationController Members
        public bool CanNavigate
        {
            get
            {
                return !IsInEditing;
            }
        }
        #endregion

        #region IGraphNavigator Members
        public void NavigateTo(DataVertex dataVertex)
        {
            Argument.IsNotNull(() => dataVertex);

            ToolSetViewModel.NavigateTo(dataVertex);
        }
        #endregion

        #region Methods
        private void OnSettingsChangedMessage(SettingsChangedMessage settingsChangedMessage)
        {
            _graphAreaLoadingService.TryRefresh(Area);
        }

        /// <summary>
        /// Called when the GraphDataGetter property has changed.
        /// </summary>
        private void OnGraphDataGetterChanged()
        {
            if (GraphDataGetter == null)
            {
                return;
            }
            _graphAreaLoadingService.ReloadGraphArea(Area, 600);
        }

        /// <summary>
        /// Called when the IsDragEnabled property has changed.
        /// </summary>
        private void OnIsDragEnabledChanged()
        {
            var vertexViewModels = _viewModelManager.GetChildViewModels(this).OfType<VertexViewModel>();
            foreach (var vertex in vertexViewModels)
            {
                vertex.IsDragEnabled = IsDragEnabled;
            }
        }

        /// <summary>
        /// Called when the IsInEditing property has changed.
        /// </summary>
        private async void OnIsInEditingChanged()
        {
            UpdateGraphItemsEditingState();

            if (IsInEditing)
            {
                StatusMessage.SendWith("Edit Mode");
            }
            else if (!await _graphAreaEditorService.TryExitEditMode(Area))
            {
                return;
            }

            EditingStartStopMessage.SendWith(IsInEditing, ToolsetName);
        }

        private void UpdateGraphItemsEditingState()
        {
            var vertexViewModels = _viewModelManager.GetChildViewModels(this).OfType<VertexViewModel>();
            foreach (var vertex in vertexViewModels)
            {
                vertex.IsInEditing = IsInEditing;
            }

            var edgeViewModels = _viewModelManager.GetChildViewModels(this).OfType<EdgeViewModel>();
            foreach (var edge in edgeViewModels)
            {
                edge.IsInEditing = IsInEditing;
            }
        }
        #endregion
    }
}