﻿using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace Orc.GraphExplorer.Model
{
    public class PropertyViewmodel : NotificationObject, IObservable<IOperation>,IDisposable
    {
        #region Properties

        int _index;

        public int Index
        {
            get { return _index; }
        }

        bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged("IsEditing");
            }
        }

        public bool IsDirty
        {
            get { return OriginalValue != Value; }
        }

        string _originalKey;

        public string OriginalKey
        {
            get { return _originalKey; }
        }

        string _key;

        public string Key
        {
            get { return _key; }
            set
            {
                Observe(new EditKeyPropertyOperation(this, value));
            }
        }

        string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                Observe(new EditValuePropertyOperation(this,value));

                RaisePropertyChanged("IsDirty");
            }
        }

        string _originalValue;

        public string OriginalValue
        {
            get { return _originalValue; }
            private set
            {
                _originalValue = value;
                RaisePropertyChanged("OriginalValue");
                RaisePropertyChanged("IsDirty");
            }
        }

        DataVertex _data;

        public DataVertex Data
        {
            get { return _data; }
        }

        IDisposable _dispose;

        #endregion

        public PropertyViewmodel(int index,string key, string value, DataVertex data)
        {
            _index = index;
            _originalKey = _key = key;
            _value = _originalValue = value;
            _data = data;
            _dispose = this.Subscribe(_data);
        }

        public void Reset()
        {
            Value = OriginalValue;
        }

        public void Commit()
        {
            OriginalValue = Value;
        }

        #region IObservable<IOperation>

        Dictionary<Guid, IObserver<IOperation>> _observerDic = new Dictionary<Guid, IObserver<IOperation>>();

        public IDisposable Subscribe(IObserver<IOperation> observer)
        {
            var id = Guid.NewGuid();
            _observerDic.Add(id, observer);
            return new PropertyObserverable(this, id);
        }

        public class PropertyObserverable : IDisposable
        {
            PropertyViewmodel _property;
            Guid _observerId;
            public PropertyObserverable(PropertyViewmodel property, Guid observerId)
            {
                _property = property;
                _observerId = observerId;
            }

            public void Dispose()
            {
                _property.RemoveObserver(_observerId);
                _property = null;
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

        public Tuple<string,string> UpdateKey(string key)
        {
            var oriVel = _key;
            _key = key;
            RaisePropertyChanged("Key");
            return new Tuple<string, string>(oriVel, _key);
        }

        public Tuple<string, string> UpdateValue(string value)
        {
            var oriVel = _value;
            _value = value;
            RaisePropertyChanged("Value");
            return new Tuple<string, string>(oriVel, _value);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            _dispose.Dispose();
        }
        #endregion
    }
}
