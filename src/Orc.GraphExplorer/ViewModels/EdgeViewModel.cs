#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;
    using Services;

    public class EdgeViewModel : ViewModelBase
    {
        #region Fields
        private readonly IGraphAreaEditorService _graphAreaEditorService;
        #endregion

        #region Constructors
        public EdgeViewModel()
        {
        }

        public EdgeViewModel(DataEdge dataEdge, IGraphAreaEditorService graphAreaEditorService)
        {
            Argument.IsNotNull(() => dataEdge);
            Argument.IsNotNull(() => graphAreaEditorService);

            _graphAreaEditorService = graphAreaEditorService;
            DataEdge = dataEdge;

            DeleteEdgeCommand = new Command(OnDeleteEdgeCommandExecute, OnDeleteEdgeCommandCanExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the DeleteEdgeCommand command.
        /// </summary>
        public Command DeleteEdgeCommand { get; private set; }

        public GraphAreaViewModel AreaViewModel
        {
            get { return ParentViewModel as GraphAreaViewModel; }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [Expose("IsVisible")]
        [Expose("IsHighlightEnabled")]
        [Expose("IsHighlighted")]
        [Expose("IsEnabled")]
        public DataEdge DataEdge { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataEdge")]
        public bool IsInEditing { get; set; }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();
            SyncWithAreaProperties();
        }

        private void SyncWithAreaProperties()
        {
            var areaViewModel = AreaViewModel;
            if (areaViewModel == null)
            {
                return;
            }
            IsInEditing = areaViewModel.IsInEditing;
        }

        /// <summary>
        /// Method to check whether the DeleteEdgeCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnDeleteEdgeCommandCanExecute()
        {
            return IsInEditing;
        }

        /// <summary>
        /// Method to invoke when the DeleteEdgeCommand command is executed.
        /// </summary>
        private void OnDeleteEdgeCommandExecute()
        {
            var areaViewModel = AreaViewModel;
            if (areaViewModel != null)
            {
                _graphAreaEditorService.RemoveEdge(areaViewModel.Area, DataEdge);
            }
        }
        #endregion
    }
}