#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphToolset.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Catel.Data;
    using Catel.Memento;
    using Data;
    using GraphX;
    using Microsoft.Win32;

    using Orc.GraphExplorer.Messages;

    public class GraphToolset : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of toolset
        /// </summary>
        public string ToolsetName { get; set; }

        public GraphToolset(string toolsetName, bool isFilterEnabled, IMementoService mementoService)
        {            
            ToolsetName = toolsetName;
            Area = new GraphArea(ToolsetName, mementoService);
            Filter = new Filter(Area.Logic) {IsFilterEnabled = isFilterEnabled};
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphArea Area
        {
            get { return GetValue<GraphArea>(EditorAreaProperty); }
            set { SetValue(EditorAreaProperty, value); }
        }

        /// <summary>
        /// Register the Area property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorAreaProperty = RegisterProperty("Area", typeof(GraphArea), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Filter Filter
        {
            get { return GetValue<Filter>(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Register the Filter property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterProperty = RegisterProperty("Filter", typeof(Filter), null);

        public void SaveToXml()
        {
            var dlg = new SaveFileDialog { Filter = "All files|*.xml", Title = "Select layout file name", FileName = "overrall_layout.xml" };
            if (dlg.ShowDialog() == true)
            {
                SaveToXmlMessage.SendWith(dlg.FileName, ToolsetName);
            }
        }

        public void LoadFromXml()
        {
            var dlg = new OpenFileDialog { Filter = "All files|*.xml", Title = "Select layout file", FileName = "overrall_layout.xml" };
            if (dlg.ShowDialog() == true)
            {
                LoadFromXmlMessage.SendWith(dlg.FileName, ToolsetName);
            }
        }

        public void SaveToImage()
        {
            SaveToImageMessage.SendWith(ImageType.PNG, ToolsetName);
        }
    }
}