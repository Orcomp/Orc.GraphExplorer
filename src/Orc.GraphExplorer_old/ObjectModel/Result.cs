namespace Orc.GraphExplorer.ObjectModel
{
    using System;

    public class Result
    {
        Exception _error;

        public Exception Error
        {
            get { return _error; }
        }

        public Result(Exception error)
        {
            _error = error;
        }
    }
}
