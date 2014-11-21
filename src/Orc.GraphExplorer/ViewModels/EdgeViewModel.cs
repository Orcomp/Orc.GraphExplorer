#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel;
    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class EdgeViewModel : ViewModelBase
    {
        public EdgeViewModel()
        {
            
        }

        public EdgeViewModel(DataEdge dataEdge)
        {
            DataEdge = dataEdge;

            DeleteEdgeCommand = new Command(OnDeleteEdgeCommandExecute, OnDeleteEdgeCommandCanExecute);
        }

        protected override void Initialize()
        {
            base.Initialize();
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
        /// Gets the DeleteEdgeCommand command.
        /// </summary>
        public Command DeleteEdgeCommand { get; private set; }

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
                areaViewModel.RemoveEdge(DataEdge);
            }
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
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof (bool), () => false);

        public GraphAreaViewModel AreaViewModel
        {
            get
            {
                return ParentViewModel as GraphAreaViewModel;
                
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataEdge DataEdge
        {
            get { return GetValue<DataEdge>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the DataEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("DataEdge", typeof (DataEdge));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlightEnabled
        {
            get { return GetValue<bool>(IsHighlightEnabledProperty); }
            set { SetValue(IsHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlightEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightEnabledProperty = RegisterProperty("IsHighlightEnabled", typeof (bool), () => true);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlighted
        {
            get { return GetValue<bool>(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlighted property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightedProperty = RegisterProperty("IsHighlighted", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataEdge")]
        public bool IsVisible
        {
            get { return GetValue<bool>(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsVisibleProperty = RegisterProperty("IsVisible", typeof (bool));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsEnabled
        {
            get { return GetValue<bool>(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsEnabledProperty = RegisterProperty("IsEnabled", typeof (bool), () => true);
    }
}