#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayoutBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System;
    using System.Windows.Interactivity;
    using GraphX.Controls;

    using Orc.GraphExplorer.Behaviors.Interfaces;
    using Orc.GraphExplorer.Helpers;

    using Views;
    using Views.Base;

    public class RelayoutBehavior : Behavior<GraphAreaViewBase>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GenerateGraphFinished += AssociatedObject_GenerateGraphFinished;
        }

        private void AssociatedObject_GenerateGraphFinished(object sender, EventArgs e)
        {
            ShowAllEdgesLabels(true);
            FitToBounds();
            var filterable = AssociatedObject.ViewModel as IFilterable;
            if (filterable != null)
            {
                filterable.UpdateFilterSource();
            }
        }

        private void ShowAllEdgesLabels(bool show)
        {
            AssociatedObject.ShowAllEdgesLabels(show);
            AssociatedObject.InvalidateVisual();
        }

        private void FitToBounds()
        {
            var zoom = AssociatedObject.FindFirstParentOfType<ZoomControl>();
            zoom.ZoomToFill();
            zoom.Mode = ZoomControlModes.Custom;
        }
        #endregion
    }
}