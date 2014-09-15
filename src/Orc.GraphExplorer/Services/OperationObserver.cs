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

    public class OperationStacks
    {
        public OperationStacks()
        {
            ForRedo = new Stack<IOperation>();
            ForUndo = new Stack<IOperation>();
        }
        public Stack<IOperation> ForUndo { get; private set; }

        public Stack<IOperation> ForRedo { get; private set; }
    }

    public class OperationObserver : IOperationObserver
    {
        #region Fields
        private readonly IDictionary<EditorData, OperationStacks> _operations;
        #endregion

        #region Constructors
        public OperationObserver()
        {
            _operations = new Dictionary<EditorData, OperationStacks>();
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
        public void Undo(EditorData editorData)
        {
            var stackPair = _operations[editorData];

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

            UpdateEditor(editorData, stackPair);

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                editorData.OperationStatus = "Undo " + operation.Sammary;
            }
        }

        private static void UpdateEditor(EditorData editorData, OperationStacks stackPair)
        {
            editorData.HasRedoable = stackPair.ForRedo.Any(o => o.IsUnDoable);
            editorData.HasUndoable = stackPair.ForUndo.Any(o => o.IsUnDoable);
        }

        public void Redo(EditorData editorData)
        {
            var stackPair = _operations[editorData];
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

            UpdateEditor(editorData, stackPair);

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                editorData.OperationStatus = "Redo " + operation.Sammary;
            }
        }

        public void Clear(EditorData editorData)
        {
            _operations.Remove(editorData);
        }
        #endregion
    }
}