#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM.Views;
    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views.Base;
    using Services.Interfaces;

    public class GraphAreaView : GraphAreaViewBase
    {
        public GraphAreaView()
        {
            ViewModelChanged += GraphAreaView_ViewModelChanged;
        }

        void GraphAreaView_ViewModelChanged(object sender, EventArgs e)
        {
            if (ViewModel == null)
            {
                return;
            }

            SaveToXmlMessage.Register(this, OnSaveToXmlMessage, ViewModel.ToolsetName);
            LoadFromXmlMessage.Register(this, OnLoadFromXmlMessage, ViewModel.ToolsetName);
            SaveToImageMessage.Register(this, OnSaveToImageMessage, ViewModel.ToolsetName);
        }

        private void OnSaveToXmlMessage(SaveToXmlMessage message)
        {
            SaveIntoFile(message.Data);
        }

        private void OnSaveToImageMessage(SaveToImageMessage message)
        {
            ExportAsImage(message.Data);
        }

        private void OnLoadFromXmlMessage(LoadFromXmlMessage message)
        {
            // TODO: test and fix it
            ClearLayout();
            LoadFromFile(message.Data);
            RelayoutGraph();
        }

        public new GraphAreaViewModel ViewModel {
            get
            {               
                return base.ViewModel as GraphAreaViewModel;
            }
        }

        /// <summary>
        /// IsDragEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsDragEnabledProperty =
            DependencyProperty.Register("IsDragEnabled", typeof(bool), typeof(GraphAreaView),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Gets or sets the IsDragEnabled property.
        /// </summary>
        [ViewToViewModel]
        public bool IsDragEnabled
        {
            get { return (bool)GetValue(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }
    }
}