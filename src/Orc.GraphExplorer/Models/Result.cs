using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Models
{
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
