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

    public class OperationObserver : IOperationObserver
    {
        #region Fields
        private readonly List<IOperation> _operations;
        private readonly List<IOperation> _operationsRedo;
        private readonly EditorModel _editor;
        #endregion

        #region Constructors
        public OperationObserver(EditorModel editor)
        {
            _editor = editor;
            _operationsRedo = new List<IOperation>();
            _operations = new List<IOperation>();
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
            _editor.HasChange = true;
            _operations.Insert(0, operation);

            foreach (IOperation v in _operationsRedo)
            {
                v.Dispose();
            }

            _operationsRedo.Clear();

            UpdateHasUndoable();
            UpdateHasRedoable();

            if (!String.IsNullOrEmpty(operation.Sammary))
            {
                _editor.OperationStatus = operation.Sammary;
            }
        }
        #endregion

        #region Methods
        private void UpdateHasUndoable()
        {
            _editor.HasUndoable = _operations.Any(o => o.IsUnDoable);
        }

        private void UpdateHasRedoable()
        {
            _editor.HasRedoable = _operationsRedo.Any(o => o.IsUnDoable);
        }

        public void Undo()
        {
            IOperation op = _operations.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
            {
                return;
            }

            op.UnDo();

            _operations.Remove(op);
            _operationsRedo.Insert(0, op);

            UpdateHasUndoable();
            UpdateHasRedoable();

            if (!String.IsNullOrEmpty(op.Sammary))
            {
                _editor.OperationStatus = "Undo " + op.Sammary;
            }
        }

        public void Redo()
        {
            IOperation op = _operationsRedo.FirstOrDefault();

            if (op == null || !op.IsUnDoable)
            {
                return;
            }

            op.Do();

            _operationsRedo.Remove(op);
            _operations.Insert(0, op);

            UpdateHasUndoable();
            UpdateHasRedoable();

            if (!String.IsNullOrEmpty(op.Sammary))
            {
                _editor.OperationStatus = "Redo " + op.Sammary;
            }
        }

        public void Clear()
        {
            _operations.Clear();
            _operationsRedo.Clear();
        }
        #endregion
    }
}