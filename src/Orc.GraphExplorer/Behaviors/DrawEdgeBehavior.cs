#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows.Input;

    using Catel.Windows;
    using Catel.Windows.Interactivity;
    using GraphX.Controls;
    using GraphX.Models;
    using Models;

    using Orc.GraphExplorer.Views.Base;

    using Views;

    public class DrawEdgeBehavior : BehaviorBase<GraphAreaViewBase>
    {
        #region Fields
        private EdgeView _edge;
        private ZoomControl _zoomControl;
        private IEdgeDrawer _edgeDrawer;
        #endregion

        #region Properties
        private ZoomControl ZoomControl
        {
            get
            {
                if (_zoomControl == null)
                {
                    _zoomControl = AssociatedObject.FindLogicalOrVisualAncestorByType<ZoomControl>();
                }

                return _zoomControl;
            }
        }
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
            ZoomControl.PreviewMouseMove += ZoomControl_PreviewMouseMove;
            _edgeDrawer = AssociatedObject.ViewModel as IEdgeDrawer;
        }

        private void ZoomControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_edge == null)
            {
                return;
            }

            var point = ZoomControl.TranslatePoint(e.GetPosition(ZoomControl), AssociatedObject);
            _edgeDrawer.MoveBrush(point);
            _edge.SetEdgePathManually(_edgeDrawer.GetEdgeGeometry());
        }

        private void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeViewCreatedEventArgs e)
        {
            _edge = e.EdgeViewBase as EdgeView;
            if (_edge != null)
            {
                _edge.SetEdgePathManually(_edgeDrawer.GetEdgeGeometry());
            }
        }

        private void AssociatedObject_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            var vertex = (DataVertex) args.VertexControl.Vertex;
            var startPoint = args.VertexControl.GetPosition();
            var lastPoint = ZoomControl.TranslatePoint(startPoint, AssociatedObject);

            if (_edgeDrawer.TryStartEdgeDrawing(vertex, startPoint, lastPoint))
            {
                return;
            }

            if (_edgeDrawer.TryFinishEdgeDrawing(vertex))
            {
                _edge = null;
            }
        }
        #endregion
    }
}