namespace Orc.GraphExplorer.DomainModel
{
    using System;
    using System.ComponentModel;
    using GraphX;
    using Models;
    using YAXLib;

    [Serializable]
    public class DataEdge : EdgeBase<DataVertex>,INotifyPropertyChanged, IDisposable
    {
        #region Properties
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
        #endregion
        
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
            : base(source, target, weight)
        {
        }

        public DataEdge()
            : base(null, null, 1)
        {
        }

        /// <summary>
        /// Node main description (header)
        /// </summary>
        public string Text { get; set; }
        public string ToolTipText { get; set; }
        public object Tag { get; set; }

        public override string ToString()
        {
            return Text;
        }

        [YAXDontSerialize]
        public DataEdge Self
        {
            get { return this; }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
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
    }
}
