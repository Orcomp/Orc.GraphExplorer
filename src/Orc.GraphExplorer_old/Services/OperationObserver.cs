#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationObserver.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Models;
    using Operations.Interfaces;

    using Orc.GraphExplorer.ObjectModel;

    // TODO: this class should be well tested with real operations
    public class OperationObserver : IOperationObserver
    {
        #region Fields
        private readonly IDictionary<Editor, OperationStacks> _operations;
        #endregion

        #region Constructors
        public OperationObserver()
        {
            _operations = new Dictionary<Editor, OperationStacks>();
        }
        #endregion

        #region IOperationObserver Members
        public void OnNext(IOperation value)
        {
            Do(value);
        }

        public void OnError(Exception error)
        {
            // throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            //  throw new NotImplementedException();
        }

        /// <summary>
        /// Execute new operaton and put the operation in to undoable list
        /// </summary>
        /// <param name="operation"></param>
        public void Do(IOperation operation)
        {
            operation.Do();
            operation.Editor.HasChange = true;
            if (!_operations.ContainsKey(operation.Editor))
            {
                _operations.Add(operation.Editor, new OperationStacks());
            }
            var stackPair = _operations[operation.Editor];
            stackPair.ForUndo.Push(operation);

            while (stackPair.ForRedo.Any())
            {
                stackPair.ForRedo.Pop().Dispose();
            }

            UpdateEditor(operation.Editor, stackPair);

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                operation.Editor.OperationStatus = operation.Sammary;
            }
        }
        #endregion

        #region Methods
        public void Undo(Editor editor)
        {
            var stackPair = _operations[editor];

            if (!stackPair.ForUndo.Any())
            {
                return;
            }

            var operation = stackPair.ForUndo.Pop();

            if (!operation.IsUnDoable)
            {
                return;
            }

            operation.UnDo();

            stackPair.ForRedo.Push(operation);

            UpdateEditor(editor, stackPair);

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                editor.OperationStatus = "Undo " + operation.Sammary;
            }
        }

        private static void UpdateEditor(Editor editor, OperationStacks stackPair)
        {
            editor.HasRedoable = stackPair.ForRedo.Any(o => o.IsUnDoable);
            editor.HasUndoable = stackPair.ForUndo.Any(o => o.IsUnDoable);
        }

        public void Redo(Editor editor)
        {
            var stackPair = _operations[editor];
            if (!stackPair.ForRedo.Any())
            {
                return;
            }

            var operation = stackPair.ForRedo.Pop();

            if (!operation.IsUnDoable)
            {
                return;
            }

            operation.Do();

            stackPair.ForUndo.Push(operation);

            UpdateEditor(editor, stackPair);

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                editor.OperationStatus = "Redo " + operation.Sammary;
            }
        }

        public void Clear(Editor editor)
        {
            _operations.Remove(editor);
        }
        #endregion
    }
}