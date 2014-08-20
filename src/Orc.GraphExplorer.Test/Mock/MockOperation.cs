using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Tests.Mock
{
    using Operations.Interfaces;
    using Orc.GraphExplorer.Enums;

    class MockOperation : IOperation
    {
        bool summaryCalled;

        public bool SummaryCalled
        {
            get { return summaryCalled; }
        }

        public string Sammary
        {
            get
            {
                summaryCalled = true;
                return "Summary";
            }
        }

        bool doCalled;

        public bool DoCalled
        {
            get { return doCalled; }
        }

        public void Do()
        {
            doCalled = true;
        }

        bool undoCalled;

        public bool UndoCalled
        {
            get { return undoCalled; }
        }

        public void UnDo()
        {
            undoCalled = true;
        }

        bool isUnDoable = true;
        public bool IsUnDoable
        {
            get { return isUnDoable; }
            set { isUnDoable = value; }
        }

        public OperationStatus OperationStatus
        {
            get;
            protected set;
        }

        bool isDisposed;
        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            //throw new NotImplementedException();
        }
    }
}
