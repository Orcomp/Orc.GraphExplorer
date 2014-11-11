#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.Data;
    using Messages;
    using Orc.GraphExplorer.Models.Data;
    using QuickGraph;

    public class Filter : ModelBase
    {
        private readonly GraphLogic _logic;

        public Filter(GraphLogic logic)
        {
            _logic = logic;

            _logic.Graph.VertexAdded += OnVertexAdded;
            _logic.Graph.VertexRemoved += OnVertexRemoved;

            _logic.GraphReloaded += _logic_GraphReloaded;

            FilterableEntities = new ObservableCollection<FilterableEntity>();
            FilteredEntities = new ObservableCollection<FilterableEntity>();

            FilteredEntities.CollectionChanged += FilteredEntities_CollectionChanged;
        }

        void _logic_GraphReloaded(object sender, Events.GraphEventArgs e)
        {
            FilterableEntities.Clear();
            FilterableEntities.AddRange(FilterableEntity.GenerateFilterableEntities(e.Graph.Vertices));
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
            entity.Vertex.IsFiltered = filtered;
            entity.Vertex.IsVisible = !IsHideVertexes || filtered;
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
                dataEdge.IsVisible = dataEdge.IsFiltered;
            }
        }

        void FilteredEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (IsFilterEnabled)
                    {
                        if (IsFilterApplied && FilteredEntities.Count == FilterableEntities.Count)
                        {
                            IsFilterApplied = false;
                        }
                        else if (!IsFilterApplied && FilteredEntities.Count != FilterableEntities.Count)
                        {
                            IsFilterApplied = true;
                        }

                        if (IsFilterApplied && IsFilterEnabled)
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
        
        void OnVertexAdded(DataVertex vertex)
        {
            FilterableEntities.Add(new FilterableEntity(vertex));
        }

        void OnVertexRemoved(DataVertex vertex)
        {
            var filterableEntity = FilterableEntities.FirstOrDefault(x => x.ID == vertex.ID);
            FilterableEntities.Remove(filterableEntity);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsFilterEnabled
        {
            get { return GetValue<bool>(IsFilterEnabledProperty); }
            set { SetValue(IsFilterEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsFilterEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsFilterEnabledProperty = RegisterProperty("IsFilterEnabled", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHideVertexes
        {
            get { return GetValue<bool>(IsHideVertexesProperty); }
            set { SetValue(IsHideVertexesProperty, value); }
        }

        /// <summary>
        /// Register the IsHideVertexes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHideVertexesProperty = RegisterProperty("IsHideVertexes", typeof(bool), true, (sender, e) => ((Filter)sender).OnIsHideVertexesChanged());

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

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsFilterApplied
        {
            get { return GetValue<bool>(IsFilterAppliedProperty); }
            set { SetValue(IsFilterAppliedProperty, value); }
        }

        /// <summary>
        /// Register the IsFilterApplied property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsFilterAppliedProperty = RegisterProperty("IsFilterApplied", typeof(bool), null, (sender, args) => ((Filter)sender).OnIsFilterAppliedChanged());

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

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilterableEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilterableEntitiesProperty); }
            set { SetValue(FilterableEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilterableEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterableEntitiesProperty = RegisterProperty("FilterableEntities", typeof(ObservableCollection<FilterableEntity>), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ObservableCollection<FilterableEntity> FilteredEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilteredEntitiesProperty); }
            set { SetValue(FilteredEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilteredEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredEntitiesProperty = RegisterProperty("FilteredEntities", typeof(ObservableCollection<FilterableEntity>), null);

        public void UpdateFilterSource()
        {
            FilterableEntities.Clear();
            FilterableEntities.AddRange(FilterableEntity.GenerateFilterableEntities(_logic.Graph.Vertices));
        }
    }
}