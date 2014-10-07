#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel.Data;
    using Catel.Memento;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class GraphExplorerViewModel : ViewModelBase
    {
        private readonly IMementoService _mementoService;

        public GraphExplorerViewModel(IMementoService mementoService)
        {
            _mementoService = mementoService;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Explorer = new Explorer(_mementoService);
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
        public static readonly PropertyData ExplorerProperty = RegisterProperty("Explorer", typeof(Explorer));
    }
}