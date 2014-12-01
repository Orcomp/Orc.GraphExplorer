#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Views
{
    using System.Windows;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Windows;
    using GraphX;
    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для VertexView.xaml
    /// </summary>
    public partial class VertexView
    {
        #region Constructors
        public VertexView(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true)
            : base(vertexData, tracePositionChange, bindToDataObject)
        {
            Loaded += VertexView_Loaded;
        }
        #endregion

        #region Properties
        [ViewToViewModel]
        public bool IsHighlightEnabled
        {
            get { return HighlightBehaviour.GetIsHighlightEnabled(this); }
            set { HighlightBehaviour.SetIsHighlightEnabled(this, value); }
        }

        [ViewToViewModel]
        public bool IsHighlighted
        {
            get { return HighlightBehaviour.GetHighlighted(this); }
            set { HighlightBehaviour.SetHighlighted(this, value); }
        }

        [ViewToViewModel]
        public bool IsDragEnabled
        {
            get { return DragBehaviour.GetIsDragEnabled(this); }
            set { DragBehaviour.SetIsDragEnabled(this, value); }
        }

        [ViewToViewModel]
        public new bool IsVisible
        {
            get { return base.IsVisible; }
            set { base.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        [ViewToViewModel]
        public new bool IsEnabled
        {
            get { return base.IsEnabled; }
            set { base.IsEnabled = value; }
        }

        public new VertexViewModel ViewModel
        {
            get { return base.ViewModel; }
        }
        #endregion

        #region Methods
        private void VertexView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;

            var relationalViewModel = viewModel as IRelationalViewModel;
            var graphAreaView = this.FindLogicalOrVisualAncestorByType<GraphAreaView>();
            if (graphAreaView != null && relationalViewModel != null && viewModel.ParentViewModel == null)
            {
                relationalViewModel.SetParentViewModel(graphAreaView.ViewModel);
            }
        }
        #endregion
    }
}