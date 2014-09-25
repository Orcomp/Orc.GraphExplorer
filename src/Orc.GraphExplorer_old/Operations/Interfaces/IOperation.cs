namespace Orc.GraphExplorer.Operations.Interfaces
{
    using System;
    using Enums;

    using Orc.GraphExplorer.Models;

    public interface IOperation : IDisposable
    {
        Editor Editor { get;  }

        string Sammary { get; }

        void Do();

        void UnDo();

        bool IsUnDoable { get; }

        OperationStatus OperationStatus { get; }
    }
}
