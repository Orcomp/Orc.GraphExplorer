using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public interface IOperation : IDisposable
    {
        string Sammary { get; }

        void Do();

        void UnDo();

        bool IsUnDoable { get; }

        Status Status { get; }
    }

    public enum Status
    {
        Init,
        Done,
        Undo,
        Redo
    }
}
