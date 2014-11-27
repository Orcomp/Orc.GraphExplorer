#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Views
{
    using System.Windows;
    using Base;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Windows;
    using GraphX;

    public class EdgeView : EdgeViewBase
    {
        #region Constructors
        public EdgeView(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true) : base(source, target, edge, showLabels, showArrows)
        {
            Loaded += EdgeView_Loaded;
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
        #endregion

        #region Methods
        private void EdgeView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var relationalViewModel = ViewModel as IRelationalViewModel;
            var graphAreaView = this.FindLogicalOrVisualAncestorByType<GraphAreaView>();
            if (graphAreaView != null && relationalViewModel != null && ViewModel.ParentViewModel == null)
            {
                relationalViewModel.SetParentViewModel(graphAreaView.ViewModel);
            }
        }
        #endregion
    }
}