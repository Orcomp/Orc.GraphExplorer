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
    using GraphX;
    using GraphX.Controls.Models;

    public class CustomGraphControlFactory : IGraphControlFactory
    {
        public EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            return new EdgeControl(source, target, edge, showLabels, showArrows) {Visibility = visibility};
        }

        public VertexControl CreateVertexControl(object vertexData)
        {
            return new VertexControl(vertexData);
        }

        public GraphAreaBase FactoryRootArea { get; set; }
    }
}