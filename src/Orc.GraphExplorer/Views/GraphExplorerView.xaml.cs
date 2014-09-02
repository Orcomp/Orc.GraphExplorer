namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Catel.IoC;

    using GraphX;
    using GraphX.Controls;
    using GraphX.GraphSharp.Algorithms.OverlapRemoval;
    using GraphX.Models;

    using Microsoft.Win32;

    using Orc.GraphExplorer.DomainModel;
    using Orc.GraphExplorer.Enums;
    using Orc.GraphExplorer.Events;
    using Orc.GraphExplorer.Operations;
    using Orc.GraphExplorer.Services;
    using Orc.GraphExplorer.Services.Interfaces;
    using Orc.GraphExplorer.ViewModels;

    using QuickGraph;

    /// <summary>
    /// Interaction logic for GraphExplorerView.xaml
    /// </summary>
    public partial class GraphExplorerView
    {


        public GraphExplorerView()
        {
            InitializeComponent();
            
            ServiceLocator.Default.RegisterInstance(GetType(), this);
        }

    }
}