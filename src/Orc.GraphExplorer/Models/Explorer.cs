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
        public Explorer(IMementoService mementoService, IConfigLocationService configLocationService, IMessageService messageService)
        {
            EditorToolset = new GraphToolset("Editor", true, mementoService, messageService);
            NavigatorToolset = new GraphToolset("Navigator", false, mementoService, messageService);

            Settings = new Settings(configLocationService);

            ReadyToLoadGraphMessage.Register(this, OnReadyToLoadGraphMessage);
        }

        private void OnReadyToLoadGraphMessage(ReadyToLoadGraphMessage message)
        {
            var editorArea = EditorToolset.Area;
            if (string.Equals(message.Data, "Editor") && editorArea.GraphDataGetter == null)
            {
                var graphDataService = new CsvGraphDataService();
                editorArea.GraphDataGetter = graphDataService;
                editorArea.GraphDataSaver = graphDataService;
            }

            var navigatorArea = NavigatorToolset.Area;
            if (string.Equals(message.Data, "Navigator") && navigatorArea.GraphDataGetter == null)
            {
                navigatorArea.GraphDataGetter = new NavigatorGraphDataGetter(editorArea.Logic.Graph);
            }
            
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Settings Settings
        {
            get { return GetValue<Settings>(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        /// <summary>
        /// Register the Settings property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SettingsProperty = RegisterProperty("Settings", typeof(Settings)   );

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset EditorToolset
        {
            get { return GetValue<GraphToolset>(EditorToolsetProperty); }
            set { SetValue(EditorToolsetProperty, value); }
        }

        /// <summary>
        /// Register the EditorToolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorToolsetProperty = RegisterProperty("EditorToolset", typeof(GraphToolset), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset NavigatorToolset
        {
            get { return GetValue<GraphToolset>(NavigatorToolsetProperty); }
            set { SetValue(NavigatorToolsetProperty, value); }
        }

        /// <summary>
        /// Register the NavigatorToolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NavigatorToolsetProperty = RegisterProperty("NavigatorToolset", typeof(GraphToolset), null);

        public void NavigateTo(DataVertex dataVertex)
        {
            Argument.IsNotNull(() => dataVertex);

            var navigatorArea = NavigatorToolset.Area;
            ((IGraphNavigator)navigatorArea.GraphDataGetter).NavigateTo(dataVertex);

            navigatorArea.ReloadGraphArea(0);

            NavigatorToolset.Refresh();
        }

    }
}