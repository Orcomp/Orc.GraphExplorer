namespace Orc.GraphExplorer.Interfaces
{
    using System;
    using Events;

    public interface IDataProvider<T>
    {
        bool IsLoaded {get;}

        void Load();

        event EventHandler<DataLoadedEventArgs<T>> Loaded;
    }
}
