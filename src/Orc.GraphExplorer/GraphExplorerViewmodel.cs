using GraphX;
using Microsoft.Practices.Prism.Commands;
using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Orc.GraphExplorer
{
    public class GraphExplorerViewmodel : NotificationObject, IObserver<IOperation>
    {
        #region Properties

        List<IDisposable> _observers = new List<IDisposable>();
        IEnumerable<DataVertex> _vertexes;
        IEnumerable<DataEdge> _edges;

        public bool EnableEditProperty
        {
            get
            {
                return CsvGraphDataServiceConfig.Current.EnableProperty;
            }
        }

        string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }

        bool _isHideVertexes;

        public bool IsHideVertexes
        {
            get { return _isHideVertexes; }
            set { _isHideVertexes = value; RaisePropertyChanged("IsHideVertexes"); }
        }

        ObservableCollection<FilterEntity> filteredEntities = new ObservableCollection<FilterEntity>();

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

        ObservableCollection<FilterEntity> entities;

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

        private List<int> _selectedVertices = new List<int>();

        bool _isInEditing;

        public bool IsInEditing
        {
            get { return _isInEditing; }
            set
            {
                if (_isInEditing != value)
                {
                    _isInEditing = value;
                    _selectedVertices.Clear();
                    UpdateIsInEditing(_isInEditing);
                    if (_isInEditing)
                        CanDrag = true;
                    RaisePropertyChanged("IsInEditing");
                }
            }
        }

        private void UpdateIsInEditing(bool value)
        {
            if (View == null)
                return;

            var area = View.Area;
            var highlightEnable = !value;
            var highlighted = false;
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

        bool _canDrag;

        public bool CanDrag
        {
            get { return _canDrag; }
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

        private void UpdateCanDrag(bool value)
        {
            if (View == null)
                return;

            var area = View.Area;
            foreach (var item in area.VertexList)
            {
                DragBehaviour.SetIsDragEnabled(item.Value, value);
            }
            //throw new NotImplementedException();
        }

        bool _hasUnCommitChange;

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

        public GraphExplorer View { get; set; }

        IGraphDataService GraphDataService { get; set; }

        List<IOperation> _operations;

        public List<IOperation> Operations
        {
            get { return _operations; }
            set
            {
                _operations = value;
                RaisePropertyChanged("Operations");
            }
        }

        List<IOperation> _operationsRedo;

        public List<IOperation> OperationsRedo
        {
            get { return _operationsRedo; }
            set
            {
                _operationsRedo = value;
                RaisePropertyChanged("OperationsRedo");
            }
        }

        #endregion

        //Summary
        //    constructor of GraphExplorerViewmodel
        public GraphExplorerViewmodel()
        {
            _operationsRedo = new List<IOperation>();
            _operations = new List<IOperation>();

            IsHideVertexes = true;
            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;
        }

        //Summary
        //    handler of FilteredEntities's CollectionChanged event
        void FilteredEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<FilterEntity>())
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
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (IsHideVertexes)
                    {
                        SetVertexesVisibility(false);
                    }
                    else
                    {
                        SetVertexesVisibility(true);
                        SetVertexesIsEnabled(false);
                    }

                    break;
            }
            //throw new NotImplementedException();
        }

        private void UpdateVertexIsEnabled(FilterEntity item)
        {
            var vertex = _vertexes.FirstOrDefault(v => v == item.Vertex);
            if (vertex != null)
            {
                vertex.IsEnabled = true;
            }
        }

        private void SetVertexesIsEnabled(bool value)
        {
            if (_vertexes == null)
                return;

            foreach (var vertex in _vertexes)
            {
                vertex.IsEnabled = value;
            }
        }

        //Summary
        //    Update visibility of  specific vertex
        private void UpdateVertexVisibility(FilterEntity item)
        {
            if (_edges == null)
                return;
            //throw new NotImplementedException();
            var vertex = _vertexes.FirstOrDefault(v => v == item.Vertex);
            if (vertex != null)
            {
                vertex.IsVisible = true;

                foreach (var edge in _edges.Where(e => e.Source.Id == vertex.Id || e.Target.Id == vertex.Id))
                {
                    edge.IsVisible = true;
                }
            }
        }

        //Summary
        //    hide all vertexes in graph
        private void SetVertexesVisibility(bool value)
        {
            //throw new NotImplementedException();
            if (_vertexes == null)
                return;

            foreach (var vertex in _vertexes)
            {
                vertex.IsVisible = value;
            }
        }

        //Summary
        //    Execute new operaton and put the operation in to undoable list
        public void Do(IOperation operation)
        {
            operation.Do();
            HasChange = true;
            _operations.Insert(0, operation);

            foreach (var v in _operationsRedo)
            {
                v.Dispose();
            }

            _operationsRedo.Clear();

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(operation.Sammary))
                PostStatusMessage(operation.Sammary);
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

        #region Commands

        DelegateCommand _undoCommand;

        public DelegateCommand UndoCommand
        {
            get
            {
                if (_undoCommand == null)
                    _undoCommand = new DelegateCommand(ExecuteUndo, CanExecuteUndo);
                return _undoCommand;
            }
        }

        void ExecuteUndo()
        {
            var op = Operations.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
                return;

            op.UnDo();

            Operations.Remove(op);
            OperationsRedo.Insert(0, op);

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(op.Sammary))
                PostStatusMessage("Undo " + op.Sammary);
        }

        bool CanExecuteUndo()
        {
            return HasUndoable;
        }

        DelegateCommand _redoCommand;

        public DelegateCommand RedoCommand
        {
            get
            {
                if (_redoCommand == null)
                    _redoCommand = new DelegateCommand(ExecuteRedo, CanExecuteRedo);
                return _redoCommand;
            }
        }

        void ExecuteRedo()
        {
            var op = OperationsRedo.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
                return;

            op.Do();

            OperationsRedo.Remove(op);
            Operations.Insert(0, op);

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();

            if (!string.IsNullOrEmpty(op.Sammary))
                PostStatusMessage("Redo " + op.Sammary);
        }

        bool CanExecuteRedo()
        {
            return HasRedoable;
        }

        public void CreateEdge(int fromId, int toId)
        {
            if (View == null)
                return;

            var area = View.Area;
            var source = area.VertexList.Where(pair => pair.Key.Id == fromId).Select(pair => pair.Key).FirstOrDefault();
            var target = area.VertexList.Where(pair => pair.Key.Id == toId).Select(pair => pair.Key).FirstOrDefault();
            if (source == null || target == null)
                return;

            Do(new CreateEdgeOperation(area, source, target,
                (e) =>
                {
                    //on vertex created
                    //_selectedVertices.Add(v.Id);

                    HighlightBehaviour.SetIsHighlightEnabled(e, false);
                    HighlightBehaviour.SetHighlighted(e, false);

                    HighlightBehaviour.SetHighlighted(area.VertexList[source], false);
                    HighlightBehaviour.SetHighlighted(area.VertexList[target], false);

                    //UpdateIsInEditing(true);
                },
                (e) =>
                {
                    //_selectedVertices.Remove(v.Id);
                    //on vertex recreated
                }));
        }

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

        public void OnVertexLoaded(IEnumerable<DataVertex> vertexes, bool clearHistory = false)
        {
            if (clearHistory)
                _observers.Clear();

            if (vertexes == null)
                return;

            _vertexes = vertexes;

            if (View != null)
                _edges = View.Area.EdgesList.Keys;

            foreach (var vertex in vertexes)
            {
                _observers.Add(vertex.Subscribe(this));
                vertex.OnPositionChanged -= vertex_OnPositionChanged;
                vertex.OnPositionChanged += vertex_OnPositionChanged;
            }

            Entities = new ObservableCollection<FilterEntity>(FilterEntity.GenerateFilterEntities(vertexes));

            SetVertexesVisibility(true);

            PostStatusMessage("Ready");
        }

        void vertex_OnPositionChanged(object sender, DataVertex.VertexPositionChangedEventArgs e)
        {
            if (View == null || View.Area == null)
                return;

            var vertex = (DataVertex)sender;
            if (View.Area.VertexList.Keys.Any(v => v.Id == vertex.Id))
            {
                var vc = View.Area.VertexList.First(v => v.Key.Id == vertex.Id).Value;
                //throw new NotImplementedException();
                OnNext(new VertexPositionChangeOperation(View.Area, vc, e.OffsetX, e.OffsetY, vertex));
            }
        }

        #endregion

        #region Set Vertex Binding
        //Summary
        //    binding in style will be overrided in graph control, so need create binding after data loaded
        public void SetVertexPropertiesBinding()
        {
            var graph = View.Area;
            IValueConverter conv = Orc.GraphExplorer.Converter.BoolToVisibilityConverter.Instance;

            foreach (var vertex in graph.VertexList)
            {
                var bindingIsVisible = new Binding("IsVisible")
                {
                    Source = vertex.Key,
                    Mode = BindingMode.TwoWay,
                    Converter = conv
                };

                var bindingIsEnabled = new Binding("IsEnabled")
                {
                    Source = vertex.Key,
                    Mode = BindingMode.TwoWay
                };

                vertex.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                vertex.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);

                foreach (var edge in graph.GetRelatedControls(vertex.Value, GraphControlType.Edge, EdgesType.All).OfType<EdgeControl>())
                {
                    if (edge.GetBindingExpression(UIElement.VisibilityProperty) != null)
                        continue;

                    bindingIsVisible = new Binding("IsVisible")
                    {
                        Source = vertex.Key,
                        Mode = BindingMode.TwoWay,
                        Converter = conv
                    };

                    bindingIsEnabled = new Binding("IsEnabled")
                    {
                        Source = vertex.Key,
                        Mode = BindingMode.TwoWay
                    };

                    edge.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                    edge.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
                }
            }
        }
        #endregion

        #region Post Status Message
        public void PostStatusMessage(string message)
        {
            StatusMessage = message;
        }
        #endregion
    }
}
