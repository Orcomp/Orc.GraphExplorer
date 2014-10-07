#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaViewBase.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Views.Base
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Windows;
    using Events;
    using Fasterflect;
    using GraphX;
    using GraphX.Controls;
    using GraphX.Controls.Models;
    using Models;
    using Models.Data;
    using Services;
    using Services.Interfaces;

    public abstract class GraphAreaViewBase : GraphArea<DataVertex, DataEdge, Graph>, IUserControl
    {
        #region Constants
        /// <summary>
        /// Content Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof (object), typeof (GraphAreaViewBase), new FrameworkPropertyMetadata((object) null));
        #endregion

        #region Fields
        private UserControlLogic _logic;
        #endregion

        #region Constructors
        public GraphAreaViewBase()
        {
            // TODO: try to inject IGraphControlFactory
            IServiceLocator serviceLocator = ServiceLocator.Default;
            ControlFactory = serviceLocator.ResolveType<IGraphControlFactory>();
            ControlFactory.FactoryRootArea = this;

            _logic = new UserControlLogic(this);            
        }

        public override void BeginInit()
        {
            _logic.ViewModelChanged += (sender, args) => ViewModelChanged.SafeInvoke(this);
            _logic.Loaded += (sender, args) => _viewLoaded.SafeInvoke(this);
            _logic.Unloaded += (sender, args) => _viewUnloaded.SafeInvoke(this);

            _logic.PropertyChanged += (sender, args) => _propertyChanged.SafeInvoke(this, args);

            this.AddDataContextChangedHandler((sender, e) => _viewDataContextChanged.SafeInvoke(this, EventArgs.Empty));

            ViewModelChanged += GraphAreaViewBase_ViewModelChanged;

            base.BeginInit();
        }

        void GraphAreaViewBase_GraphReloaded(object sender, GraphEventArgs e)
        {            
            GenerateGraph(e.Graph, true);
            SubscribeOnGraphEvents();
        }

        void GraphAreaViewBase_BeforeReloadingGraph(object sender, EventArgs e)
        {
            if (LogicCore.Graph != null)
            {
                UnSubscribeOnGraphEvents();
            }

            ClearLayout();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the CustomContent property.
        /// </summary>
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region IUserControl Members
        public IViewModel ViewModel
        {
            get { return _logic.ViewModel; }
        }

        public event EventHandler<EventArgs> ViewModelChanged;

        event EventHandler<EventArgs> IView.Loaded
        {
            add { _viewLoaded += value; }
            remove { _viewLoaded -= value; }
        }

        event EventHandler<EventArgs> IView.Unloaded
        {
            add { _viewUnloaded += value; }
            remove { _viewUnloaded -= value; }
        }

        event EventHandler<EventArgs> IView.DataContextChanged
        {
            add { _viewDataContextChanged += value; }
            remove { _viewDataContextChanged -= value; }
        }

        public bool CloseViewModelOnUnloaded
        {
            get { return _logic.CloseViewModelOnUnloaded; }
            set { _logic.CloseViewModelOnUnloaded = value; }
        }

        public bool SupportParentViewModelContainers
        {
            get { return _logic.SupportParentViewModelContainers; }
            set { _logic.SupportParentViewModelContainers = value; }
        }

        public bool SkipSearchingForInfoBarMessageControl
        {
            get { return _logic.SkipSearchingForInfoBarMessageControl; }
            set { _logic.SkipSearchingForInfoBarMessageControl = value; }
        }

        public bool DisableWhenNoViewModel
        {
            get { return _logic.DisableWhenNoViewModel; }
            set { _logic.DisableWhenNoViewModel = value; }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        #endregion

        #region Methods

        void GraphAreaViewBase_ViewModelChanged(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                var logic = (GraphLogic) ViewModel.GetPropertyValue("Logic");
                logic.BeforeReloadingGraph += GraphAreaViewBase_BeforeReloadingGraph;
                logic.GraphReloaded += GraphAreaViewBase_GraphReloaded;
            }

       //     DataContext = ViewModel;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property != LogicCoreProperty)
            {
                return;
            }

            var oldLogic = e.OldValue as GraphLogic;
            if (oldLogic != null)
            {
                UnSubscribeOnGraphEvents(oldLogic);
            }

            SubscribeOnGraphEvents();
        }        

        private void SubscribeOnGraphEvents()
        {
            if (LogicCore == null)
            {
                return;
            }
            LogicCore.Graph.VertexAdded += GraphVertexAdded;
            LogicCore.Graph.VertexRemoved += GraphVertexRemoved;
            LogicCore.Graph.EdgeAdded += GraphEdgeAdded;
            LogicCore.Graph.EdgeRemoved += GraphEdgeRemoved;
        }

        private void UnSubscribeOnGraphEvents(GraphLogic logic = null)
        {
            if (logic == null)
            {
                logic = LogicCore as GraphLogic;
            }

            if (logic == null)
            {
                return;
            }

            logic.Graph.VertexAdded -= GraphVertexAdded;
            logic.Graph.VertexRemoved -= GraphVertexRemoved;
            logic.Graph.EdgeAdded -= GraphEdgeAdded;
            logic.Graph.EdgeRemoved -= GraphEdgeRemoved;
        }

        private void GraphEdgeRemoved(DataEdge e)
        {
            RemoveEdge(e);
        }

        private void GraphEdgeAdded(DataEdge e)
        {
            if (EdgesList.ContainsKey(e))
            {
                return;
            }
            VertexControl source = VertexList[e.Source];
            VertexControl target = DataVertex.IsFakeVertex(e.Target) ? null : VertexList[e.Target];

            var edgeView = (EdgeViewBase) ControlFactory.CreateEdgeControl(source, target, e);
            AddEdge(e, edgeView);
            edgeView.ShowArrows = true;
            edgeView.ShowLabel = true;
            edgeView.ManualDrawing = target == null;

            if (target == null && TemporaryEdgeCreated != null)
            {
                TemporaryEdgeCreated(this, new EdgeViewCreatedAventArgs(edgeView));
            }
        }

        private void GraphVertexRemoved(DataVertex vertex)
        {
            RemoveVertex(vertex);
        }

        private void GraphVertexAdded(DataVertex vertex)
        {
            if (DataVertex.IsFakeVertex(vertex))
            {
                return;
            }
            var vertexControl = (VertexView)ControlFactory.CreateVertexControl(vertex);            

            AddVertex(vertex, vertexControl);

            SafeSetPositon(vertexControl, vertex);

        }

        private void SafeSetPositon(VertexControl vertexControl, DataVertex dataVertex)
        {
            var app = Application.Current;

            if (app != null)
            {
                RunCodeInUiThread(() => SetPositon(vertexControl, dataVertex), app.Dispatcher, DispatcherPriority.Loaded);
            }
            else
            {
                SetPositon(vertexControl, dataVertex);
            }
        }

        private void SetPositon(VertexControl vertexControl, DataVertex dataVertex)
        {
            var point = (Point)dataVertex.Tag;

            if (Math.Abs(point.X - double.MinValue) > 0d)
            { SetX(vertexControl, point.X, true); }
            if (Math.Abs(point.Y - double.MinValue) > 0d)
            { SetY(vertexControl, point.Y, true); }

            dataVertex.Tag = null;
        }

        public static void RunCodeInUiThread(Action action, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
                return;

            if (dispatcher == null && Application.Current != null)
                dispatcher = Application.Current.Dispatcher;

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority);
            }
            else
            {
                action.Invoke();
            }
        }


        #endregion

        private event EventHandler<EventArgs> _viewLoaded;

        private event EventHandler<EventArgs> _viewUnloaded;

        private event EventHandler<EventArgs> _viewDataContextChanged;

        private event PropertyChangedEventHandler _propertyChanged;

        public event EventHandler<EdgeViewCreatedAventArgs> TemporaryEdgeCreated;
    }
}