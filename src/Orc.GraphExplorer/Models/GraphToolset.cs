﻿#region Copyright (c) 2014 Orcomp development team.
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
    using System.Threading.Tasks;
    using System.Windows;

    using Catel;
    using Catel.Data;
    using Catel.Memento;
    using Catel.Services;
    using Data;
    using GraphX;
    using Microsoft.Win32;

    using Orc.GraphExplorer.Messages;

    public class GraphToolset : ModelBase
    {
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Gets or sets the name of toolset
        /// </summary>
        public string ToolsetName { get; set; }

        public GraphToolset(string toolsetName, bool isFilterEnabled, IMementoService mementoService, IMessageService messageService)
        {
            Argument.IsNotNullOrEmpty(() => toolsetName);

            _mementoService = mementoService;
            _messageService = messageService;
            ToolsetName = toolsetName;
            Area = new GraphArea(toolsetName);
            Filter = new Filter(Area.Logic) {IsFilterEnabled = isFilterEnabled};

            SettingsChangedMessage.Register(this, OnSettingsChangedMessage);
        }

        private void OnSettingsChangedMessage(SettingsChangedMessage settingsChangedMessage)
        {
            Refresh();
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanRedo
        {
            get { return _mementoService.CanRedo; }

        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphArea Area { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Filter Filter { get; set; }

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

        public async Task Refresh()
        {
            var area = Area;

            if (area.IsInEditing && _mementoService.CanUndo)
            {
                var messageResult = await _messageService.Show("Refresh view in edit mode will discard changes you made, will you want to continue?", "Confirmation", MessageButton.YesNo);
                if (messageResult == MessageResult.Yes)
                {
                    _mementoService.Clear();
                    area.IsInEditing = false;
                    area.IsDragEnabled = false;
                    area.ReloadGraphArea(600);
                    StatusMessage.SendWith("Graph Refreshed");
                }
            }
            else
            {
                area.ReloadGraphArea(600);
            }
        }

        public void Undo()
        {
            _mementoService.Undo();
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }

        public void Redo()
        {
            _mementoService.Redo();
            GraphChangedMessage.SendWith(_mementoService.CanUndo);
        }
    }
}