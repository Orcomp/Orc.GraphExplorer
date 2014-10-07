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
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using GraphX;
    using GraphX.Controls.Models;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Views;
    using Orc.GraphExplorer.Views.Base;
    using ViewModels;

    public class CustomGraphControlFactory : IGraphControlFactory
    {
        public EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            var edgeControl = new EdgeView(source, target, edge, showLabels, showArrows) {Visibility = visibility, RootArea = FactoryRootArea};
            return edgeControl;
        }

        public VertexControl CreateVertexControl(object vertexData)
        {            
            var vertexControl = new VertexView(vertexData) {RootArea = FactoryRootArea, };
            return vertexControl;
        }

        public GraphAreaBase FactoryRootArea { get; set; }
    }
}