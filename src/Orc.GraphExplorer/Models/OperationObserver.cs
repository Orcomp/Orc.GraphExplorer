namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;
    using Operations.Interfaces;
    using Services.Interfaces;

    public class OperationObserver : ModelBase, IOperationObserver
    {
        #region Fields

        private List<IOperation> _operations;
        private List<IOperation> _operationsRedo;

        #endregion // Fields

        #region Constructors
        public OperationObserver()
        {
            _operationsRedo = new List<IOperation>();
            _operations = new List<IOperation>();
        }
        #endregion // Constructors

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
            HasChange = true;
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
                OperationStatus = operation.Sammary;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string OperationStatus
        {
            get { return GetValue<string>(OperationStatusProperty); }
            set { SetValue(OperationStatusProperty, value); }
        }

        /// <summary>
        /// Register the OperationStatus property so it is known in the class.
        /// </summary>
        public static readonly PropertyData OperationStatusProperty = RegisterProperty("OperationStatus", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool HasChange
        {
            get { return GetValue<bool>(HasChangeProperty); }
            set { SetValue(HasChangeProperty, value); }
        }

        /// <summary>
        /// Register the HasChange property so it is known in the class.
        /// </summary>
        public static readonly PropertyData HasChangeProperty = RegisterProperty("HasChange", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool HasUndoable
        {
            get { return GetValue<bool>(HasUndoableProperty); }
            set { SetValue(HasUndoableProperty, value); }
        }

        /// <summary>
        /// Register the HasUndoable property so it is known in the class.
        /// </summary>
        public static readonly PropertyData HasUndoableProperty = RegisterProperty("HasUndoable", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool HasRedoable
        {
            get { return GetValue<bool>(HasRedoableProperty); }
            set { SetValue(HasRedoableProperty, value); }
        }

        /// <summary>
        /// Register the HasRedoable property so it is known in the class.
        /// </summary>
        public static readonly PropertyData HasRedoableProperty = RegisterProperty("HasRedoable", typeof(bool), () => false);

        private void UpdateHasUndoable()
        {
            HasUndoable = _operations.Any(o => o.IsUnDoable);
        }

        private void UpdateHasRedoable()
        {
            HasRedoable = _operationsRedo.Any(o => o.IsUnDoable);
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
                OperationStatus = "Undo " + op.Sammary;
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
                OperationStatus = "Redo " + op.Sammary;
            }
        }

        public void Clear()
        {
            _operations.Clear();
            _operationsRedo.Clear();
        }
    }
}