#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer
{
    using System.Collections.Generic;
    using System.Linq;

    using Catel;
    using Catel.Collections;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public static class FilterExtensions
    {
        #region Methods
        public static void ApplyFilter(this Filter filter)
        {
            ApplyForEntities(filter);
            ApplyForEdges(filter);
        }

        private static void ApplyForEntities(this Filter filter)
        {
            if (!filter.IsFilterEnabled)
            {
                return;
            }

            foreach (var filterable in filter.FilterableEntities.Except(filter.FilteredEntities))
            {
                ApplyFilterForEntity(filter, filterable, false);
            }

            foreach (var filteredEntity in filter.FilteredEntities)
            {
                ApplyFilterForEntity(filter, filteredEntity, true);
            }
        }

        public static void ApplyFilterForEntity(this Filter filter, FilterableEntity entity, bool filtered)
        {
            Argument.IsNotNull(() => entity);

            var vertex = entity.Vertex;

            vertex.IsFiltered = filtered;
            vertex.IsVisible = !filter.IsHideVertexes || filtered;
            ApplyForEdges(filter);
        }

        private static void ApplyForEdges(this Filter filter)
        {
            if (!filter.IsFilterEnabled)
            {
                return;
            }

            foreach (var dataEdge in filter.GraphLogic.Graph.Edges)
            {
                dataEdge.IsVisible = !filter.IsHideVertexes || dataEdge.IsFiltered();
            }
        }

        public static void ClearFilter(this Filter filter)
        {
            foreach (var filterableEntity in filter.FilterableEntities)
            {
                filterableEntity.Vertex.IsFiltered = true;
                filterableEntity.Vertex.IsVisible = true;
            }

            foreach (var dataEdge in filter.GraphLogic.Graph.Edges)
            {
                dataEdge.IsVisible = true;
            }
        }

        public static void ChangeFilterSource(this Filter filter, IEnumerable<DataVertex> vertices)
        {
            Argument.IsNotNull(() => vertices);

            var filterableEntities = filter.FilterableEntities;

            filterableEntities.Clear();
            filterableEntities.AddRange(FilterableEntity.GenerateFilterableEntities(vertices));
        }

        public static void AddVertexToSource(this Filter filter, DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            filter.FilterableEntities.Add(new FilterableEntity(vertex));
        }

        public static void RemoveVertexFromSource(this Filter filter, DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            var filterableEntities = filter.FilterableEntities;

            var filterableEntity = filterableEntities.FirstOrDefault(x => x.ID == vertex.ID);
            filterableEntities.Remove(filterableEntity);
        }
        #endregion
    }
}