#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolBoxViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Catel.Data;
    using Catel.IoC;
    using Catel.Messaging;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    using Orc.GraphExplorer.ViewModels.Messages;
    using Orc.GraphExplorer.ViewModels.Messages.Enums;
    using Orc.GraphExplorer.Views;

    public class ToolBoxViewModel : ViewModelBase
    {
        public ToolBoxViewModel()
        {
            // var mediator = ServiceLocator.Default.ResolveType<IMessageMediator>();
            SaveToXml = new Command(OnSaveToXmlExecute);
            LoadFromXml = new Command(OnLoadFromXmlExecute);
            SaveAsImage = new Command(OnSaveAsImageExecute);
            StartDrag = new Command(OnStartDragExecute);
            UndoCommand = new Command(OnUndoCommandExecute, () => CanUndo);
            RedoCommand = new Command(OnRedoCommandExecute, () => CanRedo);
            SaveChanges = new Command(OnSaveChangesExecute);
            Refresh = new Command(OnRefreshExecute);
        }

        #region Commands

        /// <summary>
        /// Gets the SaveToXml command.
        /// </summary>
        public Command SaveToXml { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveToXml command is executed.
        /// </summary>
        private void OnSaveToXmlExecute()
        {
            SaveMessage.SendWith(StoredDataType.Xml);
        }

        /// <summary>
        /// Gets the LoadFromXml command.
        /// </summary>
        public Command LoadFromXml { get; private set; }

        /// <summary>
        /// Method to invoke when the LoadFromXml command is executed.
        /// </summary>
        private void OnLoadFromXmlExecute()
        {
            LoadMeaasge.SendWith(StoredDataType.Xml);
        }

        /// <summary>
        /// Gets the SaveAsImage command.
        /// </summary>
        public Command SaveAsImage { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveAsImage command is executed.
        /// </summary>
        private void OnSaveAsImageExecute()
        {
            SaveMessage.SendWith(StoredDataType.Image);
        }

        /// <summary>
        /// Gets the StartDrag command.
        /// </summary>
        public Command StartDrag { get; private set; }

        /// <summary>
        /// Method to invoke when the StartDrag command is executed.
        /// </summary>
        private void OnStartDragExecute()
        {
            var viewManager = ServiceLocator.Default.ResolveType<IViewManager>();
            var views = viewManager.GetViewsOfViewModel(this);
            if (views.Length != 1)
            {
                // TODO: handle this situation.
                throw new NotImplementedException();
            }

            var view = (ToolBoxView) views.First();

            StartDragMessage.SendWith(view.tbnNewNode);
        }

        /// <summary>
        /// Gets the UndoCommand command.
        /// </summary>
        public Command UndoCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the UndoCommand command is executed.
        /// </summary>
        private void OnUndoCommandExecute()
        {
            UndoMessage.SendWith(null);
        }

        /// <summary>
        /// Gets the RedoCommand command.
        /// </summary>
        public Command RedoCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the RedoCommand command is executed.
        /// </summary>
        private void OnRedoCommandExecute()
        {
            RedoMessage.SendWith(null);
        }

        /// <summary>
        /// Gets the SaveChanges command.
        /// </summary>
        public Command SaveChanges { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveChanges command is executed.
        /// </summary>
        private void OnSaveChangesExecute()
        {
            SaveChangesMessage.SendWith(null);
        }

        /// <summary>
        /// Gets the Refresh command.
        /// </summary>
        public Command Refresh { get; private set; }

        /// <summary>
        /// Method to invoke when the Refresh command is executed.
        /// </summary>
        private void OnRefreshExecute()
        {
            RefreshMessage.SendWith(null);
        }

        #endregion // Commands

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanDrag
        {
            get { return GetValue<bool>(CanDragProperty); }
            set { SetValue(CanDragProperty, value); }
        }

        /// <summary>
        /// Register the CanDrag property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanDragProperty = RegisterProperty("CanDrag", typeof (bool), false, (sender, e) => ((ToolBoxViewModel) sender).OnCanDragChanged());

        /// <summary>
        /// Called when the CanDrag property has changed.
        /// </summary>
        private void OnCanDragChanged()
        {
            CanDragChangedMessage.SendWith(CanDrag);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof (bool), false, (sender, e) => ((ToolBoxViewModel) sender).OnIsInEditingChanged());

        /// <summary>
        /// Called when the IsInEditing property has changed.
        /// </summary>
        private void OnIsInEditingChanged()
        {
            IsInEditingChangedMessage.SendWith(IsInEditing);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsDrawingEdge
        {
            get { return GetValue<bool>(IsDrawingEdgeProperty); }
            set { SetValue(IsDrawingEdgeProperty, value); }
        }

        /// <summary>
        /// Register the IsDrawingEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDrawingEdgeProperty = RegisterProperty("IsDrawingEdge", typeof (bool), false, (sender, e) => ((ToolBoxViewModel) sender).OnIsDrawingEdgeChanged());

        /// <summary>
        /// Called when the IsDrawingEdge property has changed.
        /// </summary>
        private void OnIsDrawingEdgeChanged()
        {
            IsDrawingEdgeChangedMessage.SendWith(IsDrawingEdge);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanUndo
        {
            get { return GetValue<bool>(CanUndoProperty); }
            set { SetValue(CanUndoProperty, value); }
        }

        /// <summary>
        /// Register the CanUndo property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanUndoProperty = RegisterProperty("CanUndo", typeof (bool), false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool CanRedo
        {
            get { return GetValue<bool>(CanRedoProperty); }
            set { SetValue(CanRedoProperty, value); }
        }

        /// <summary>
        /// Register the CanRedo property so it is known in the class.
        /// </summary>
        public static readonly PropertyData CanRedoProperty = RegisterProperty("CanRedo", typeof (bool), false);

        #endregion // Properties

    }
}