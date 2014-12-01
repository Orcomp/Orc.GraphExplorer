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
    using Catel;
    using GraphX;
    using GraphX.Controls.Models;
    using Views;

    public class CustomGraphControlFactory : IGraphControlFactory
    {
        #region IGraphControlFactory Members
        public EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            Argument.IsNotNull(() => source);
            Argument.IsNotNull(() => edge);

            var edgeControl = new EdgeView(source, target, edge, showLabels, showArrows)
            {
                Visibility = visibility, 
                RootArea = FactoryRootArea
            };
            return edgeControl;
        }

        public VertexControl CreateVertexControl(object vertexData)
        {
            Argument.IsNotNull(() => vertexData);

            var vertexControl = new VertexView(vertexData) {RootArea = FactoryRootArea,};
            return vertexControl;
        }

        public GraphAreaBase FactoryRootArea { get; set; }
        #endregion
    }
}