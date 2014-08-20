namespace Orc.GraphExplorer.Operations.Interfaces
{
    using System;
    using Enums;

    public interface IOperation : IDisposable
    {
        string Sammary { get; }

        void Do();

        void UnDo();

        bool IsUnDoable { get; }

        OperationStatus OperationStatus { get; }
    }
}
