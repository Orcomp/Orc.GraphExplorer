#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVertex.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;

    using Catel.Data;
    using Catel.MVVM;

    using GraphX;

    using Orc.GraphExplorer.Events;
    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Operations.Interfaces;
    using Orc.GraphExplorer.ViewModels;

    using YAXLib;

    public class DataVertex : ModelBase, IGraphXVertex, IDisposable, IObservable<IOperation>, IObserver<IOperation>
    {
        #region Constants
        private static int _totalCount = 0;

        private static int _maxId;

        private static readonly Random Rand = new Random();
        #endregion

        #region Fields
        private double originalX = double.NaN;

        private double originalY = double.NaN;

        private bool _isEnabled = true;

        private bool _enableEditProperty;

        private bool _isVisible = true;

        private double x;

        private double y;

        private bool _trackDragging = true;

        private bool _dragStartX;

        private bool _dragStartY;

        private bool _isDragging;

        private bool isSelected;

        private bool _isExpanded;

        private bool isInEditMode;

        private bool _isEditing;

        private Dictionary<string, string> _properties;

        private ObservableCollection<PropertyViewModel> propertiesVMs = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        public DataVertex()
            : this(-1, "")
        {
        }

        public DataVertex(int id, string title = "")
        {
            ID = id;

            Title = (title == string.Empty) ? id.ToString() : title;

            _totalCount++;
            _maxId = id > _maxId ? id : _maxId;

            EnableEditProperty = GraphExplorerSection.Current.CsvGraphDataServiceConfig.EnableProperty;
            GraphExplorerSection.ConfigurationChanged += GraphExplorerSection_ConfigurationChanged;
        }
        #endregion

        #region Properties
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

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        public double X
        {
            get
            {
                return x;
            }
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

        public double Y
        {
            get
            {
                return y;
            }
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

        public bool TrackDragging
        {
            get
            {
                return _trackDragging;
            }
            set
            {
                _trackDragging = value;
                RaisePropertyChanged("TrackDragging");
            }
        }

        public bool IsDragging
        {
            get
            {
                return _isDragging;
            }
            set
            {
                if (_isDragging != value)
                {
                    _isDragging = value;
                    if (_isDragging && TrackDragging)
                    {
                        DragStart();
                    }
                    else
                    {
                        DragStop();
                    }
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
                FireIsExpandedChanged();
            }
        }

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

        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                _isEditing = value;
                UpdatePropertiesIsEditing(_isEditing);
                RaisePropertyChanged("IsEditing");
                RaisePropertyChanged("EnableEditProperty");
            }
        }

        public static int TotalCount
        {
            get
            {
                return DataVertex._totalCount;
            }
        }

        public string Title { get; set; }

        public ObservableCollection<PropertyViewModel> Properties
        {
            get
            {
                if (propertiesVMs == null)
                {
                    propertiesVMs = new ObservableCollection<PropertyViewModel>();
                }
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
            get
            {
                return this;
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _properties = null;
            _totalCount--;

            GraphExplorerSection.ConfigurationChanged -= GraphExplorerSection_ConfigurationChanged;
            //if (_maxId <= this.ID+1)
            //    _maxId--;
        }
        #endregion

        #region IObservable<IOperation>

        #region Fields
        private readonly Dictionary<Guid, IObserver<IOperation>> _observerDic = new Dictionary<Guid, IObserver<IOperation>>();
        #endregion

        public IDisposable Subscribe(IObserver<IOperation> observer)
        {
            var id = Guid.NewGuid();
            _observerDic.Add(id, observer);
            return new VertexObserverable(this, id);
        }

        #region Methods
        public void RemoveObserver(Guid observerId)
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

        #region Methods
        private void FirePositionChanged(double x, double y, double offsetx, double offsety)
        {
            var handler = OnPositionChanged;
            if (handler != null)
            {
                var arg = new VertexPositionChangedEventArgs(x, y, offsetx, offsety);

                handler.Invoke(this, arg);
            }
        }
        #endregion

        public event EventHandler<VertexPositionChangedEventArgs> OnPositionChanged;

        
        #endregion

        #region IGraphXVertex Members
        /// <summary>
        /// Unique vertex ID
        /// </summary>
        public int ID { get; set; }

        public bool Equals(IGraphXVertex other)
        {
            return this == other;
        }
        #endregion

        #region Methods
        private void DragStop()
        {
            if (!IsEditing)
            {
                return;
            }

            if (double.IsNaN(originalX) || double.IsNaN(originalY))
            {
                originalX = 0;
                originalY = 0;
            }

            double offsetX = X - originalX;
            double offsetY = y - originalY;

            if (offsetX != 0 || offsetY != 0)
            {
                FirePositionChanged(X, Y, offsetX, offsetY);
            }
        }

        private void DragStart()
        {
            _dragStartX = true;
            _dragStartY = true;
        }

        private void FireIsExpandedChanged()
        {
            var handler = IsExpandedChanged;
            if (handler != null)
            {
                handler.Invoke(this, new EventArgs());
            }
        }

        private void UpdatePropertiesIsEditing(bool isEditing)
        {
            if (Properties == null)
            {
                return;
            }

            foreach (var vm in Properties)
            {
                vm.IsEditing = isEditing;

                if (!isEditing)
                {
                    vm.IsSelected = false;
                }
            }
        }

        public override string ToString()
        {
            return Title;
        }

        private void GraphExplorerSection_ConfigurationChanged(object sender, EventArgs e)
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

        public void Commit()
        {
            if (Properties == null)
            {
                return;
            }

            foreach (var p in Properties)
            {
                p.Commit();
            }
        }

        public override bool Equals(object obj)
        {
            var o = obj as IGraphXVertex;
            if (o == null)
            {
                return false;
            }
            else
            {
                return ID == o.ID;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Commands

        #region Fields
        private Command _addCommand;

        private Command _deleteCommand;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlightEnabled
        {
            get { return GetValue<bool>(IsHighlightEnabledProperty); }
            set { SetValue(IsHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlightEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightEnabledProperty = RegisterProperty("IsHighlightEnabled", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsDragEnabled
        {
            get { return GetValue<bool>(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsDragEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDragEnabledProperty = RegisterProperty("IsDragEnabled", typeof(bool), () => false);
        #endregion // Properties

        public Command AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new Command(ExecAdd, CanExecAdd);
                }
                return _addCommand;
            }
        }

        public Command DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new Command(ExecDelete, CanExecDelete);
                }
                return _deleteCommand;
            }
        }
        #endregion

        #region Methods
        private void ExecAdd()
        {
            //AddProperty(NewProperty());

            throw new NotImplementedException();
            /*  var apo = new AddPropertyOperation(null, this);

            Observe(apo);*/
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
            {
                Properties = new ObservableCollection<PropertyViewModel>();
            }

            if (property == null)
            {
                property = NewProperty();
            }

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
            {
                return null;
            }

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

        private void ExecDelete()
        {
            throw new NotImplementedException();
            /*var dpo = new DeletePropertyOperation(this);

            Observe(dpo);*/
            //RemoveSelectedProperties();
        }

        private bool CanExecDelete()
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
            {
                return;
            }

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

        private bool CanExecAdd()
        {
            return true;
        }
        #endregion

        public event EventHandler IsExpandedChanged;

        public event EventHandler ChangedCommited;
    }
}