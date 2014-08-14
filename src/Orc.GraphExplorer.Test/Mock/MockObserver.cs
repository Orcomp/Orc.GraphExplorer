using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests.Mock
{
    class MockObserver : IObserver<IOperation>
    {
        List<IOperation> _operations = new List<IOperation>();

        public List<IOperation> Operations
        {
            get { return _operations; }
            set { _operations = value; }
        }

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
            //throw new NotImplementedException();
            _operations.Add(value);
        }
    }
}
