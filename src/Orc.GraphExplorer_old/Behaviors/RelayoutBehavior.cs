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
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interactivity;
    using Catel.MVVM.Converters;
    using GraphX.Controls;
    using ObjectModel;
    using Views;
    using Views.Enums;
    using IValueConverter = Catel.MVVM.Converters.IValueConverter;

    public class RelayoutBehavior : Behavior<GraphArea>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GenerateGraphFinished += AssociatedObject_GenerateGraphFinished;
        }

        void AssociatedObject_GenerateGraphFinished(object sender, System.EventArgs e)
        {
            ShowAllEdgesLabels(true);

            FitToBounds();

            SetVertexPropertiesBinding();
        }

        public void ShowAllEdgesLabels(bool show)
        {
            AssociatedObject.ShowAllEdgesLabels(show);
            AssociatedObject.InvalidateVisual();
        }

        public void FitToBounds()
        {
            ZoomControl zoom = (ZoomView)AssociatedObject.Parent;
            zoom.ZoomToFill();
            zoom.Mode = ZoomControlModes.Custom;
        }        

        //Summary
        //    binding in style will be overrided in graph control, so need create binding after data loaded
        public void SetVertexPropertiesBinding()
        {
            IValueConverter conv = new BooleanToHidingVisibilityConverter();

            foreach (var vertex in AssociatedObject.VertexList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = vertex.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = vertex.Key, Mode = BindingMode.TwoWay };

                vertex.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                vertex.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }

            foreach (var edge in AssociatedObject.EdgesList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = edge.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = edge.Key, Mode = BindingMode.TwoWay };

                edge.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                edge.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }
        }
    }
}