namespace Orc.GraphExplorer.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Media;

    using Catel.MVVM;

    using GraphX;

    using Orc.GraphExplorer.Operations;
    using Orc.GraphExplorer.Operations.Interfaces;
    using Orc.GraphExplorer.ViewModels;

    using YAXLib;

    public class DataVertex : VertexBase, INotifyPropertyChanged, IDisposable, IObservable<IOperation>, IObserver<IOperation>
    {
        #region Properties

        double originalX = double.NaN;
        double originalY = double.NaN;

        bool _isEnabled = true;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        bool _enableEditProperty;

        public bool EnableEditProperty
        {
            get
            {
                return _enableEditProperty && IsEditing;
            }
            set
            {
                _enableEditProperty = value;
                RaisePropertyChanged("EnableEditProperty");
            }
        }

        bool _isVisible = true;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        double x;

        public double X
        {
            get { return x; }
            set
            {
                x = value;

                if (_dragStartX)
                {
                    originalX = x;
                    _dragStartX = false;
                }

                RaisePropertyChanged("X");
            }
        }

        double y;

        public double Y
        {
            get { return y; }
            set
            {
                y = value;

                if (_dragStartY)
                {
                    originalY = y;
                    _dragStartY = false;
                }

                RaisePropertyChanged("Y");
            }
        }

        bool _trackDragging = true;

        public bool TrackDragging
        {
            get { return _trackDragging; }
            set
            {
                _trackDragging = value;
                RaisePropertyChanged("TrackDragging");
            }
        }

        bool _dragStartX;
        bool _dragStartY;

        bool _isDragging;

        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {
                if (_isDragging != value)
                {
                    _isDragging = value;
                    if (_isDragging && TrackDragging)
                        DragStart();
                    else
                        DragStop();
                }
            }
        }

        private void DragStop()
        {
            if (!IsEditing)
                return;

            if (double.IsNaN(originalX) || double.IsNaN(originalY))
            {
                originalX = 0;
                originalY = 0;
            }

            double offsetX = X - originalX;
            double offsetY = y - originalY;

            if (offsetX != 0 || offsetY != 0)
                FirePositionChanged(X, Y, offsetX, offsetY);
        }

        private void DragStart()
        {
            _dragStartX = true;
            _dragStartY = true;
        }

        bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
                FireIsExpandedChanged();
            }
        }

        public event EventHandler IsExpandedChanged;

        void FireIsExpandedChanged()
        {
            var handler = IsExpandedChanged;
            if (handler != null)
            {
                handler.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler ChangedCommited;

        bool isInEditMode;

        public bool IsInEditMode
        {
            get
            {
                return isInEditMode;
            }
            set
            {
                isInEditMode = value;
                RaisePropertyChanged("IsInEditMode");
            }
        }

        bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                UpdatePropertiesIsEditing(_isEditing);
                RaisePropertyChanged("IsEditing");
                RaisePropertyChanged("EnableEditProperty");
            }
        }

        private void UpdatePropertiesIsEditing(bool isEditing)
        {
            if (Properties == null)
                return;

            foreach (var vm in Properties)
            {
                vm.IsEditing = isEditing;

                if (!isEditing)
                    vm.IsSelected = false;
            }
        }

        private static int _totalCount = 0;

        public static int TotalCount
        {
            get { return DataVertex._totalCount; }
        }

        private static int _maxId;

        public string Title { get; set; }
        public int Id { get; set; }

        Dictionary<string, string> _properties;
        ObservableCollection<PropertyViewModel> propertiesVMs = null;
        public ObservableCollection<PropertyViewModel> Properties
        {
            get
            {
                if (propertiesVMs == null)
                    propertiesVMs = new ObservableCollection<PropertyViewModel>();
                return propertiesVMs;
            }
            set
            {
                propertiesVMs = value;
                RaisePropertyChanged("Properties");
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        [YAXDontSerialize]
        public ImageSource Icon { get; set; }

        [YAXDontSerialize]
        public DataVertex Self
        {
            get { return this; }
        }

        public override string ToString()
        {
            return Title;
        }

        #endregion

        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        public DataVertex()
            : this(-1, "")
        {

        }

        private static readonly Random Rand = new Random();

        public DataVertex(int id, string title = "")
        {
            base.ID = id;
            Id = id;
            Title = (title == string.Empty) ? id.ToString() : title;

            _totalCount++;
            _maxId = id > _maxId ? id : _maxId;

            EnableEditProperty = GraphExplorerSection.Current.CsvGraphDataServiceConfig.EnableProperty;
            GraphExplorerSection.ConfigurationChanged += GraphExplorerSection_ConfigurationChanged;
        }

        void GraphExplorerSection_ConfigurationChanged(object sender, EventArgs e)
        {
            var config = (GraphExplorerSection)sender;

            EnableEditProperty = config.CsvGraphDataServiceConfig.EnableProperty;
        }

        public static DataVertex Create()
        {
            return new DataVertex(++_maxId);
        }

        public void SetProperties(Dictionary<string, string> dictionary)
        {
            _properties = dictionary;
            Properties = GenerateProperties(dictionary, this);
        }

        private static ObservableCollection<PropertyViewModel> GenerateProperties(Dictionary<string, string> dictionary, DataVertex data)
        {
            int index = 0;

            var pvs = from pair in dictionary select new PropertyViewModel(index++, pair.Key, pair.Value, data);

            return new ObservableCollection<PropertyViewModel>(pvs);
        }

        #region INotifyPropertyChanged
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>        

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Commands

        Command _addCommand;

        public Command AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new Command(ExecAdd, CanExecAdd);
                return _addCommand;
            }
        }

        void ExecAdd()
        {
            //AddProperty(NewProperty());

            var apo = new AddPropertyOperation(this);

            Observe(apo);
        }

        private PropertyViewModel NewProperty()
        {
            return new PropertyViewModel(GetIndex(), "", "", this) { IsEditing = true };
        }

        private int GetIndex()
        {
            return Properties.Count;
        }

        public PropertyViewModel AddProperty(PropertyViewModel property = null)
        {
            if (Properties == null)
                Properties = new ObservableCollection<PropertyViewModel>();

            if (property == null)
                property = NewProperty();

            Properties.Add(property);

            IsExpanded = true;
            DeleteCommand.RaiseCanExecuteChanged();

            return property;
        }

        public void AddPropertyRange(IEnumerable<PropertyViewModel> properties)
        {
            foreach (var p in properties)
            {
                Properties.Add(p);
            }

            Properties = new ObservableCollection<PropertyViewModel>(Properties.OrderBy(p => p.Index));
        }

        public IEnumerable<PropertyViewModel> RemoveSelectedProperties()
        {
            if (Properties == null && Properties.Any(p => p.IsSelected))
                return null;

            List<PropertyViewModel> list = new List<PropertyViewModel>();

            var deleteList = Properties.Where(p => p.IsSelected).ToList();

            foreach (var p in deleteList)
            {
                Properties.Remove(p);
                list.Add(p);
            }

            DeleteCommand.RaiseCanExecuteChanged();

            return list;
        }

        public PropertyViewModel RemoveProperty(PropertyViewModel property)
        {
            Properties.Remove(property);

            return property;
        }

        Command _deleteCommand;

        public Command DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new Command(ExecDelete, CanExecDelete);
                return _deleteCommand;
            }
        }

        void ExecDelete()
        {
            var dpo = new DeletePropertyOperation(this);

            Observe(dpo);
            //RemoveSelectedProperties();
        }

        bool CanExecDelete()
        {
            return Properties != null && Properties.Count > 0;
        }

        private void Submit()
        {
            var handler = ChangedCommited;
            if (handler != null)
            {
                ChangedCommited.Invoke(this, new EventArgs());
            }
            //throw new NotImplementedException();
        }

        private void Update(PropertyViewModel vm)
        {
            Update(vm.OriginalKey, vm.Key, vm.Value);
        }

        private void Update(string oriKey, string key, string value, Action<Result> onCompleted = null)
        {
            if (!_properties.ContainsKey(oriKey))
                return;

            bool isKeyChanged = oriKey == key;

            try
            {
                if (isKeyChanged)
                {
                    _properties.Remove(oriKey);
                    _properties.Add(key, value);
                }
                else
                {
                    _properties[oriKey] = value;
                }
            }
            catch (Exception ex)
            {
                if (onCompleted != null)
                {
                    onCompleted.Invoke(new Result(new Exception(string.Format("error occured during updating property [{0}]", oriKey), ex)));
                }
                else
                {
                    throw new Exception(string.Format("error occured during updating property [{0}]", oriKey), ex);
                }
            }

            if (onCompleted != null)
            {
                onCompleted.Invoke(new Result(null));
            }
        }

        bool CanExecAdd()
        {
            return true;
        }

        #endregion

        public void Commit()
        {
            if (Properties == null)
                return;

            foreach (var p in Properties)
            {
                p.Commit();
            }
        }

        public void Dispose()
        {
            _properties = null;
            _totalCount--;

            GraphExplorerSection.ConfigurationChanged -= GraphExplorerSection_ConfigurationChanged;
            //if (_maxId <= this.Id+1)
            //    _maxId--;
        }

        #region IObservable<IOperation>

        Dictionary<Guid, IObserver<IOperation>> _observerDic = new Dictionary<Guid, IObserver<IOperation>>();

        public IDisposable Subscribe(IObserver<IOperation> observer)
        {
            var id = Guid.NewGuid();
            _observerDic.Add(id, observer);
            return new VertexObserverable(this, id);
        }

        public class VertexObserverable : IDisposable
        {
            DataVertex _vertex;
            Guid _observerId;
            public VertexObserverable(DataVertex vertex, Guid observerId)
            {
                _vertex = vertex;
                _observerId = observerId;
            }

            public void Dispose()
            {
                _vertex.RemoveObserver(_observerId);
                _vertex = null;
            }
        }

        private void RemoveObserver(Guid observerId)
        {
            _observerDic.Remove(observerId);
        }

        private void Observe(IOperation op)
        {
            foreach (var observer in _observerDic.Values)
            {
                observer.OnNext(op);
            }
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
            Observe(value);
            //throw new NotImplementedException();
        }

        #endregion

        #region Event
        public event EventHandler<VertexPositionChangedEventArgs> OnPositionChanged;

        private void FirePositionChanged(double x, double y, double offsetx, double offsety)
        {
            var handler = OnPositionChanged;
            if (handler != null)
            {
                var arg = new VertexPositionChangedEventArgs(x, y, offsetx, offsety);

                handler.Invoke(this, arg);
            }
        }

        public class VertexPositionChangedEventArgs : EventArgs
        {
            public double X { get; private set; }
            public double Y { get; private set; }
            public double OffsetX { get; private set; }
            public double OffsetY { get; private set; }

            public VertexPositionChangedEventArgs(double x, double y, double offsetx, double offsety)
            {
                X = x;
                Y = y;
                OffsetX = offsetx;
                OffsetY = offsety;
            }
        }

        #endregion
    }
}
