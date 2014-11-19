#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphNavigationBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows.Interactivity;
    using Catel.Windows.Interactivity;
    using Models.Data;
    using Views;
    using Views.Base;

    public class GraphNavigationBehavior : BehaviorBase<GraphAreaViewBase>
    {
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            _navigator = AssociatedObject.ViewModel as IGraphNavigator;
            _navigationController = AssociatedObject.ViewModel as IGraphNavigationController;

            if (_navigator == null || _navigationController == null)
            {
                return;
            }

            AssociatedObject.VertexDoubleClick += AssociatedObject_VertexDoubleClick;
        }

        private IGraphNavigator _navigator;
        private IGraphNavigationController _navigationController;

        void AssociatedObject_VertexDoubleClick(object sender, GraphX.Models.VertexSelectedEventArgs args)
        {
            var vertexView = args.VertexControl as VertexView;
            if (vertexView == null)
            {
                return;
            }
            if (_navigationController.CanNavigate)
            {
                _navigator.NavigateTo(vertexView.ViewModel.DataVertex);
            }
        }
    }
}