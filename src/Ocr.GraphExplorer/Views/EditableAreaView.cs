#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableAreaView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.IoC;
    using Orc.GraphExplorer.Views.Base;
    using Services.Interfaces;

    public class EditableAreaView : GraphAreaViewBase
    {
        public EditableAreaView()
        {
            Loaded += EditableAreaView_Loaded;
        }

        void EditableAreaView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {            
            CreateGraphArea(600);
        }

        protected override IGraphDataService GetGraphDataService()
        {
            return ServiceLocator.Default.ResolveType<IGraphDataService>(GraphExplorerSection.Current.DefaultGraphDataService);
        }
    }
}