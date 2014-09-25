#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomGraphControlFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Windows;
    using System.Windows.Controls;

    using GraphX;
    using GraphX.Controls.Models;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Views;

    public class CustomGraphControlFactory : IGraphControlFactory
    {
        public EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            var edgeControl = new EdgeView(source, target, edge, showLabels, showArrows) {Visibility = visibility};
            edgeControl.Loaded += (sender, args) => edgeControl.ViewModel.Data = (DataEdge)edge;
            return edgeControl;
        }

        public VertexControl CreateVertexControl(object vertexData)
        {
            var vertexControl = new VertexView(vertexData);
            vertexControl.Loaded += (sender, args) => vertexControl.ViewModel.Data = (DataVertex)vertexData;
            return vertexControl;
        }

        public GraphAreaBase FactoryRootArea { get; set; }
    }
}