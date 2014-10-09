#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextMenuBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using Orc.GraphExplorer.Views;

    public class ContextMenuBehavior : Behavior<GraphAreaView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.EdgeSelected += AssociatedObject_EdgeSelected;
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
        }

        void AssociatedObject_VertexSelected(object sender, GraphX.Models.VertexSelectedEventArgs args)
        {
            if (AssociatedObject.ViewModel.IsInEditing)
            {
                //args.VertexControl.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                //args.VertexControl.ContextMenu.Visibility = Visibility.Hidden;
            }
        }

        void miDeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        void AssociatedObject_EdgeSelected(object sender, GraphX.Models.EdgeSelectedEventArgs args)
        {
         //   throw new System.NotImplementedException();
        }
    }
}