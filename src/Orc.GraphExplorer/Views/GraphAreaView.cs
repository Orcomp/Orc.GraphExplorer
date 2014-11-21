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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views.Base;

    public class GraphAreaView : GraphAreaViewBase
    {
        public GraphAreaView()
        {
            Loaded += GraphAreaView_Loaded;
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel == null)
            {
                return;
            }

            SaveToXmlMessage.Register(this, OnSaveToXmlMessage, ViewModel.ToolsetName);
            LoadFromXmlMessage.Register(this, OnLoadFromXmlMessage, ViewModel.ToolsetName);
            SaveToImageMessage.Register(this, OnSaveToImageMessage, ViewModel.ToolsetName);
            
            ReadyToLoadGraphMessage.SendWith(ViewModel.ToolsetName);            
        }

        void GraphAreaView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;

            var relationalViewModel = viewModel as IRelationalViewModel;

            var toolset = this.FindFirstParentOfType<GraphToolsetView>();
            if (toolset != null && relationalViewModel != null && viewModel.ToolSetViewModel == null)
            {
                relationalViewModel.SetParentViewModel(toolset.ViewModel);
            }

            RelayoutGraph();
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