namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Converters;

    using Enums;

    using GraphX;
    using GraphX.Controls;
    using GraphX.GraphSharp.Algorithms.OverlapRemoval;
    using GraphX.Models;

    using Microsoft.Win32;
    using Models;
    using Orc.GraphExplorer.Config;
    using Orc.GraphExplorer.Events;
    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Operations;
    using Orc.GraphExplorer.Operations.Interfaces;
    using Orc.GraphExplorer.Services;
    using Orc.GraphExplorer.Views;
    using Orc.GraphExplorer.Views.Enums;
    using Orc.GraphExplorer.Views.Interfaces;

    using QuickGraph;

    using Services.Interfaces;

    using IValueConverter = Catel.MVVM.Converters.IValueConverter;

    public class GraphExplorerViewModel : ViewModelBase
    {
        #region Fields
        private DataVertex _currentNavItem;       

        private DataVertex _edFakeDV;

        private readonly List<IDisposable> _observers = new List<IDisposable>();        

        private IEnumerable<DataVertex> _vertexes;

        private IEnumerable<DataEdge> _edges;

        private string _filterText;

        private bool _isFilterApplied;

        private string _statusMessage;

        private bool _isHideVertexes;

        private bool _hasUnCommitChange;

        

       
        #endregion

        #region Constructors
        public GraphExplorerViewModel()
        {
            SelectedVertices = new List<int>();
            OperationObserver = new OperationObserver();
            IsHideVertexes = false;
            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;

            SaveToXml = new Command(OnSaveToXmlExecute);
            LoadFromXml = new Command(OnLoadFromXmlExecute);
            SaveToImage = new Command(OnSaveToImageExecute);
            CanEditCommand = new Command(OnCanEditCommandExecute);
            CreateLinkCommand = new Command(OnCreateLinkCommandExecute);
            UndoCommand = new Command(OnUndoCommandExecute, OnUndoCommandCanExecute);
            RedoCommand = new Command(OnRedoCommandExecute, OnRedoCommandCanExecute);
            SaveChangesCommand = new Command(OnSaveChangesCommandExecute);
            RefreshCommand = new Command(OnRefreshCommandExecute);
            ClearFilterCommand = new Command(OnClearFilterCommandExecute, OnClearFilterCommandCanExecute);
            CloseNavTabCommand = new Command(OnCloseNavTabCommandExecute);
            SaveNavToImage = new Command(OnSaveNavToImageExecute);
            OpenSettingsCommand = new Command(OnOpenSettingsCommandExecute);
            SettingAppliedCommand = new Command<SettingAppliedRoutedEventArgs>(OnSettingAppliedCommandExecute);
            FurtherNavigateCommand = new Command<VertexSelectedEventArgs>(OnFurtherNavigateCommandExecute);
            RelayoutAreaNavFinishedCommand = new Command(OnRelayoutAreaNavFinishedCommandExecute);
            NavigateCommand = new Command<VertexSelectedEventArgs>(OnNavigateCommandExecute);
            EdgeSelectedCommand = new Command<EdgeSelectedEventArgs>(OnEdgeSelectedCommandExecute);
            VertexSelectedCommand = new Command<VertexSelectedEventArgs>(OnVertexSelectedCommandExecute);
            RelayoutAreaFinishedCommand = new Command(OnRelayoutAreaFinishedCommandExecute);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the RelayoutAreaFinishedCommand command.
        /// </summary>
        public Command RelayoutAreaFinishedCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the RelayoutAreaFinishedCommand command is executed.
        /// </summary>
        private void OnRelayoutAreaFinishedCommandExecute()
        {
            OnRelayoutFinished(GraphExplorerTab.Main);
        }

        /// <summary>
        /// Gets the RelayoutAreaNavFinishedCommand command.
        /// </summary>
        public Command RelayoutAreaNavFinishedCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the RelayoutAreaNavFinishedCommand command is executed.
        /// </summary>
        private void OnRelayoutAreaNavFinishedCommandExecute()
        {
            OnRelayoutFinished(GraphExplorerTab.Navigation);
        }

        /// <summary>
        /// Gets the VertexSelectedCommand command.
        /// </summary>
        public Command<VertexSelectedEventArgs> VertexSelectedCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the VertexSelectedCommand command is executed.
        /// </summary>
        private void OnVertexSelectedCommandExecute(VertexSelectedEventArgs eventArgs)
        {
            if (eventArgs.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                //if (DragBehaviour.GetIsDragging(args.VertexControl)) return;
                SelectVertex(eventArgs.VertexControl);

                if (IsInEditing && Status.HasFlag(GraphExplorerStatus.CreateLinkSelectSource))
                {
                    if (!View.IsVertexEditing) //select starting vertex
                    {
                        View.SetEdVertex(eventArgs.VertexControl as VertexControl);
                        _edFakeDV = new DataVertex { ID = -666 };
                        EdGeometry = View.CreatePathGeometry();
                        Point pos = View.zoomctrl.TranslatePoint(eventArgs.VertexControl.GetPosition(), View.Area);
                        var lastseg = EdGeometry.Figures[0].Segments[EdGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
                        lastseg.Points[lastseg.Points.Count - 1] = pos;

                        var dedge = new DataEdge(View.GetEdVertex(), _edFakeDV);
                        View.AddEdge(dedge);
                        Logic.Graph.AddVertex(_edFakeDV);
                        Logic.Graph.AddEdge(dedge);
                        View.SetEdgePathManually(EdGeometry);
                        Status = GraphExplorerStatus.CreateLinkSelectTarget;
                        PostStatusMessage("Select Target Node");
                    }
                    else if (!View.IsEdVertex(eventArgs.VertexControl as VertexControl) && Status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget)) //finish draw
                    {
                        CreateEdge(View.GetEdVertex().Id, (eventArgs.VertexControl.Vertex as DataVertex).Id);

                        ClearEdgeDrawing();

                        Status = GraphExplorerStatus.Ready;

                        IsAddingNewEdge = false;
                    }
                }
            }
            else if (eventArgs.MouseArgs.RightButton == MouseButtonState.Pressed && IsInEditing)
            {
                eventArgs.VertexControl.ContextMenu = new ContextMenu();
                var miDeleteVertex = new MenuItem { Header = "Delete", Tag = eventArgs.VertexControl };
                miDeleteVertex.Click += miDeleteVertex_Click;
                eventArgs.VertexControl.ContextMenu.Items.Add(miDeleteVertex);
            }
        }


        /// <summary>
        /// Gets the EdgeSelectedCommand command.
        /// </summary>
        public Command<EdgeSelectedEventArgs> EdgeSelectedCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the EdgeSelectedCommand command is executed.
        /// </summary>
        private void OnEdgeSelectedCommandExecute(EdgeSelectedEventArgs eventArgs)
        {
            if (IsInEditing)
            {
                eventArgs.EdgeControl.ContextMenu = new ContextMenu();
                var miDeleteLink = new MenuItem { Header = "Delete Link", Tag = eventArgs.EdgeControl };
                miDeleteLink.Click += miDeleteLink_Click;
                eventArgs.EdgeControl.ContextMenu.Items.Add(miDeleteLink);
            }
        }

        /// <summary>
        /// Gets the NavigateCommand command.
        /// </summary>
        public Command<VertexSelectedEventArgs> NavigateCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the NavigateCommand command is executed.
        /// </summary>
        private void OnNavigateCommandExecute(VertexSelectedEventArgs eventArgs)
        {
            if (IsInEditing)
            {
                return;
            }

            var vertex = eventArgs.VertexControl.DataContext as DataVertex;

            if (vertex == null)
            {
                return;
            }

            _currentNavItem = vertex;

            int degree = Logic.Graph.Degree(vertex);

            if (degree < 1)
            {
                return;
            }

            NavigateTo(vertex, Logic.Graph);

            if (!IsNavTabVisible)
            {
                IsNavTabVisible = true;
            }

            IsNavTabSelected = true;
        }



        /// <summary>
        /// Gets the FurtherNavigateCommand command.
        /// </summary>
        public Command<VertexSelectedEventArgs> FurtherNavigateCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the FurtherNavigateCommand command is executed.
        /// </summary>
        private void OnFurtherNavigateCommandExecute(VertexSelectedEventArgs eventArgs)
        {
            //throw new NotImplementedException();
            var vertex = eventArgs.VertexControl.DataContext as DataVertex;

            if (vertex == null || vertex == _currentNavItem)
            {
                return;
            }

            _currentNavItem = vertex;

            int degree = Logic.Graph.Degree(vertex);

            if (degree < 1)
            {
                return;
            }

            NavigateTo(vertex, Logic.Graph);
        }

        /// <summary>
        /// Gets the SettingAppliedCommand command.
        /// </summary>
        public Command<SettingAppliedRoutedEventArgs> SettingAppliedCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the SettingAppliedCommand command is executed.
        /// </summary>
        private void OnSettingAppliedCommandExecute(SettingAppliedRoutedEventArgs eventArgs)
        {
            if (eventArgs.NeedRefresh)
            {
                View.ClearLayout(GraphExplorerTab.Navigation);

                IsNavTabVisible = false;

                IsOverallTabSelected = true;

                GraphDataService = new CsvGraphDataService();

                CanDrag = false;
                IsInEditing = false;
                IsFilterApplied = false;
            }

            IsSettingsVisible = false;
        }

        /// <summary>
        /// Gets the OpenSettingsCommand command.
        /// </summary>
        public Command OpenSettingsCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenSettingsCommand command is executed.
        /// </summary>
        private void OnOpenSettingsCommandExecute()
        {
            IsSettingsVisible = !IsSettingsVisible;
        }

        /// <summary>
        /// Gets the CloseNavTabCommand command.
        /// </summary>
        public Command CloseNavTabCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CloseNavTabCommand command is executed.
        /// </summary>
        private void OnCloseNavTabCommandExecute()
        {
            View.ClearLayout(GraphExplorerTab.Navigation);

            IsNavTabVisible = false;

            IsOverallTabSelected = true;
        }

        /// <summary>
        /// Gets the ClearFilterCommand command.
        /// </summary>
        public Command ClearFilterCommand { get; private set; }

        /// <summary>
        /// Method to check whether the ClearFilterCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnClearFilterCommandCanExecute()
        {
            return IsFilterApplied;
        }

        /// <summary>
        /// Method to invoke when the ClearFilterCommand command is executed.
        /// </summary>
        private void OnClearFilterCommandExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the RefreshCommand command.
        /// </summary>
        public Command RefreshCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the RefreshCommand command is executed.
        /// </summary>
        private void OnRefreshCommandExecute()
        {
            if (IsInEditing)
            {
                if (MessageBox.Show("Refresh view in edit mode will discard changes you made, will you want to continue?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IsInEditing = false;
                    CanDrag = false;
                    IsFilterApplied = false;
                    InnerRefreshGraph();
                    PostStatusMessage("Graph Refreshed");
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

        public void GetEdges()
        {
            GraphDataService.GetEdges(OnEdgeLoaded, OperationObserver.OnError);
        }

        private void OnEdgeLoaded(IEnumerable<DataEdge> edges)
        {
            Edges = edges;
            GraphDataService.GetVertexes(OnVertexesLoaded, OperationObserver.OnError);
        }

        private void OnVertexesLoaded(IEnumerable<DataVertex> vertexes)
        {
            Vertexes = new List<DataVertex>(vertexes);

            CreateGraphArea(View.Area, Vertexes, Edges, 600);

            HookVertexEvent();

            OnVertexLoaded(Vertexes);
            //FitToBounds(Area.Dispatcher, zoomctrl);
        }

        private void HookVertexEvent()
        {
            foreach (var vertex in Logic.Graph.Vertices)
            {
                vertex.IsExpandedChanged += (s, e) => vertex_IsExpandedChanged(s, e);
            }
            //throw new NotImplementedException();
        }

        private void vertex_IsExpandedChanged(object sender, EventArgs e)
        {
            var vertex = (DataVertex)sender;

            if (vertex.IsExpanded || vertex.Properties == null || vertex.Properties.Count < 1 || !Logic.Graph.ContainsVertex(vertex))
            {
                return;
            }

            VertexControl vc = View.Area.VertexList[vertex];

            if (CanDrag)
            {
                RunCodeInUiThread(() =>
                {
                    foreach (IGraphControl edge in View.Area.GetRelatedControls(vc, GraphControlType.Edge, EdgesType.All))
                    {
                        var ec = (EdgeControl)edge;
                        var op = new DeleteEdgeOperation(View.Area, ec.Source.Vertex as DataVertex, ec.Target.Vertex as DataVertex, ec.Edge as DataEdge);
                        op.Do();
                        op.UnDo();
                    }
                }, priority: DispatcherPriority.Loaded);
            }
        }

        public void CreateGraphArea(GraphArea area, IEnumerable<DataVertex> vertexes, IEnumerable<DataEdge> edges, double offsetY)
        {
            area.ClearLayout();

            var graph = new Graph();

            graph.AddVertexRange(vertexes);

            graph.AddEdgeRange(edges);

            ((GraphLogic)area.LogicCore).ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>(graph, 1.5, offsetY: offsetY);

            area.GenerateGraph(graph, true, true);
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


        /// <summary>
        /// Gets the SaveChangesCommand command.
        /// </summary>
        public Command SaveChangesCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveChangesCommand command is executed.
        /// </summary>
        private void OnSaveChangesCommandExecute()
        {
            try
            {
                GraphDataService.UpdateEdges(Logic.Graph.Edges, (result, error) =>
                {
                    if (!result && error != null)
                    {
                        ShowAlertMessage(error.Message);
                    }
                });

                GraphDataService.UpdateVertexes(Logic.Graph.Vertices, (result, error) =>
                {
                    if (!result && error != null)
                    {
                        ShowAlertMessage(error.Message);
                    }
                });

                SelectedVertices.Clear();

                //clear dirty flag
                Commit();

                IsInEditing = false;

                UpdateIsInEditMode(IsInEditing);

                UpdateHighlightBehaviour(true);
                //GetEdges();
            }
            catch (Exception ex)
            {
                ShowAlertMessage(ex.Message);
            }
        }


        /// <summary>
        /// Gets the CreateLinkCommand command.
        /// </summary>
        public Command CreateLinkCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CreateLinkCommand command is executed.
        /// </summary>
        private void OnCreateLinkCommandExecute()
        {
            try
            {
                UpdateHighlightBehaviour(true);

                if (IsAddingNewEdge)
                {
                    Status = Status | GraphExplorerStatus.CreateLinkSelectSource;
                    PostStatusMessage("Select Source Node");
                }
                else
                {
                    ClearEdgeDrawing();
                    PostStatusMessage("Exit Create Link");
                }
            }
            catch (Exception ex)
            {
                ShowAlertMessage(ex.Message);
            }
        }

        public void ClearEdgeDrawing()
        {
            if (_edFakeDV != null)
            {
                Logic.Graph.RemoveVertex(_edFakeDV);
            }
            if (View.IsEdgeEditing)
            {
                var edge = View.GetEdEdge();
                Logic.Graph.RemoveEdge(edge);
                View.RemoveEdge(edge);
            }
            EdGeometry = null;
            _edFakeDV = null;
            View.ClearEdEdge();
            View.ClearEdVertex();
        }

        /// <summary>
        /// Gets the SaveToXml command.
        /// </summary>
        public Command SaveToXml { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveToXml command is executed.
        /// </summary>
        private void OnSaveToXmlExecute()
        {
            //select overrall tab
            IsOverallTabSelected = true;

            try
            {
                var dlg = new SaveFileDialog { Filter = "All files|*.xml", Title = "Select layout file name", FileName = "overrall_layout.xml" };
                if (dlg.ShowDialog() == true)
                {
                    //gg_Area.SaveVisual(dlg.FileName);
                    View.Area.SaveIntoFile(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the LoadFromXml command.
        /// </summary>
        public Command LoadFromXml { get; private set; }

        /// <summary>
        /// Method to invoke when the LoadFromXml command is executed.
        /// </summary>
        private void OnLoadFromXmlExecute()
        {
            //select overrall tab
            IsOverallTabSelected = true;

            var dlg = new OpenFileDialog { Filter = "All files|*.xml", Title = "Select layout file", FileName = "overrall_layout.xml" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    View.Area.LoadFromFile(dlg.FileName);
                    View.Area.RelayoutGraph();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Failed to load layout file:\n {0}", ex));
                }
            }
        }

        /// <summary>
        /// Gets the SaveNavToImage command.
        /// </summary>
        public Command SaveNavToImage { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveNavToImage command is executed.
        /// </summary>
        private void OnSaveNavToImageExecute()
        {
            View.AreaNav.ExportAsPng();
        }

        /// <summary>
        /// Gets the SaveToImage command.
        /// </summary>
        public Command SaveToImage { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveToImage command is executed.
        /// </summary>
        private void OnSaveToImageExecute()
        {
            View.Area.ExportAsImage(ImageType.PNG);
        }

        /// <summary>
        /// Gets the CanEditCommand command.
        /// </summary>
        public Command CanEditCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CanEditCommand command is executed.
        /// </summary>
        private void OnCanEditCommandExecute()
        {
            //if (tbtnCanDrag.IsChecked.HasValue && tbtnCanDrag.IsChecked.Value)
            //{
            //    tbtnCanDrag.IsChecked = false;
            //    UpdateCanDrag(Area, tbtnCanDrag.IsChecked.Value);
            //}
            UpdateIsInEditMode(IsInEditing);
            UpdateHighlightBehaviour(true);
        }

        public void UpdateHighlightBehaviour(bool clearSelectedVertices)
        {
            if (clearSelectedVertices)
            {
                SelectedVertices.Clear();
            }

            if (IsInEditing)
            {
                foreach (var v in Logic.Graph.Vertices)
                {
                    View.SetIsHighlightEnabled(GraphExplorerTab.Main, v, false);
                    View.SetHighlighted(GraphExplorerTab.Main, v, false);
                }
                foreach (var edge in Logic.Graph.Edges)
                {
                    View.SetIsHighlightEnabled(GraphExplorerTab.Main, edge, false);
                    View.SetHighlighted(GraphExplorerTab.Main, edge, false);
                }
            }
            else
            {
                foreach (var v in Logic.Graph.Vertices)
                {
                    View.SetIsHighlightEnabled(GraphExplorerTab.Main, v, true);
                    View.SetHighlighted(GraphExplorerTab.Main, v, false);
                }
                foreach (var edge in Logic.Graph.Edges)
                {
                    View.SetIsHighlightEnabled(GraphExplorerTab.Main, edge, true);
                    View.SetHighlighted(GraphExplorerTab.Main, edge, false);
                }
            }
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

        private void ShowAlertMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void UpdateIsInEditMode(bool isInEditMode)
        {
            foreach (var v in Logic.Graph.Vertices)
            {
                v.IsEditing = isInEditMode;

                if (isInEditMode)
                {
                    v.ChangedCommited += Key_ChangedCommited;
                }
                else
                {
                    v.ChangedCommited -= Key_ChangedCommited;
                }
            }
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the UndoCommand command.
        /// </summary>
        public Command UndoCommand { get; private set; }

        /// <summary>
        /// Method to check whether the UndoCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnUndoCommandCanExecute()
        {
            return OperationObserver.HasUndoable;
        }

        /// <summary>
        /// Method to invoke when the UndoCommand command is executed.
        /// </summary>
        private void OnUndoCommandExecute()
        {
            OperationObserver.Undo();
        }

        /// <summary>
        /// Gets the RedoCommand command.
        /// </summary>
        public Command RedoCommand { get; private set; }

        /// <summary>
        /// Method to check whether the RedoCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRedoCommandCanExecute()
        {
            return OperationObserver.HasRedoable;
        }

        /// <summary>
        /// Method to invoke when the RedoCommand command is executed.
        /// </summary>
        private void OnRedoCommandExecute()
        {
            OperationObserver.Redo();
        }

        #endregion // Commands

        #region Properties
       /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphLogic NavLogic
        {
            get
            {
                return GetValue<GraphLogic>(NavLogicProperty);
            }
            set
            {
                SetValue(NavLogicProperty, value);
            }
        }

        /// <summary>
        /// Register the NavLogic property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NavLogicProperty = RegisterProperty("NavLogic", typeof(GraphLogic), () => new GraphLogic());

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphLogic Logic
        {
            get
            {
                return GetValue<GraphLogic>(LogicProperty);
            }
            set
            {
                SetValue(LogicProperty, value);
            }
        }

        /// <summary>
        /// Register the Logic property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LogicProperty = RegisterProperty("Logic", typeof(GraphLogic), () => new GraphLogic());

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return GetValue<string>(StatusMessageProperty);
            }
            set
            {
                SetValue(StatusMessageProperty, value);
            }
        }

        /// <summary>
        /// Register the StatusMessage property so it is known in the class.
        /// </summary>
        public static readonly PropertyData StatusMessageProperty = RegisterProperty("StatusMessage", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsSettingsVisible
        {
            get
            {
                return GetValue<bool>(IsSettingsVisibleProperty);
            }
            set
            {
                SetValue(IsSettingsVisibleProperty, value);
            }
        }

        /// <summary>
        /// Register the IsSettingsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsSettingsVisibleProperty = RegisterProperty("IsSettingsVisible", typeof(bool), false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabSelected
        {
            get
            {
                return GetValue<bool>(IsNavTabSelectedProperty);
            }
            set
            {
                SetValue(IsNavTabSelectedProperty, value);
            }
        }

        /// <summary>
        /// Register the IsNavTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabSelectedProperty = RegisterProperty("IsNavTabSelected", typeof(bool), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabVisible
        {
            get
            {
                return GetValue<bool>(IsNavTabVisibleProperty);
            }
            set
            {
                SetValue(IsNavTabVisibleProperty, value);
            }
        }

        /// <summary>
        /// Register the IsNavTabVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabVisibleProperty = RegisterProperty("IsNavTabVisible", typeof(bool), false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string FilterText
        {
            get
            {
                return GetValue<string>(FilterTextProperty);
            }
            set
            {
                SetValue(FilterTextProperty, value);
            }
        }

        /// <summary>
        /// Register the FilterText property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterTextProperty = RegisterProperty("FilterText", typeof(string), null, (sender, e) => ((GraphExplorerViewModel)sender).OnFilterTextChanged());

        /// <summary>
        /// Called when the FilterText property has changed.
        /// </summary>
        private void OnFilterTextChanged()
        {
            if (IsFilterApplied)
            {
                ApplyFilter(_filterText);
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHideVertexes
        {
            get
            {
                return GetValue<bool>(IsHideVertexesProperty);
            }
            set
            {
                SetValue(IsHideVertexesProperty, value);
            }
        }

        /// <summary>
        /// Register the IsHideVertexes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHideVertexesProperty = RegisterProperty("IsHideVertexes", typeof(bool), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterEntity> FilteredEntities
        {
            get
            {
                return GetValue<ObservableCollection<FilterEntity>>(FilteredEntitiesProperty);
            }
            set
            {
                SetValue(FilteredEntitiesProperty, value);
            }
        }

        /// <summary>
        /// Register the FilteredEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredEntitiesProperty = RegisterProperty("FilteredEntities", typeof(ObservableCollection<FilterEntity>), () => new ObservableCollection<FilterEntity>());

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterEntity> Entities
        {
            get
            {
                return GetValue<ObservableCollection<FilterEntity>>(EntitiesProperty);
            }
            set
            {
                SetValue(EntitiesProperty, value);
            }
        }

        /// <summary>
        /// Register the Entities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EntitiesProperty = RegisterProperty("Entities", typeof(ObservableCollection<FilterEntity>), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ZoomControlModes ZoomModeNav
        {
            get
            {
                return GetValue<ZoomControlModes>(ZoomModeNavProperty);
            }
            set
            {
                SetValue(ZoomModeNavProperty, value);
            }
        }

        /// <summary>
        /// Register the ZoomModeNav property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ZoomModeNavProperty = RegisterProperty("ZoomModeNav", typeof(ZoomControlModes), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ZoomControlModes ZoomMode
        {
            get
            {
                return GetValue<ZoomControlModes>(ZoomModeProperty);
            }
            set
            {
                SetValue(ZoomModeProperty, value);
            }
        }

        /// <summary>
        /// Register the ZoomMode property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ZoomModeProperty = RegisterProperty("ZoomMode", typeof(ZoomControlModes), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double ZoomNav
        {
            get
            {
                return GetValue<double>(ZoomNavProperty);
            }
            set
            {
                SetValue(ZoomNavProperty, value);
            }
        }

        /// <summary>
        /// Register the ZoomNav property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ZoomNavProperty = RegisterProperty("ZoomNav", typeof(double), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double Zoom
        {
            get
            {
                return GetValue<double>(ZoomProperty);
            }
            set
            {
                SetValue(ZoomProperty, value);
            }
        }

        /// <summary>
        /// Register the Zoom property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ZoomProperty = RegisterProperty("Zoom", typeof(double), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double MinZoomNav
        {
            get
            {
                return GetValue<double>(MinZoomNavProperty);
            }
            set
            {
                SetValue(MinZoomNavProperty, value);
            }
        }

        /// <summary>
        /// Register the MinZoomNav property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MinZoomNavProperty = RegisterProperty("MinZoomNav", typeof(double), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double MaxZoomNav
        {
            get
            {
                return GetValue<double>(MaxZoomNavProperty);
            }
            set
            {
                SetValue(MaxZoomNavProperty, value);
            }
        }

        /// <summary>
        /// Register the MaxZoomNav property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MaxZoomNavProperty = RegisterProperty("MaxZoomNav", typeof(double), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double MinZoom
        {
            get
            {
                return GetValue<double>(MinZoomProperty);
            }
            set
            {
                SetValue(MinZoomProperty, value);
            }
        }

        /// <summary>
        /// Register the MinZoom property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MinZoomProperty = RegisterProperty("MinZoom", typeof(double), 0.1);


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double MaxZoom
        {
            get
            {
                return GetValue<double>(MaxZoomProperty);
            }
            set
            {
                SetValue(MaxZoomProperty, value);
            }
        }

        /// <summary>
        /// Register the MaxZoom property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MaxZoomProperty = RegisterProperty("MaxZoom", typeof(double), 2);


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsFilterApplied
        {
            get
            {
                return GetValue<bool>(IsFilterAppliedProperty);
            }
            set
            {
                SetValue(IsFilterAppliedProperty, value);
            }
        }

        /// <summary>
        /// Register the IsFilterApplied property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsFilterAppliedProperty = RegisterProperty("IsFilterApplied", typeof(bool), null, (sender, e) => ((GraphExplorerViewModel)sender).OnIsFilterAppliedChanged());

        /// <summary>
        /// Called when the IsFilterApplied property has changed.
        /// </summary>
        private void OnIsFilterAppliedChanged()
        {
            ClearFilterCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IEnumerable<DataVertex> Vertexes
        {
            get
            {
                return GetValue<IEnumerable<DataVertex>>(VertexesProperty);
            }
            set
            {
                SetValue(VertexesProperty, value);
            }
        }

        /// <summary>
        /// Register the Vertexes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData VertexesProperty = RegisterProperty("Vertexes", typeof(IEnumerable<DataVertex>), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IEnumerable<DataEdge> Edges
        {
            get
            {
                return GetValue<IEnumerable<DataEdge>>(EdgesProperty);
            }
            set
            {
                SetValue(EdgesProperty, value);
            }
        }

        /// <summary>
        /// Register the Edges property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EdgesProperty = RegisterProperty("Edges", typeof(IEnumerable<DataEdge>), null);

        public GraphExplorerStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsAddingNewEdge
        {
            get
            {
                return GetValue<bool>(IsAddingNewEdgeProperty);
            }
            set
            {
                SetValue(IsAddingNewEdgeProperty, value);
            }
        }

        /// <summary>
        /// Register the IsAddingNewEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsAddingNewEdgeProperty = RegisterProperty("IsAddingNewEdge", typeof(bool), false);

        // TODO: Initialize GraphDataService via constructor
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IGraphDataService GraphDataService
        {
            get
            {
                return GetValue<IGraphDataService>(GraphDataServiceProperty);
            }
            set
            {
                SetValue(GraphDataServiceProperty, value);
            }
        }

        /// <summary>
        /// Register the GraphDataService property so it is known in the class.
        /// </summary>
        public static readonly PropertyData GraphDataServiceProperty = RegisterProperty("GraphDataService", typeof(IGraphDataService), null, (sender, e) => ((GraphExplorerViewModel)sender).OnGraphDataServiceChanged());

        /// <summary>
        /// Called when the GraphDataService property has changed.
        /// </summary>
        private void OnGraphDataServiceChanged()
        {
            GetEdges();
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsOverallTabSelected
        {
            get
            {
                return GetValue<bool>(IsOverallTabSelectedProperty);
            }
            set
            {
                SetValue(IsOverallTabSelectedProperty, value);
            }
        }

        /// <summary>
        /// Register the IsOverallTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsOverallTabSelectedProperty = RegisterProperty("IsOverallTabSelected", typeof(bool), true);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanDrag
        {
            get
            {
                return GetValue<bool>(CanDragProperty);
            }
            set
            {
                SetValue(CanDragProperty, value);
            }
        }

        /// <summary>
        /// Register the CanDrag property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanDragProperty = RegisterProperty("CanDrag", typeof(bool), false, (sender, e) => ((GraphExplorerViewModel)sender).OnCanDragChanged());

        /// <summary>
        /// Called when the CanDrag property has changed.
        /// </summary>
        private void OnCanDragChanged()
        {
            UpdateCanDrag(CanDrag);
        }


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsInEditing
        {
            get
            {
                return GetValue<bool>(IsInEditingProperty);
            }
            set
            {
                SetValue(IsInEditingProperty, value);
            }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), false, (sender, e) => ((GraphExplorerViewModel)sender).OnIsInEditingChanged());

        /// <summary>
        /// Called when the IsInEditing property has changed.
        /// </summary>
        private void OnIsInEditingChanged()
        {
            SelectedVertices.Clear();
            UpdateIsInEditing(IsInEditing);
            if (IsInEditing)
            {
                CanDrag = true;
                PostStatusMessage("Edit Mode");
            }
            else
            {
                PostStatusMessage("Exit Edit Mode");
            }
        }

        /*public IGraphDataService GraphDataService
        {
            get { return GetValue<IGraphDataService>(GraphDataServiceProperty); }
            set { SetValue(GraphDataServiceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphDataService.  This enables animation, styling, binding, etc...
        public static readonly PropertyData GraphDataServiceProperty =
            DependencyProperty.Register("GraphDataService", typeof(IGraphDataService), typeof(GraphExplorerView), new PropertyMetadata(null, GraphDataServiceChanged));

        static void GraphDataServiceChanged(PropertyData d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
              //  ((GraphExplorerView)d).GetEdges();
                throw new NotImplementedException();
            }
        }*/


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public OperationObserver OperationObserver
        {
            get { return GetValue<OperationObserver>(OperationObserverProperty); }
            set { SetValue(OperationObserverProperty, value); }
        }

        /// <summary>
        /// Register the OperationObserver property so it is known in the class.
        /// </summary>
        public static readonly PropertyData OperationObserverProperty = RegisterProperty("OperationObserver", typeof(OperationObserver), null);


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("OperationObserver")]
        public bool HasChange
        {
            get { return GetValue<bool>(HasChangeProperty); }
            set { SetValue(HasChangeProperty, value); }
        }

        /// <summary>
        /// Register the HasChange property so it is known in the class.
        /// </summary>
        public static readonly PropertyData HasChangeProperty = RegisterProperty("HasChange", typeof(bool), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("OperationObserver")]
        public string OperationStatus
        {
            get { return GetValue<string>(OperationStatusProperty); }
            set { SetValue(OperationStatusProperty, value); }
        }

        /// <summary>
        /// Register the OperationStatus property so it is known in the class.
        /// </summary>
        public static readonly PropertyData OperationStatusProperty = RegisterProperty("OperationStatus", typeof(string), string.Empty,
            (sender, e) => ((GraphExplorerViewModel)sender).OnOperationStatusChanged());

        private void OnOperationStatusChanged()
        {
            PostStatusMessage(OperationStatus);
        }

        public GraphExplorerView View
        {
            get
            {
                if (_view == null)
                {
                    var viewLocator = (IViewLocator)ServiceLocator.Default.ResolveType(typeof(IViewLocator));
                    Type viewType = viewLocator.ResolveView(GetType());
                    _view = (GraphExplorerView)ServiceLocator.Default.ResolveType(viewType);
                }
                return _view;
            }
        }

        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            MinZoom = 0.1;
            MaxZoom = 2;
            MinZoomNav = 0.1;
            MaxZoomNav = 2;

            ApplySetting(Logic);
            ApplySetting(NavLogic, true);

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

            if (removeFromSelected && v != null && SelectedVertices.Contains(v.Id))
            {
                SelectedVertices.Remove(v.Id);
            }
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



        private void miDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            var eCtrl = (sender as MenuItem).Tag as EdgeControl;
            if (eCtrl != null)
            {
                var edge = eCtrl.Edge as DataEdge;

                var op = new DeleteEdgeOperation(View.Area, edge.Source, edge.Target, edge, ec =>
                {
                    //do nothing
                }, ec =>
                {
                    //do nothing
                });

                OperationObserver.Do(op);
            }
            //throw new NotImplementedException();
        }

        private void NavigateTo(DataVertex dataVertex, BidirectionalGraph<DataVertex, DataEdge> overrallGraph)
        {
            //overrallGraph.get
            NavigateHistoryItem historyItem = GetHistoryItem(dataVertex, overrallGraph);

            CreateGraphArea(View.AreaNav, historyItem.Vertexes, historyItem.Edges, 0);

            //var dispatcher = AreaNav.Dispatcher;

            //FitToBounds(dispatcher, zoomctrlNav);
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

        private void ApplySetting(GraphLogic logic, bool nav = false)
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



        private void miDeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            var vCtrl = (sender as MenuItem).Tag as VertexControl;
            if (vCtrl != null)
            {
                var op = new DeleteVertexOperation(View.Area, vCtrl.Vertex as DataVertex, (dv, vc) => { }, dv => { View.Area.RelayoutGraph(true); });

                OperationObserver.Do(op);
            }
        }



        private void SelectVertex(VertexControl vc)
        {
            var v = vc.Vertex as DataVertex;
            if (v == null)
            {
                return;
            }

            if (SelectedVertices.Contains(v.Id))
            {
                SelectedVertices.Remove(v.Id);
                HighlightBehaviour.SetHighlighted(vc, false);
                //DragBehaviour.SetIsTagged(vc, false);
            }
            else
            {
                SelectedVertices.Add(v.Id);
                HighlightBehaviour.SetHighlighted(vc, true);
                //DragBehaviour.SetIsTagged(vc, true);
            }
        }
        

        private void UpdateIsInEditing(bool value)
        {
            if (View == null)
            {
                return;
            }

            GraphArea area = View.Area;
            bool highlightEnable = !value;
            bool highlighted = false;
            foreach (var vertex in Logic.Graph.Vertices)
            {
                vertex.IsEditing = value;

                View.SetIsHighlightEnabled(GraphExplorerTab.Main, vertex, highlightEnable);
                View.SetHighlighted(GraphExplorerTab.Main, vertex, highlighted);
            }

            foreach (var edge in Logic.Graph.Edges)
            {
                View.SetIsHighlightEnabled(GraphExplorerTab.Main, edge, highlightEnable);
                View.SetHighlighted(GraphExplorerTab.Main, edge, highlighted);
            }
        }

        private void UpdateCanDrag(bool value)
        {
            if (View == null)
            {
                return;
            }

            GraphArea area = View.Area;
            foreach (var vertex in Logic.Graph.Vertices)
            {
                View.SetIsDragEnabled(GraphExplorerTab.Main, vertex, value);               
            }
        }

        //Summary
        //    handler of FilteredEntities's CollectionChanged event
        private void FilteredEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (FilterEntity item in e.NewItems.OfType<FilterEntity>())
                    {
                        if (IsHideVertexes)
                        {
                            UpdateVertexVisibility(item);
                        }
                        else
                        {
                            UpdateVertexIsEnabled(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (IsHideVertexes)
                    {
                        SetVertexesIsEnabled(true);
                        SetVertexesVisibility(false);
                        UpdateEdgesVisibility();
                    }
                    else
                    {
                        SetVertexesVisibility(true);
                        SetVertexesIsEnabled(false);
                    }
                    IsFilterApplied = true;
                    break;
            }
            //throw new NotImplementedException();
        }

        private void UpdateEdgesVisibility()
        {
            RunCodeInUiThread(() =>
            {
                if (_edges != null)
                {
                    foreach (DataEdge edge in _edges)
                    {
                        if (edge.Target != null && !edge.Target.IsVisible)
                        {
                            edge.IsVisible = false;
                            continue;
                        }

                        if (edge.Source != null && !edge.Source.IsVisible)
                        {
                            edge.IsVisible = false;
                            continue;
                        }

                        edge.IsVisible = true;
                    }
                }
            }, null, DispatcherPriority.Loaded);
        }

        private void UpdateVertexIsEnabled(FilterEntity item)
        {
            DataVertex vertex = _vertexes.FirstOrDefault(v => v.Id == item.Vertex.Id);
            if (vertex != null)
            {
                vertex.IsEnabled = true;
            }
        }

        private void SetVertexesIsEnabled(bool value)
        {
            if (_vertexes == null)
            {
                return;
            }

            foreach (DataVertex vertex in _vertexes)
            {
                vertex.IsEnabled = value;
            }
        }

        //Summary
        //    Update visibility of  specific vertex
        private void UpdateVertexVisibility(FilterEntity item)
        {
            if (_edges == null)
            {
                return;
            }
            //throw new NotImplementedException();
            DataVertex vertex = _vertexes.FirstOrDefault(v => v.Id == item.Vertex.Id);
            if (vertex != null)
            {
                vertex.IsVisible = true;

                //foreach (var edgeIn in _edges.Where(e => e.Target.Id == vertex.Id))
                //{
                //    if (edgeIn.Source.IsVisible)
                //        edgeIn.IsVisible = true;
                //    else
                //        edgeIn.IsVisible = false;
                //}

                //foreach (var edgeOut in _edges.Where(e => e.Source.Id == vertex.Id))
                //{
                //    if (edgeOut.Target.IsVisible)
                //        edgeOut.IsVisible = true;
                //    else
                //        edgeOut.IsVisible = false;
                //}
            }
        }

        //Summary
        //    hide all vertexes in graph
        private void SetVertexesVisibility(bool value)
        {
            //throw new NotImplementedException();
            if (_vertexes == null)
            {
                return;
            }

            foreach (DataVertex vertex in _vertexes)
            {
                vertex.IsVisible = value;
            }

            if (_edges != null)
            {
                foreach (DataEdge edge in _edges)
                {
                    edge.IsVisible = value;
                }
            }
        }        

        //Summary
        //    Commit changes to data source, after commit, clear undo/redo list
        public void Commit()
        {
            SelectedVertices.Clear();
            OperationObserver.Clear();
            
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
            HasChange = false;

            IsInEditing = false;

            PostStatusMessage("Ready");
        }

        public PathGeometry EdGeometry { get; private set; }
        #endregion

        #region Commands

        #region Fields
        private Command _clearFilterCommand;

        private GraphExplorerView _view;
        #endregion

        #region Methods
        private void ExecuteClearFilter()
        {
            if (IsFilterApplied)
            {
                DisApplyFilter();
            }
        }

        public void CreateEdge(int fromId, int toId)
        {
            if (View == null)
            {
                return;
            }

            GraphArea area = View.Area;
            DataVertex source = Logic.Graph.Vertices.FirstOrDefault(x => x.Id == fromId);
            DataVertex target = Logic.Graph.Vertices.FirstOrDefault(x => x.Id == toId);
            if (source == null || target == null)
            {
                return;
            }

            OperationObserver.Do(new CreateEdgeOperation(area, source, target, e =>
            {
                //on vertex created
                //SelectedVertices.Add(v.Id);

                HighlightBehaviour.SetIsHighlightEnabled(e, false);
                HighlightBehaviour.SetHighlighted(e, false);

                View.SetHighlighted(GraphExplorerTab.Main, source, false);
                View.SetHighlighted(GraphExplorerTab.Main, target, false);

                //UpdateIsInEditing(true);
            }, e =>
            {
                //SelectedVertices.Remove(v.Id);
                //on vertex recreated
            }));
        }

        public List<int> SelectedVertices { get; private set; }

        #endregion

        #endregion

        #region IObserver<IOperation>
        
        #region Methods
        public void OnVertexLoaded(IEnumerable<DataVertex> vertexes, bool clearHistory = false)
        {
            if (clearHistory)
            {
                _observers.Clear();
            }

            if (vertexes == null)
            {
                return;
            }

            _vertexes = vertexes;

            if (View != null)
            {
                _edges = View.Area.EdgesList.Keys;
            }

            foreach (DataVertex vertex in vertexes)
            {
                _observers.Add(vertex.Subscribe(OperationObserver));
                vertex.OnPositionChanged -= vertex_OnPositionChanged;
                vertex.OnPositionChanged += vertex_OnPositionChanged;
            }

            Entities = new ObservableCollection<FilterEntity>(FilterEntity.GenerateFilterEntities(vertexes));

            SetVertexesVisibility(true);

            PostStatusMessage("Ready");
        }

        private void vertex_OnPositionChanged(object sender, DataVertex.VertexPositionChangedEventArgs e)
        {
            if (View == null || View.Area == null)
            {
                return;
            }

            var vertex = (DataVertex)sender;
            if (Logic.Graph.Vertices.Any(v => v.Id == vertex.Id))
            {
                VertexControl vc = View.Area.VertexList.First(v => v.Key.Id == vertex.Id).Value;
                //throw new NotImplementedException();
                OperationObserver.OnNext(new VertexPositionChangeOperation(View.Area, vc, e.OffsetX, e.OffsetY, vertex));
            }
        }
        #endregion

        #endregion


        #region Post Status Message
        public void PostStatusMessage(string message)
        {
            StatusMessage = message;
        }
        #endregion

        #region Filter Nodes
        private void ApplyFilter(string filterText)
        {
            if (Entities == null)
            {
                return;
            }

            if (String.IsNullOrEmpty(filterText))
            {
                DisApplyFilter();
                return;
            }

            FilteredEntities.Clear();

            foreach (FilterEntity entity in Entities)
            {
                if (entity.Title.Contains(filterText) || (!String.IsNullOrEmpty(entity.FirstName) && entity.LastName.Contains(filterText)))
                {
                    if (!FilteredEntities.Contains(entity))
                    {
                        FilteredEntities.Add(entity);
                    }
                }
            }
            //throw new NotImplementedException();
        }

        private void DisApplyFilter()
        {
            FilteredEntities.Clear();

            foreach (FilterEntity entity in Entities)
            {
                FilteredEntities.Add(entity);
            }

            IsFilterApplied = false;
        }
        #endregion

        #region Methods
        private void OnRelayoutFinished(GraphExplorerTab tab)
        {
            View.ShowAllEdgesLabels(tab, true);

            View.FitToBounds(tab);

            View.SetVertexPropertiesBinding(tab);
        }
        #endregion // Methods
    }
}