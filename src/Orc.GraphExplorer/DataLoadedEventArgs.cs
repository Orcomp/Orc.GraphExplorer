using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class DataLoadedEventArgs<T> : EventArgs
    {
        IEnumerable<T> _data;
        Exception _error;
        public DataLoadedEventArgs(IEnumerable<T> data, Exception error = null)
        {
            _data = data;
            _error = error;
        }

        public IEnumerable<T> Data
        {
            get
            {
                return _data;
            }
        }

        public Exception Error
        {
            get
            {
                return _error;
            }
        }
    }
}
