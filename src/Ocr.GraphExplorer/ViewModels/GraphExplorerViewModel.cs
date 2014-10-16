#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Behaviors.Interfaces;
    using Catel.Data;
    using Catel.Memento;
    using Catel.MVVM;
    using Models.Data;
    using Orc.GraphExplorer.Models;

    public class GraphExplorerViewModel : ViewModelBase, IGraphNavigator
    {
        public GraphExplorerViewModel()
        {
            
        }
        private readonly IMementoService _mementoService;

        public GraphExplorerViewModel(IMementoService mementoService)
        {
            _mementoService = mementoService;
            Explorer = new Explorer(_mementoService);

            CloseNavTabCommand = new Command(OnCloseNavTabCommandExecute);
        }

        /// <summary>
        /// Gets the CloseNavTabCommand command.
        /// </summary>
        public Command CloseNavTabCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the CloseNavTabCommand command is executed.
        /// </summary>
        private void OnCloseNavTabCommandExecute()
        {
            IsNavTabVisible = false;
            IsNavTabSelected = false;
            IsEditorTabSelected = true;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Explorer Explorer
        {
            get { return GetValue<Explorer>(ExplorerProperty); }
            private set { SetValue(ExplorerProperty, value); }
        }

        /// <summary>
        /// Register the Explorer property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ExplorerProperty = RegisterProperty("Explorer", typeof (Explorer));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabVisible
        {
            get { return GetValue<bool>(IsNavTabVisibleProperty); }
            set { SetValue(IsNavTabVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsNavTabVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabVisibleProperty = RegisterProperty("IsNavTabVisible", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsNavTabSelected
        {
            get { return GetValue<bool>(IsNavTabSelectedProperty); }
            set { SetValue(IsNavTabSelectedProperty, value); }
        }

        /// <summary>
        /// Register the IsNavTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsNavTabSelectedProperty = RegisterProperty("IsNavTabSelected", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsEditorTabSelected
        {
            get { return GetValue<bool>(IsEditorTabSelectedProperty); }
            set { SetValue(IsEditorTabSelectedProperty, value); }
        }

        /// <summary>
        /// Register the IsEditorTabSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsEditorTabSelectedProperty = RegisterProperty("IsEditorTabSelected", typeof (bool), () => true);

        public void NavigateTo(DataVertex dataVertex)
        {
            IsNavTabVisible = true;
            IsNavTabSelected = true;
            Explorer.NavigateTo(dataVertex);
        }
    }
}