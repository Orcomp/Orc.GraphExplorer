namespace Orc.GraphExplorer.Common
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
