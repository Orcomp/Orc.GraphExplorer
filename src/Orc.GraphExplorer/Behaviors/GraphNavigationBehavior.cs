#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphNavigationBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using Catel.Windows.Interactivity;
    using Views;
    using Views.Base;

    public class GraphNavigationBehavior : BehaviorBase<GraphAreaViewBase>
    {
        #region Fields
        private IGraphNavigator _navigator;
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            _navigator = AssociatedObject.ViewModel as IGraphNavigator;

            if (_navigator == null)
            {
                return;
            }

            AssociatedObject.VertexDoubleClick += AssociatedObject_VertexDoubleClick;
        }

        private void AssociatedObject_VertexDoubleClick(object sender, GraphX.Models.VertexSelectedEventArgs args)
        {
            var vertexView = args.VertexControl as VertexView;
            if (vertexView == null)
            {
                return;
            }

            _navigator.NavigateTo(vertexView.ViewModel.DataVertex);
        }
        #endregion
    }
}