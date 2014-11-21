#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    using Catel;
    using Catel.Collections;
    using Catel.Data;

    using Orc.GraphExplorer.Models.Data;

    public class Filter : ModelBase
    {
        #region Fields
        private readonly GraphLogic _logic;
        #endregion

        #region Constructors
        public Filter(GraphLogic logic)
        {
            Argument.IsNotNull(() => logic);

            _logic = logic;

            var graph = _logic.Graph;
            graph.VertexAdded += OnVertexAdded;
            graph.VertexRemoved += OnVertexRemoved;

            _logic.GraphReloaded += _logic_GraphReloaded;

            FilterableEntities = new ObservableCollection<FilterableEntity>();
            FilteredEntities = new ObservableCollection<FilterableEntity>();

            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsFilterEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsHideVertexes { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsFilterApplied { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilterableEntities { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilteredEntities { get; set; }
        #endregion

        #region Methods
        private void _logic_GraphReloaded(object sender, GraphEventArgs e)
        {
            var filterableEntities = FilterableEntities;

            filterableEntities.Clear();
            filterableEntities.AddRange(FilterableEntity.GenerateFilterableEntities(e.Graph.Vertices));
        }

        public void FilterEntities()
        {
            if (!IsFilterEnabled)
            {
                return;
            }

            foreach (var filterable in FilterableEntities.Except(FilteredEntities))
            {
                ApplyFilterForEntity(filterable, false);
            }

            foreach (var filteredEntity in FilteredEntities)
            {
                ApplyFilterForEntity(filteredEntity, true);
            }
        }

        private void ApplyFilterForEntity(FilterableEntity entity, bool filtered)
        {
            Argument.IsNotNull(() => entity);

            var vertex = entity.Vertex;

            vertex.IsFiltered = filtered;
            vertex.IsVisible = !IsHideVertexes || filtered;
            FilterEdges();
        }

        private void FilterEdges()
        {
            if (!IsFilterEnabled)
            {
                return;
            }

            foreach (var dataEdge in _logic.Graph.Edges)
            {
                dataEdge.IsVisible = !IsHideVertexes || dataEdge.IsFiltered;
            }
        }

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
                                ApplyFilterForEntity(item, true);
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

        private void OnVertexAdded(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            FilterableEntities.Add(new FilterableEntity(vertex));
        }

        private void OnVertexRemoved(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            var filterableEntities = FilterableEntities;

            var filterableEntity = filterableEntities.FirstOrDefault(x => x.ID == vertex.ID);
            filterableEntities.Remove(filterableEntity);
        }

        /// <summary>
        /// Called when the IsHideVertexes property has changed.
        /// </summary>
        private void OnIsHideVertexesChanged()
        {
            if (IsFilterEnabled && IsFilterApplied)
            {
                FilterEntities();
                FilterEdges();
            }
        }

        private void OnIsFilterAppliedChanged()
        {
            if (IsFilterApplied)
            {
                FilterEntities();
                FilterEdges();
            }
            else
            {
                foreach (var filterableEntity in FilterableEntities)
                {
                    filterableEntity.Vertex.IsFiltered = true;
                    filterableEntity.Vertex.IsVisible = true;
                }

                foreach (var dataEdge in _logic.Graph.Edges)
                {
                    dataEdge.IsVisible = true;
                }
            }
        }

        public void UpdateFilterSource()
        {
            var filterableEntities = FilterableEntities;

            filterableEntities.Clear();
            filterableEntities.AddRange(FilterableEntity.GenerateFilterableEntities(_logic.Graph.Vertices));
        }
        #endregion
    }
}