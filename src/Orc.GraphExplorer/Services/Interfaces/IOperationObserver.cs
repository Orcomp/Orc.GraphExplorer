#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IOperationObserver.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services.Interfaces
{
    using System;
    using Operations.Interfaces;

    public interface IOperationObserver : IObserver<IOperation>
    {
        void Do(IOperation operation);
        void Undo();
        void Redo();
        void Clear();
    }
}