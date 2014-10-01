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
    using System.Windows;

    using Catel.Data;
    using GraphX;
    using Microsoft.Win32;

    using Orc.GraphExplorer.Messages;

    public class GraphToolset : ModelBase
    {
        public GraphToolset()
        {
            Area = new GraphArea();
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

        public void SaveToXml()
        {
            var dlg = new SaveFileDialog { Filter = "All files|*.xml", Title = "Select layout file name", FileName = "overrall_layout.xml" };
            if (dlg.ShowDialog() == true)
            {
                SaveToXmlMessage.SendWith(dlg.FileName);
            }
        }

        public void LoadFromXml()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "All files|*.xml",
                Title = "Select layout file",
                FileName = "overrall_layout.xml"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    LoadFromXmlMessage.SendWith(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Failed to load layout file:\n {0}", ex));
                }
            }
        }

        public void SaveToImage()
        {
            SaveToImageMessage.SendWith(ImageType.PNG);
        }
    }
}