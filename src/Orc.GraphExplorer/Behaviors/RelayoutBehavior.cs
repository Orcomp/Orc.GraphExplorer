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
    using System.Threading;
    using System.Windows.Interactivity;
    using Catel.Windows.Interactivity;
    using GraphX.Controls;

    using Views;
    using Views.Base;

    public class RelayoutBehavior : BehaviorBase<GraphAreaViewBase>
    {
        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
            AssociatedObject.GenerateGraphFinished += AssociatedObject_GenerateGraphFinished;
            ResumeRelayout();
        }

        private void AssociatedObject_GenerateGraphFinished(object sender, EventArgs e)
        {
            ResumeRelayout();
        }

        private void ResumeRelayout()
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