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
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using Behaviors.Interfaces;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Memento;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public class GraphAreaViewModel : ViewModelBase, IDropable, IGraphNavigator, IGraphNavigationController, IFilterable/*, IEdgeDrwingCanvas*/
    {
        public GraphAreaViewModel()
        {
            
        }


        public GraphAreaViewModel(GraphArea area)
        {
            Area = area;
        }

        
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public GraphArea Area
        {
            get { return GetValue<GraphArea>(EditorProperty); }
            private set { SetValue(EditorProperty, value); }
        }

        /// <summary>
        /// Register the Area property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorProperty = RegisterProperty("Area", typeof(GraphArea));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public GraphLogic Logic
        {
            get { return GetValue<GraphLogic>(LogicProperty); }
            set { SetValue(LogicProperty, value); }
        }

        /// <summary>
        /// Register the Logic property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LogicProperty = RegisterProperty("Logic", typeof(GraphLogic));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public string ToolsetName
        {
            get { return GetValue<string>(ToolsetNameProperty); }
            set { SetValue(ToolsetNameProperty, value); }
        }

        /// <summary>
        /// Register the ToolsetName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ToolsetNameProperty = RegisterProperty("ToolsetName", typeof(string));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsDragEnabled
        {
            get { return GetValue<bool>(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsDragEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDragEnabledProperty = RegisterProperty("IsDragEnabled", typeof(bool), null, (sender, e) => ((GraphAreaViewModel)sender).OnIsDragEnabledChanged());

        /// <summary>
        /// Called when the IsDragEnabled property has changed.
        /// </summary>
        private void OnIsDragEnabledChanged()
        {
            var vertexViewModels = ServiceLocator.Default.ResolveType<IViewModelManager>().GetChildViewModels(this).OfType<VertexViewModel>();
            foreach (var vertex in vertexViewModels)
            {
                vertex.IsDragEnabled = IsDragEnabled;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), null, (sender, e) => ((GraphAreaViewModel)sender).OnIsInEditingChanged());

        /// <summary>
        /// Called when the IsInEditing property has changed.
        /// </summary>
        private void OnIsInEditingChanged()
        {            
            var vertexViewModels = ServiceLocator.Default.ResolveType<IViewModelManager>().GetChildViewModels(this).OfType<VertexViewModel>();
            foreach (var vertex in vertexViewModels)
            {
                vertex.IsInEditing = IsInEditing;
            }

            var edgeViewModels = ServiceLocator.Default.ResolveType<IViewModelManager>().GetChildViewModels(this).OfType<EdgeViewModel>();
            foreach (var edge in edgeViewModels)
            {
                edge.IsInEditing = IsInEditing;
            }
        }

        public Type DataTypeFormat {
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
            Area.AddVertex((DataVertex)dataObject.GetData(typeof(DataVertex)), position);
        }

        public GraphToolsetViewModel ToolSetViewModel
        {
            get
            {
                return ParentViewModel as GraphToolsetViewModel;
            }
        }

        public void RemoveEdge(DataEdge dataEdge)
        {
            Area.RemoveEdge(dataEdge);
        }

        public void RemoveVertex(DataVertex dataVertex)
        {
            Area.RemoveVertex(dataVertex);
        }

        public void NavigateTo(DataVertex dataVertex)
        {
            ToolSetViewModel.NavigateTo(dataVertex);
        }

        public bool CanNavigate { get { return !IsInEditing; } }

        public void UpdateFilterSource()
        {
            if (ToolSetViewModel != null)
            {
                ToolSetViewModel.Toolset.Filter.UpdateFilterSource();
            }
        }
    }
}