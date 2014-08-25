namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Threading;

    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Converters;

    using GraphX;

    using Orc.GraphExplorer.Config;
    using Orc.GraphExplorer.DomainModel;
    using Orc.GraphExplorer.Operations;
    using Orc.GraphExplorer.Operations.Interfaces;
    using Orc.GraphExplorer.Views;

    using IValueConverter = Catel.MVVM.Converters.IValueConverter;

    public class GraphExplorerViewModel : ViewModelBase /*ObservableObject*/, IObserver<IOperation>
    {
        #region Fields
        private readonly List<IDisposable> _observers = new List<IDisposable>();

        private readonly List<int> _selectedVertices = new List<int>();

        private IEnumerable<DataVertex> _vertexes;

        private IEnumerable<DataEdge> _edges;

        private string _filterText;

        private bool _isFilterApplied;

        private string _statusMessage;

        private bool _isHideVertexes;

        private ObservableCollection<FilterEntity> filteredEntities = new ObservableCollection<FilterEntity>();

        private ObservableCollection<FilterEntity> entities;

        private bool _isInEditing;

        private bool _canDrag;

        private bool _hasUnCommitChange;

        private List<IOperation> _operations;

        private List<IOperation> _operationsRedo;
        #endregion

        #region Constructors
        public GraphExplorerViewModel()
        {
            _operationsRedo = new List<IOperation>();
            _operations = new List<IOperation>();

            IsHideVertexes = false;
            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;

            Logic = new GraphLogic();

            SaveToXml = new Command(OnSaveToXmlExecute);
        }
        #endregion

/// <summary>
/// Gets the SaveToXml command.
/// </summary>
public Command SaveToXml { get; private set; }

/// <summary>
/// Method to invoke when the SaveToXml command is executed.
/// </summary>
private void OnSaveToXmlExecute()
{
    // TODO: Handle command logic here
}

        #region Properties
        public GraphLogic Logic { get; set; }

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

        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    RaisePropertyChanged("FilterText");

                    if (IsFilterApplied)
                    {
                        ApplyFilter(_filterText);
                    }
                }
            }
        }

        public bool IsFilterApplied
        {
            get
            {
                return _isFilterApplied;
            }
            set
            {
                _isFilterApplied = value;
                RaisePropertyChanged("IsFilterApplied");
                ClearFilterCommand.RaiseCanExecuteChanged();
                //if (_isFilterApplied)
                //{
                //    ApplyFilter(FilterText);
                //    PostStatusMessage("Filter Applied");
                //}
                //else
                //{
                //    DisApplyFilter();
                //    PostStatusMessage("Filter Removed");
                //}
            }
        }

        public bool EnableEditProperty
        {
            get
            {
                return CsvGraphDataServiceConfig.Current.EnableProperty;
            }
        }

        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }

        public bool IsHideVertexes
        {
            get
            {
                return _isHideVertexes;
            }
            set
            {
                _isHideVertexes = value;
                RaisePropertyChanged("IsHideVertexes");
            }
        }

        public ObservableCollection<FilterEntity> FilteredEntities
        {
            get
            {
                return filteredEntities;
            }
            set
            {
                filteredEntities = value;
                RaisePropertyChanged("FilteredEntities");
            }
        }

        public ObservableCollection<FilterEntity> Entities
        {
            get
            {
                return entities;
            }
            set
            {
                entities = value;
                RaisePropertyChanged("Entities");
            }
        }

        public bool IsInEditing
        {
            get
            {
                return _isInEditing;
            }
            set
            {
                if (_isInEditing != value)
                {
                    _isInEditing = value;
                    _selectedVertices.Clear();
                    UpdateIsInEditing(_isInEditing);
                    if (_isInEditing)
                    {
                        CanDrag = true;
                        PostStatusMessage("Edit Mode");
                    }
                    else
                    {
                        PostStatusMessage("Exit Edit Mode");
                    }
                    RaisePropertyChanged("IsInEditing");
                }
            }
        }

        public bool CanDrag
        {
            get
            {
                return _canDrag;
            }
            set
            {
                if (_canDrag != value)
                {
                    _canDrag = value;
                    UpdateCanDrag(value);
                    RaisePropertyChanged("CanDrag");
                }
            }
        }

        public bool HasChange
        {
            get
            {
                return _hasUnCommitChange;
            }
            set
            {
                _hasUnCommitChange = value;
                RaisePropertyChanged("HasChange");
            }
        }

        public bool HasUndoable
        {
            get
            {
                return _operations.Any(o => o.IsUnDoable);
            }
        }

        public bool HasRedoable
        {
            get
            {
                return _operationsRedo.Any(o => o.IsUnDoable);
            }
        }

        public IOperation LastOperation
        {
            get
            {
                return _operations.FirstOrDefault();
            }
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

        public List<IOperation> Operations
        {
            get
            {
                return _operations;
            }
            set
            {
                _operations = value;
                RaisePropertyChanged("Operations");
            }
        }

        public List<IOperation> OperationsRedo
        {
            get
            {
                return _operationsRedo;
            }
            set
            {
                _operationsRedo = value;
                RaisePropertyChanged("OperationsRedo");
            }
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            //  GraphDataService = new CsvGraphDataService();
            base.Initialize();
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
            foreach (var v in area.VertexList)
            {
                v.Key.IsEditing = value;
                //if (value)
                //{
                //    v.Key.ChangedCommited += ChangedCommited;
                //}
                //else
                //{
                //    v.Key.ChangedCommited -= ChangedCommited;
                //}

                HighlightBehaviour.SetIsHighlightEnabled(v.Value, highlightEnable);
                HighlightBehaviour.SetHighlighted(v.Value, highlighted);
            }

            foreach (var edge in area.EdgesList)
            {
                HighlightBehaviour.SetIsHighlightEnabled(edge.Value, highlightEnable);
                HighlightBehaviour.SetHighlighted(edge.Value, highlighted);
            }
        }

        private void UpdateCanDrag(bool value)
        {
            if (View == null)
            {
                return;
            }

            GraphArea area = View.Area;
            foreach (var item in area.VertexList)
            {
                DragBehaviour.SetIsDragEnabled(item.Value, value);
            }
            //throw new NotImplementedException();
        }

        //Summary
        //    constructor of GraphExplorerViewModel

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
            GraphExplorerView.RunCodeInUiThread(() =>
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
        //    Execute new operaton and put the operation in to undoable list
        public void Do(IOperation operation)
        {
            operation.Do();
            HasChange = true;
            _operations.Insert(0, operation);

            foreach (IOperation v in _operationsRedo)
            {
                v.Dispose();
            }

            _operationsRedo.Clear();

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(operation.Sammary))
            {
                PostStatusMessage(operation.Sammary);
            }
        }

        //Summary
        //    Commit changes to data source, after commit, clear undo/redo list
        public void Commit()
        {
            _selectedVertices.Clear();
            _operations.Clear();
            _operationsRedo.Clear();

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
            HasChange = false;

            IsInEditing = false;

            PostStatusMessage("Ready");
        }
        #endregion

        #region Commands

        #region Fields
        private Command _clearFilterCommand;

        private Command _undoCommand;

        private Command _redoCommand;

        private GraphExplorerView _view;
        #endregion

        #region Properties
        public Command ClearFilterCommand
        {
            get
            {
                if (_clearFilterCommand == null)
                {
                    _clearFilterCommand = new Command(ExecuteClearFilter, CanExecuteClearFilter);
                }
                return _clearFilterCommand;
            }
        }

        public Command UndoCommand
        {
            get
            {
                if (_undoCommand == null)
                {
                    _undoCommand = new Command(ExecuteUndo, CanExecuteUndo);
                }
                return _undoCommand;
            }
        }

        public Command RedoCommand
        {
            get
            {
                if (_redoCommand == null)
                {
                    _redoCommand = new Command(ExecuteRedo, CanExecuteRedo);
                }
                return _redoCommand;
            }
        }
        #endregion

        #region Methods
        private void ExecuteClearFilter()
        {
            if (IsFilterApplied)
            {
                DisApplyFilter();
            }
        }

        private bool CanExecuteClearFilter()
        {
            return IsFilterApplied;
        }

        private void ExecuteUndo()
        {
            IOperation op = Operations.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
            {
                return;
            }

            op.UnDo();

            Operations.Remove(op);
            OperationsRedo.Insert(0, op);

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(op.Sammary))
            {
                PostStatusMessage("Undo " + op.Sammary);
            }
        }

        private bool CanExecuteUndo()
        {
            return HasUndoable;
        }

        private void ExecuteRedo()
        {
            IOperation op = OperationsRedo.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
            {
                return;
            }

            op.Do();

            OperationsRedo.Remove(op);
            Operations.Insert(0, op);

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(op.Sammary))
            {
                PostStatusMessage("Redo " + op.Sammary);
            }
        }

        private bool CanExecuteRedo()
        {
            return HasRedoable;
        }

        public void CreateEdge(int fromId, int toId)
        {
            if (View == null)
            {
                return;
            }

            GraphArea area = View.Area;
            DataVertex source = area.VertexList.Where(pair => pair.Key.Id == fromId).Select(pair => pair.Key).FirstOrDefault();
            DataVertex target = area.VertexList.Where(pair => pair.Key.Id == toId).Select(pair => pair.Key).FirstOrDefault();
            if (source == null || target == null)
            {
                return;
            }

            Do(new CreateEdgeOperation(area, source, target, e =>
            {
                //on vertex created
                //_selectedVertices.Add(v.Id);

                HighlightBehaviour.SetIsHighlightEnabled(e, false);
                HighlightBehaviour.SetHighlighted(e, false);

                HighlightBehaviour.SetHighlighted(area.VertexList[source], false);
                HighlightBehaviour.SetHighlighted(area.VertexList[target], false);

                //UpdateIsInEditing(true);
            }, e =>
            {
                //_selectedVertices.Remove(v.Id);
                //on vertex recreated
            }));
        }
        #endregion

        #endregion

        #region IObserver<IOperation>
        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnNext(IOperation value)
        {
            //throw new NotImplementedException();
            Do(value);
        }

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
                _observers.Add(vertex.Subscribe(this));
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
            if (View.Area.VertexList.Keys.Any(v => v.Id == vertex.Id))
            {
                VertexControl vc = View.Area.VertexList.First(v => v.Key.Id == vertex.Id).Value;
                //throw new NotImplementedException();
                OnNext(new VertexPositionChangeOperation(View.Area, vc, e.OffsetX, e.OffsetY, vertex));
            }
        }
        #endregion

        #endregion

        #region Set Vertex Binding
        //Summary
        //    binding in style will be overrided in graph control, so need create binding after data loaded
        public void SetVertexPropertiesBinding()
        {
            GraphArea graph = View.Area;
            IValueConverter conv = new BooleanToHidingVisibilityConverter();

            foreach (var vertex in graph.VertexList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = vertex.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = vertex.Key, Mode = BindingMode.TwoWay };

                vertex.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                vertex.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }

            foreach (var edge in graph.EdgesList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = edge.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = edge.Key, Mode = BindingMode.TwoWay };

                edge.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                edge.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }
        }
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

            if (string.IsNullOrEmpty(filterText))
            {
                DisApplyFilter();
                return;
            }

            FilteredEntities.Clear();

            foreach (FilterEntity entity in Entities)
            {
                if (entity.Title.Contains(filterText) || (!string.IsNullOrEmpty(entity.FirstName) && entity.LastName.Contains(filterText)))
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
    }
}