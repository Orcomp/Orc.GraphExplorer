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
    using Catel.MVVM.Converters;
    using Catel.MVVM.Views;
    using GraphX;

    using Orc.GraphExplorer.ViewModels;

    public class EdgeView : EdgeViewBase
    {
        public EdgeView(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true) : base(source, target, edge, showLabels, showArrows)
        {
        }

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

        public EdgeViewModel ViewModel {
            get
            {
                return ViewModel as EdgeViewModel;
            }
        }
    }
}