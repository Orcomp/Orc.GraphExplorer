#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using Catel;
    using Catel.MVVM;
    using Models;
    using Models.Data;

    public class FilterViewModel : ViewModelBase
    {
        #region Constructors
        public FilterViewModel(Filter filter)
        {
            Argument.IsNotNull(() => filter);

            Filter = filter;

            var graphLogic = filter.GraphLogic;
            var graph = graphLogic.Graph;
            graph.VertexAdded += OnVertexAdded;
            graph.VertexRemoved += OnVertexRemoved;

            graphLogic.GraphReloaded += _logic_GraphReloaded;

            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;

            ClearFilterCommand = new Command(OnClearFilterCommandExecute);
        }
        #endregion

        /// <summary>
        /// Gets the ClearFilterCommand command.
        /// </summary>
        public Command ClearFilterCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the ClearFilterCommand command is executed.
        /// </summary>
        private void OnClearFilterCommandExecute()
        {
            IsFilterApplied = false;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Filter Filter { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public bool IsFilterApplied { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public bool IsHideVertexes { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public bool IsFilterEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public ObservableCollection<FilterableEntity> FilterableEntities { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public ObservableCollection<FilterableEntity> FilteredEntities { get; set; }
        #endregion

        #region Methods
        private void FilteredEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (IsFilterEnabled)
                    {
                        var filteredEntities = FilteredEntities;
                        var filterableEntities = FilterableEntities;

                        if (IsFilterApplied && filteredEntities.Count == filterableEntities.Count)
                        {
                            IsFilterApplied = false;
                        }
                        else if (!IsFilterApplied && filteredEntities.Count != filterableEntities.Count)
                        {
                            IsFilterApplied = true;
                        }

                        if (IsFilterApplied)
                        {
                            foreach (var item in e.NewItems.OfType<FilterableEntity>())
                            {
                                Filter.ApplyFilterForEntity(item, true);
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (IsFilterEnabled)
                    {
                        IsFilterApplied = false;
                    }
                    break;
            }
        }

        private void _logic_GraphReloaded(object sender, GraphEventArgs e)
        {
            Filter.ChangeFilterSource(e.Graph.Vertices);
        }

        private void OnVertexAdded(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            Filter.AddVertexToSource(vertex);
        }

        private void OnVertexRemoved(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            Filter.RemoveVertexFromSource(vertex);
        }

        /// <summary>
        /// Called when the IsHideVertexes property has changed.
        /// </summary>
        private void OnIsHideVertexesChanged()
        {
            if (IsFilterEnabled && IsFilterApplied)
            {
                Filter.ApplyFilter();
            }
        }

        private void OnIsFilterAppliedChanged()
        {
            if (IsFilterApplied)
            {
                Filter.ApplyFilter();
            }
            else
            {
                Filter.ClearFilter();
            }
        }
        #endregion
    }
}