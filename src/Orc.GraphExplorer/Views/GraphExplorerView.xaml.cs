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
        #region Constants
        public static readonly DependencyProperty VertexesProperty = DependencyProperty.Register("Vertexes", typeof(IEnumerable<DataVertex>), typeof(GraphExplorerView), new PropertyMetadata(new List<DataVertex>(), VertexesChanged));

        public static readonly DependencyProperty EdgesProperty = DependencyProperty.Register("Edges", typeof(IEnumerable<DataEdge>), typeof(GraphExplorerView), new PropertyMetadata(null, EdgesChanged));

        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(Exception), typeof(GraphExplorerView), new PropertyMetadata(null));

        public static readonly DependencyProperty GraphDataServiceProperty = DependencyProperty.Register("GraphDataService", typeof(IGraphDataService), typeof(GraphExplorerView), new PropertyMetadata(null, GraphDataServiceChanged));

        public static readonly DependencyProperty SettingProperty = DependencyProperty.Register("Setting", typeof(GraphExplorerSetting), typeof(GraphExplorerView), new PropertyMetadata(null));
        #endregion

        #region Fields
        private readonly List<int> _selectedVertices = new List<int>();

        private PathGeometry _edGeo;

        private VertexControl _edVertex;

        private EdgeControl _edEdge;

        private DataVertex _edFakeDV;

        private GraphExplorerStatus _status;

        private Queue<NavigateHistoryItem> _navigateHistory = new Queue<NavigateHistoryItem>();

        private DataVertex _currentNavItem;
        #endregion

        //another constructor for inject IGraphDataService to graph explorer

        #region Constructors
        public GraphExplorerView(IGraphDataService graphDataService)
            : this()
        {
            //load data if graphDataService is provided
            if (graphDataService != null)
            {
                Loaded += (s, e) => { GraphDataService = graphDataService; };
            }
        }

        public GraphExplorerView()
        {
            InitializeComponent();

            ApplySetting(zoomctrl, Area.LogicCore);
            ApplySetting(zoomctrlNav, AreaNav.LogicCore, true);

            Area.VertexDoubleClick += Area_VertexDoubleClick;
            AreaNav.VertexDoubleClick += AreaNav_VertexDoubleClick;

            Area.VertexSelected += Area_VertexSelected;
            Area.EdgeSelected += Area_EdgeSelected;

            AreaNav.GenerateGraphFinished += (s, e) => Area_RelayoutFinished(s, e, zoomctrlNav);
            Area.GenerateGraphFinished += (s, e) => Area_RelayoutFinished(s, e, zoomctrl);

            Loaded += (s, e) =>
            {
                GraphDataServiceEnum defaultSvc = GraphExplorerSection.Current.DefaultGraphDataService;

                switch (defaultSvc)
                {
                    case GraphDataServiceEnum.Csv:
                        GraphDataService = new CsvGraphDataService();
                        break;
                    case GraphDataServiceEnum.Factory:
                        break;
                    default:
                        break;
                }
            };

            ServiceLocator.Default.RegisterInstance(GetType(), this);
        }
        #endregion

        #region Properties
        public GraphExplorerStatus Status
        {
            get
            {
                return _status;
            }
            private set
            {
                _status = value;
            }
        }

        public bool IsInEditMode
        {
            get
            {
                return tbtnCanEdit.IsChecked.HasValue && tbtnCanEdit.IsChecked.Value;
            }
        }

        public IEnumerable<DataVertex> Vertexes
        {
            get
            {
                return (IEnumerable<DataVertex>)GetValue(VertexesProperty);
            }
            set
            {
                SetValue(VertexesProperty, value);
            }
        }

        public IEnumerable<DataEdge> Edges
        {
            get
            {
                return (IEnumerable<DataEdge>)GetValue(EdgesProperty);
            }
            set
            {
                SetValue(EdgesProperty, value);
            }
        }

        public Exception Error
        {
            get
            {
                return (Exception)GetValue(ErrorProperty);
            }
            set
            {
                SetValue(ErrorProperty, value);
            }
        }

        public IGraphDataService GraphDataService
        {
            get
            {
                return (IGraphDataService)GetValue(GraphDataServiceProperty);
            }
            set
            {
                SetValue(GraphDataServiceProperty, value);
            }
        }

        public GraphExplorerSetting Setting
        {
            get
            {
                return (GraphExplorerSetting)GetValue(SettingProperty);
            }
            set
            {
                SetValue(SettingProperty, value);
            }
        }
        #endregion

        #region Methods
        private void Area_RelayoutFinished(object sender, EventArgs e, ZoomControl zoom)
        {
            ShowAllEdgesLabels(sender as GraphArea, true);

            FitToBounds(null, zoom);

            ((GraphExplorerViewModel)ViewModel).SetVertexPropertiesBinding();
        }

        private void ShowAllEdgesLabels(GraphArea area, bool show)
        {
            area.ShowAllEdgesLabels(show);
            area.InvalidateVisual();
        }

        private void Area_EdgeSelected(object sender, EdgeSelectedEventArgs args)
        {
            if (IsInEditMode)
            {
                args.EdgeControl.ContextMenu = new ContextMenu();
                var miDeleteLink = new MenuItem { Header = "Delete Link", Tag = args.EdgeControl };
                miDeleteLink.Click += miDeleteLink_Click;
                args.EdgeControl.ContextMenu.Items.Add(miDeleteLink);
            }
        }

        private void miDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            var eCtrl = (sender as MenuItem).Tag as EdgeControl;
            if (eCtrl != null)
            {
                var edge = eCtrl.Edge as DataEdge;

                var op = new DeleteEdgeOperation(Area, edge.Source, edge.Target, edge, ec =>
                {
                    //do nothing
                }, ec =>
                {
                    //do nothing
                });

                ((GraphExplorerViewModel)ViewModel).Do(op);
            }
            //throw new NotImplementedException();
        }

        private void Area_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                //if (DragBehaviour.GetIsDragging(args.VertexControl)) return;
                SelectVertex(args.VertexControl);

                if (IsInEditMode && _status.HasFlag(GraphExplorerStatus.CreateLinkSelectSource))
                {
                    if (_edVertex == null) //select starting vertex
                    {
                        _edVertex = args.VertexControl;
                        _edFakeDV = new DataVertex { ID = -666 };
                        _edGeo = new PathGeometry(new PathFigureCollection { new PathFigure { IsClosed = false, StartPoint = _edVertex.GetPosition(), Segments = new PathSegmentCollection { new PolyLineSegment(new List<Point> { new Point() }, true) } } });
                        Point pos = zoomctrl.TranslatePoint(args.VertexControl.GetPosition(), Area);
                        var lastseg = _edGeo.Figures[0].Segments[_edGeo.Figures[0].Segments.Count - 1] as PolyLineSegment;
                        lastseg.Points[lastseg.Points.Count - 1] = pos;

                        var dedge = new DataEdge(_edVertex.Vertex as DataVertex, _edFakeDV);
                        _edEdge = new EdgeControl(_edVertex, null, dedge) { ManualDrawing = true };
                        Area.AddEdge(dedge, _edEdge);
                        Area.LogicCore.Graph.AddVertex(_edFakeDV);
                        Area.LogicCore.Graph.AddEdge(dedge);
                        _edEdge.SetEdgePathManually(_edGeo);
                        _status = GraphExplorerStatus.CreateLinkSelectTarget;
                        ((GraphExplorerViewModel)ViewModel).PostStatusMessage("Select Target Node");
                    }
                    else if (_edVertex != args.VertexControl && _status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget)) //finish draw
                    {
                        ((GraphExplorerViewModel)ViewModel).CreateEdge((_edVertex.Vertex as DataVertex).Id, (args.VertexControl.Vertex as DataVertex).Id);

                        ClearEdgeDrawing();

                        _status = GraphExplorerStatus.Ready;

                        tbnNewEdge.IsChecked = false;
                    }
                }
            }
            else if (args.MouseArgs.RightButton == MouseButtonState.Pressed && IsInEditMode)
            {
                args.VertexControl.ContextMenu = new ContextMenu();
                var miDeleteVertex = new MenuItem { Header = "Delete", Tag = args.VertexControl };
                miDeleteVertex.Click += miDeleteVertex_Click;
                args.VertexControl.ContextMenu.Items.Add(miDeleteVertex);
            }
        }

        private void ClearEdgeDrawing()
        {
            if (_edFakeDV != null)
            {
                Area.LogicCore.Graph.RemoveVertex(_edFakeDV);
            }
            if (_edEdge != null)
            {
                var edge = _edEdge.Edge as DataEdge;
                Area.LogicCore.Graph.RemoveEdge(edge);
                Area.RemoveEdge(edge);
            }
            _edGeo = null;
            _edFakeDV = null;
            _edVertex = null;
            _edEdge = null;
        }

        private void miDeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            var vCtrl = (sender as MenuItem).Tag as VertexControl;
            if (vCtrl != null)
            {
                var op = new DeleteVertexOperation(Area, vCtrl.Vertex as DataVertex, (dv, vc) => { }, dv => { Area.RelayoutGraph(true); });

                ((GraphExplorerViewModel)ViewModel).Do(op);
            }
        }

        private void AreaNav_VertexDoubleClick(object sender, VertexSelectedEventArgs args)
        {
            //throw new NotImplementedException();
            var vertex = args.VertexControl.DataContext as DataVertex;

            if (vertex == null || vertex == _currentNavItem)
            {
                return;
            }

            _currentNavItem = vertex;

            int degree = Area.LogicCore.Graph.Degree(vertex);

            if (degree < 1)
            {
                return;
            }

            NavigateTo(vertex, Area.LogicCore.Graph);
        }

        private void Area_VertexDoubleClick(object sender, VertexSelectedEventArgs args)
        {
            if (tbtnCanEdit.IsChecked.HasValue && tbtnCanEdit.IsChecked.Value)
            {
                return;
            }

            var vertex = args.VertexControl.DataContext as DataVertex;

            if (vertex == null)
            {
                return;
            }

            _currentNavItem = vertex;

            int degree = Area.LogicCore.Graph.Degree(vertex);

            if (degree < 1)
            {
                return;
            }

            NavigateTo(vertex, Area.LogicCore.Graph);

            if (navTab.Visibility != Visibility.Visible)
            {
                navTab.Visibility = Visibility.Visible;
            }

            navTab.IsSelected = true;
        }

        private void NavigateTo(DataVertex dataVertex, BidirectionalGraph<DataVertex, DataEdge> overrallGraph)
        {
            //overrallGraph.get
            NavigateHistoryItem historyItem = GetHistoryItem(dataVertex, overrallGraph);

            CreateGraphArea(AreaNav, historyItem.Vertexes, historyItem.Edges, 0);

            //var dispatcher = AreaNav.Dispatcher;

            //FitToBounds(dispatcher, zoomctrlNav);
        }

        private void FitToBounds(Dispatcher dispatcher, ZoomControl zoom)
        {
            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(new Action(() =>
                {
                    zoom.ZoomToFill();
                    zoom.Mode = ZoomControlModes.Custom;
                    //zoom.FitToBounds();
                }), DispatcherPriority.Loaded);
            }
            else
            {
                zoom.ZoomToFill();
                zoom.Mode = ZoomControlModes.Custom;
            }
        }

        private NavigateHistoryItem GetHistoryItem(DataVertex v, BidirectionalGraph<DataVertex, DataEdge> overrallGraph)
        {
            var hisItem = new NavigateHistoryItem();

            IEnumerable<DataEdge> outs;
            IEnumerable<DataEdge> ins;

            overrallGraph.TryGetInEdges(v, out ins);

            var edges = new List<DataEdge>();

            if (overrallGraph.TryGetOutEdges(v, out outs))
            {
                edges.AddRange(outs);
            }

            if (overrallGraph.TryGetInEdges(v, out ins))
            {
                edges.AddRange(ins);
            }

            if (edges.Count > 0)
            {
                var vertexes = new List<DataVertex>();
                foreach (DataEdge e in edges)
                {
                    if (!vertexes.Contains(e.Source))
                    {
                        vertexes.Add(e.Source);
                    }

                    if (!vertexes.Contains(e.Target))
                    {
                        vertexes.Add(e.Target);
                    }
                }
                hisItem.Edges = edges;
                hisItem.Vertexes = vertexes;
            }

            return hisItem;
        }

        private void GetEdges()
        {
            GraphDataService.GetEdges(OnEdgeLoaded, OnError);
        }

        private void ApplySetting(ZoomControl zoom, IGXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>> logic, bool nav = false)
        {
            //Zoombox.SetViewFinderVisibility(zoom, System.Windows.Visibility.Visible);

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logic.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logic.DefaultOverlapRemovalAlgorithmParams = logic.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);

            if (nav)
            {
                ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 150;
                ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 100;
            }
            else
            {
                ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
                ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;
            }
            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            //Bundling algorithm will try to tie different edges that follows same direction to a single channel making complex graphs more appealing.
            logic.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logic.AsyncAlgorithmCompute = true;

            //area.UseLayoutRounding = false;
            // area.UseNativeObjectArrange = false;
        }

        private void OnVertexesLoaded(IEnumerable<DataVertex> vertexes)
        {
            Vertexes = new List<DataVertex>(vertexes);

            CreateGraphArea(Area, Vertexes, Edges, 600);

            HookVertexEvent(Area);

            ((GraphExplorerViewModel)ViewModel).OnVertexLoaded(Vertexes);
            //FitToBounds(Area.Dispatcher, zoomctrl);
        }

        private void HookVertexEvent(GraphArea Area)
        {
            foreach (var vertex in Area.VertexList)
            {
                vertex.Key.IsExpandedChanged += (s, e) => vertex_IsExpandedChanged(s, e);
            }
            //throw new NotImplementedException();
        }

        private void vertex_IsExpandedChanged(object sender, EventArgs e)
        {
            var vertex = (DataVertex)sender;

            if (vertex.IsExpanded || vertex.Properties == null || vertex.Properties.Count < 1 || !Area.VertexList.ContainsKey(vertex))
            {
                return;
            }

            VertexControl vc = Area.VertexList[vertex];

            if (tbtnCanDrag.IsChecked.HasValue && tbtnCanDrag.IsChecked.Value)
            {
                RunCodeInUiThread(() =>
                {
                    foreach (IGraphControl edge in Area.GetRelatedControls(vc, GraphControlType.Edge, EdgesType.All))
                    {
                        var ec = (EdgeControl)edge;
                        var op = new DeleteEdgeOperation(Area, ec.Source.Vertex as DataVertex, ec.Target.Vertex as DataVertex, ec.Edge as DataEdge);
                        op.Do();
                        op.UnDo();
                    }

                    //    Area.GenerateAllEdges();
                    //    Area.ShowAllEdgesLabels();
                    //Area.ComputeEdgeRoutesByVertex(vc);
                    //Area.InvalidateVisual();
                    //////Area.UpdateAllEdges();
                    //Area.ComputeEdgeRoutesByVertex(vc);
                    //vc.InvalidateVisual();
                }, priority: DispatcherPriority.Loaded);
            }
        }

        private void OnEdgeLoaded(IEnumerable<DataEdge> edges)
        {
            Edges = edges;
            GraphDataService.GetVertexes(OnVertexesLoaded, OnError);
        }

        private void CreateGraphArea(GraphArea area, IEnumerable<DataVertex> vertexes, IEnumerable<DataEdge> edges, double offsetY)
        {
            area.ClearLayout();

            var graph = new Graph();

            graph.AddVertexRange(vertexes);

            graph.AddEdgeRange(edges);

            ((GraphLogic)area.LogicCore).ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>(graph, 1.5, offsetY: offsetY);

            area.GenerateGraph(graph, true, true);
        }

        private void OnError(Exception ex)
        {
        }

        private static void VertexesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void EdgesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void GraphDataServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ((GraphExplorerView)d).GetEdges();
            }
        }

        private void SettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var ge = (GraphExplorerView)d;
                ge.ApplySetting(ge.zoomctrl, ge.Area.LogicCore);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tbtnCanEdit.IsChecked.HasValue && tbtnCanEdit.IsChecked.Value)
            {
                if (MessageBox.Show("Refresh view in edit mode will discard changes you made, will you want to continue?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    tbtnCanEdit.IsChecked = false;
                    tbtnCanDrag.IsChecked = false;
                    tbtnIsFilterApplied.IsChecked = false;
                    InnerRefreshGraph();
                    ((GraphExplorerViewModel)ViewModel).PostStatusMessage("Graph Refreshed");
                }
            }
            else
            {
                InnerRefreshGraph();
            }
        }

        private void InnerRefreshGraph()
        {
            GetEdges();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            AreaNav.ClearLayout();

            navTab.Visibility = Visibility.Hidden;

            overrallTab.IsSelected = true;
        }

        private void btnRefresh_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //select overrall tab
            overrallTab.IsSelected = true;

            try
            {
                var dlg = new SaveFileDialog { Filter = "All files|*.xml", Title = "Select layout file name", FileName = "overrall_layout.xml" };
                if (dlg.ShowDialog() == true)
                {
                    //gg_Area.SaveVisual(dlg.FileName);
                    Area.SaveIntoFile(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //select overrall tab
            overrallTab.IsSelected = true;

            var dlg = new OpenFileDialog { Filter = "All files|*.xml", Title = "Select layout file", FileName = "overrall_layout.xml" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    Area.LoadFromFile(dlg.FileName);
                    Area.RelayoutGraph();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Failed to load layout file:\n {0}", ex));
                }
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Area.ExportAsImage(ImageType.PNG);
        }

        private void btnExportNav_Click(object sender, RoutedEventArgs e)
        {
            AreaNav.ExportAsPng();
        }

        private void settingView_SettingApplied(object sender, SettingAppliedRoutedEventArgs e)
        {
            if (e.NeedRefresh)
            {
                AreaNav.ClearLayout();

                navTab.Visibility = Visibility.Hidden;

                overrallTab.IsSelected = true;

                GraphDataService = new CsvGraphDataService();

                tbtnCanDrag.IsChecked = false;
                tbtnCanEdit.IsChecked = false;
                tbtnIsFilterApplied.IsChecked = false;
            }

            ((SettingView)sender).Visibility = Visibility.Collapsed;
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            settingView.Visibility = Visibility.Visible;
        }

        private void tbtnCanDrag_Click(object sender, RoutedEventArgs e)
        {
            UpdateCanDrag(Area, tbtnCanDrag.IsChecked.Value);
        }

        private void UpdateCanDrag(GraphArea area, bool canDrag)
        {
            foreach (var item in area.VertexList)
            {
                DragBehaviour.SetIsDragEnabled(item.Value, canDrag);
            }
        }

        private void Value_PositionChanged(object sender, VertexPositionEventArgs args)
        {
            Point zoomtop = zoomctrl.TranslatePoint(new Point(0, 0), Area);

            var zoombottom = new Point(Area.ActualWidth, Area.ActualHeight);

            Point pos = args.OffsetPosition;

            if (pos.X < zoomtop.X)
            {
                GraphAreaBase.SetX(args.VertexControl, zoomtop.X + 1, true);
            }

            if (pos.Y < zoomtop.Y)
            {
                GraphAreaBase.SetY(args.VertexControl, zoomtop.Y + 1, true);
            }

            if (pos.X > zoombottom.X)
            {
                GraphAreaBase.SetX(args.VertexControl, zoombottom.X, true);
            }
            if (pos.Y > zoombottom.Y)
            {
                GraphAreaBase.SetY(args.VertexControl, zoombottom.Y, true);
            }
        }

        private void SelectVertex(VertexControl vc)
        {
            var v = vc.Vertex as DataVertex;
            if (v == null)
            {
                return;
            }

            if (_selectedVertices.Contains(v.Id))
            {
                _selectedVertices.Remove(v.Id);
                HighlightBehaviour.SetHighlighted(vc, false);
                //DragBehaviour.SetIsTagged(vc, false);
            }
            else
            {
                _selectedVertices.Add(v.Id);
                HighlightBehaviour.SetHighlighted(vc, true);
                //DragBehaviour.SetIsTagged(vc, true);
            }
        }

        private void tbtnCanEdit_Click(object sender, RoutedEventArgs e)
        {
            //if (tbtnCanDrag.IsChecked.HasValue && tbtnCanDrag.IsChecked.Value)
            //{
            //    tbtnCanDrag.IsChecked = false;
            //    UpdateCanDrag(Area, tbtnCanDrag.IsChecked.Value);
            //}
            UpdateIsInEditMode(Area.VertexList, tbtnCanEdit.IsChecked.Value);
            UpdateHighlightBehaviour(true);
        }

        private void UpdateIsInEditMode(IDictionary<DataVertex, VertexControl> dictionary, bool isInEditMode)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var v in dictionary)
            {
                v.Key.IsEditing = isInEditMode;

                if (isInEditMode)
                {
                    v.Key.ChangedCommited += Key_ChangedCommited;
                }
                else
                {
                    v.Key.ChangedCommited -= Key_ChangedCommited;
                }
            }
            //throw new NotImplementedException();
        }

        private void Key_ChangedCommited(object sender, EventArgs e)
        {
            var data = (DataVertex)sender;

            GraphDataService.UpdateVertex(data, (r, v, ex) =>
            {
                if (ex != null)
                {
                    ShowAlertMessage(ex.ToString());
                }
            });
            //throw new NotImplementedException();
        }

        private void UpdateHighlightBehaviour(bool clearSelectedVertices)
        {
            if (clearSelectedVertices)
            {
                _selectedVertices.Clear();
            }

            if (tbtnCanEdit.IsChecked.Value)
            {
                foreach (var v in Area.VertexList)
                {
                    HighlightBehaviour.SetIsHighlightEnabled(v.Value, false);
                    HighlightBehaviour.SetHighlighted(v.Value, false);
                }
                foreach (var edge in Area.EdgesList)
                {
                    HighlightBehaviour.SetIsHighlightEnabled(edge.Value, false);
                    HighlightBehaviour.SetHighlighted(edge.Value, false);
                }
            }
            else
            {
                foreach (var v in Area.VertexList)
                {
                    HighlightBehaviour.SetIsHighlightEnabled(v.Value, true);
                    HighlightBehaviour.SetHighlighted(v.Value, false);
                }
                foreach (var edge in Area.EdgesList)
                {
                    HighlightBehaviour.SetIsHighlightEnabled(edge.Value, true);
                    HighlightBehaviour.SetHighlighted(edge.Value, false);
                }
            }
        }

        private void tbnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GraphDataService.UpdateEdges(Area.LogicCore.Graph.Edges, (result, error) =>
                {
                    if (!result && error != null)
                    {
                        ShowAlertMessage(error.Message);
                    }
                });

                GraphDataService.UpdateVertexes(Area.LogicCore.Graph.Vertices, (result, error) =>
                {
                    if (!result && error != null)
                    {
                        ShowAlertMessage(error.Message);
                    }
                });

                _selectedVertices.Clear();

                //clear dirty flag
                ((GraphExplorerViewModel)ViewModel).Commit();

                tbtnCanEdit.IsChecked = false;

                UpdateIsInEditMode(Area.VertexList, tbtnCanEdit.IsChecked.Value);

                UpdateHighlightBehaviour(true);
                //GetEdges();
            }
            catch (Exception ex)
            {
                ShowAlertMessage(ex.Message);
                ;
            }
        }

        private void tbnNewEdge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateHighlightBehaviour(true);

                if (tbnNewEdge.IsChecked.HasValue && tbnNewEdge.IsChecked.Value)
                {
                    _status = _status | GraphExplorerStatus.CreateLinkSelectSource;
                    ((GraphExplorerViewModel)ViewModel).PostStatusMessage("Select Source Node");
                }
                else
                {
                    ClearEdgeDrawing();
                    ((GraphExplorerViewModel)ViewModel).PostStatusMessage("Exit Create Link");
                }
            }
            catch (Exception ex)
            {
                ShowAlertMessage(ex.Message);
            }
        }

        private void tbnNewNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateVertex(Area, zoomctrl);
            }
            catch (Exception ex)
            {
                ShowAlertMessage(ex.Message);
            }
        }

        private void CreateVertex(GraphArea area, ZoomControl zoom, DataVertex data = null, double x = double.MinValue, double y = double.MinValue)
        {
            ((GraphExplorerViewModel)ViewModel).Do(new CreateVertexOperation(Area, data, x, y, (v, vc) =>
            {
                _selectedVertices.Add(v.Id);

                //area.RelayoutGraph(true);

                UpdateHighlightBehaviour(false);

                foreach (int selectedV in _selectedVertices)
                {
                    VertexControl localvc = area.VertexList.Where(pair => pair.Key.Id == selectedV).Select(pair => pair.Value).FirstOrDefault();
                    HighlightBehaviour.SetHighlighted(localvc, true);
                }

                if (tbtnCanDrag.IsChecked.Value)
                {
                    DragBehaviour.SetIsDragEnabled(vc, true);
                }
                else
                {
                    DragBehaviour.SetIsDragEnabled(vc, false);
                }

                v.IsEditing = true;
                v.OnPositionChanged -= v_OnPositionChanged;
                v.OnPositionChanged += v_OnPositionChanged;
            }, v =>
            {
                _selectedVertices.Remove(v.Id);
                //on vertex recreated
            }));
            //FitToBounds(area.Dispatcher, zoom);
        }

        private void v_OnPositionChanged(object sender, DataVertex.VertexPositionChangedEventArgs e)
        {
            var vertex = (DataVertex)sender;
            if (Area.VertexList.Keys.Any(v => v.Id == vertex.Id))
            {
                VertexControl vc = Area.VertexList.First(v => v.Key.Id == vertex.Id).Value;
                //throw new NotImplementedException();
                ((GraphExplorerViewModel)ViewModel).Do(new VertexPositionChangeOperation(Area, vc, e.OffsetX, e.OffsetY, vertex));
            }
        }

        private void SafeRemoveVertex(VertexControl vc, GraphArea area, GraphLogic logic, bool removeFromSelected = false)
        {
            //remove all adjacent edges
            foreach (IGraphControl item in area.GetRelatedControls(vc, GraphControlType.Edge, EdgesType.All))
            {
                var ec = item as EdgeControl;
                logic.Graph.RemoveEdge(ec.Edge as DataEdge);
                area.RemoveEdge(ec.Edge as DataEdge);
            }

            var v = vc.Vertex as DataVertex;
            logic.Graph.RemoveVertex(v);
            area.RemoveVertex(v);

            if (removeFromSelected && v != null && _selectedVertices.Contains(v.Id))
            {
                _selectedVertices.Remove(v.Id);
            }
        }

        private void ShowAlertMessage(string message)
        {
            MessageBox.Show(message);
        }

        public static void RunCodeInUiThread<T>(Action<T> action, T parameter, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (action == null)
            {
                return;
            }

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority, parameter);
            }
            else
            {
                action.Invoke(parameter);
            }
        }

        public static void RunCodeInUiThread(Action action, Dispatcher dispatcher = null, DispatcherPriority priority = DispatcherPriority.Loaded)
        {
            if (action == null)
            {
                return;
            }

            if (dispatcher == null && Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }

            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action, priority);
            }
            else
            {
                action.Invoke();
            }
        }

        //drag to add new node
        private void tbnNewNode_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var data = new DataObject(typeof(object), new object());
            DragDrop.DoDragDrop(tbnNewNode, data, DragDropEffects.Copy);
        }

        private void zoomctrl_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(object)))
            {
                //how to get dragged data by its type
                object myobject = e.Data.GetData(typeof(object));

                Point pos = zoomctrl.TranslatePoint(e.GetPosition(zoomctrl), Area);

                DataVertex data = DataVertex.Create();

                CreateVertex(Area, zoomctrl, data, pos.X, pos.Y);
            }
        }

        //handle create link between two node
        private void zoomctrl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget) && _edGeo != null && _edEdge != null && _edVertex != null)
            {
                Point pos = zoomctrl.TranslatePoint(e.GetPosition(zoomctrl), Area);
                var lastseg = _edGeo.Figures[0].Segments[_edGeo.Figures[0].Segments.Count - 1] as PolyLineSegment;
                lastseg.Points[lastseg.Points.Count - 1] = pos;
                _edEdge.SetEdgePathManually(_edGeo);
            }
        }

        private void zoomctrl_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }
        #endregion
    }
}