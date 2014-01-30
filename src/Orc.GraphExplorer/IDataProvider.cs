using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public interface IDataProvider<T>
    {
        bool IsLoaded {get;}

        void Load();

        event EventHandler<DataLoadedEventArgs<T>> Loaded;
    }
}
