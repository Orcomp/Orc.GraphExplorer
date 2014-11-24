#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Explorer.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Linq;
    using Behaviors;

    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Memento;
    using Catel.Services;
    using Csv.Services;
    using Data;
    using GraphX.GraphSharp;
    using Messages;
    using Services;

    public class Explorer : ModelBase, IGraphNavigator
    {

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset EditorToolset { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset NavigatorToolset { get; set; }

        public void NavigateTo(DataVertex dataVertex)
        {
            Argument.IsNotNull(() => dataVertex);

            var loadingService = this.GetServiceLocator().ResolveType<IGraphAreaLoadingService>();

            var navigatorArea = NavigatorToolset.Area;
            ((IGraphNavigator)navigatorArea.GraphDataGetter).NavigateTo(dataVertex);

            loadingService.ReloadGraphArea(navigatorArea, 0);

            loadingService.TryRefresh(navigatorArea);
        }

    }
}