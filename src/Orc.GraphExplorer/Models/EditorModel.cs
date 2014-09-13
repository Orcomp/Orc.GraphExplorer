namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;
    using Catel.IoC;
    using Operations.Interfaces;
    using Services;
    using Services.Interfaces;

    public class EditorModel : ModelBase
    {
        #region Constructors
        public EditorModel()
        {
            var operationObserver = new OperationObserver(this);
            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterInstance(typeof(IOperationObserver), operationObserver);
        }
        #endregion // Constructors              

       // public IOperationObserver OperationObserver { get; private set; }

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
    }
}