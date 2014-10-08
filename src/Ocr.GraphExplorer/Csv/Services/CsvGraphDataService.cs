#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvGraphDataService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Csv.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using CsvHelper;

    using Orc.GraphExplorer.Config;
    using Orc.GraphExplorer.Csv.Data;
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;
    using Orc.GraphExplorer.Services.Interfaces;

    public class CsvGraphDataService : IGraphDataService
    {
        #region Fields
        private readonly CsvGraphDataServiceConfig _config;

        private List<DataVertex> _verticesWithProperties;

        private IList<DataVertex> _verticesWithoutProperties;

        private List<DataEdge> _edges;

        private IList<RelationDataRecord> _loadedRelations;
        #endregion

        #region Constructors
        public CsvGraphDataService()
        {
            _config = CsvGraphDataServiceConfig.Current;
        }
        #endregion

        #region Properties
        private IEnumerable<DataVertex> VertecesWithProperties
        {
            get
            {
                if (_verticesWithProperties == null)
                {
                    _verticesWithProperties = new List<DataVertex>();
                    _verticesWithProperties.AddRange(InitializeVertexProperties());
                }

                return _verticesWithProperties;
            }
        }

        private IList<DataVertex> VertecesWithoutProperties
        {
            get
            {
                if (_verticesWithoutProperties == null)
                {
                    _verticesWithoutProperties = new List<DataVertex>();
                }

                return _verticesWithoutProperties;
            }
        }

        private IEnumerable<DataEdge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new List<DataEdge>();
                    _edges.AddRange(InitializeEdges());
                }

                return _edges;
            }
        }
        #endregion

        #region IGraphDataService Members
        public IEnumerable<DataVertex> GetVerteces()
        {
            GetEdges();
            return VertecesWithProperties.Concat(VertecesWithoutProperties);
        }

        public IEnumerable<DataEdge> GetEdges()
        {
            return Edges;
        }

        public void SaveChanges(Graph graph)
        {
            using (var fs = new FileStream(_config.EdgesFilePath, FileMode.Truncate))
            using (var writer = new CsvWriter(new StreamWriter(fs)))
            {
                writer.WriteHeader<RelationDataRecord>();
                foreach (var edge in graph.Edges)
                {
                    writer.WriteRecord(new RelationDataRecord { From = edge.Source.ID, To = edge.Target.ID });
                }
            }
            
            using (var fs = new FileStream(_config.VertexesFilePath, FileMode.Truncate))
            using (var writer = new CsvWriter(new StreamWriter(fs)))
            {
                writer.WriteHeader<PropertyDataRecord>();
                foreach (var propertyData in graph.Vertices.SelectMany(x => x.Properties.Select(prop => new PropertyDataRecord{ ID = x.ID, Property = prop.Key, Value = prop.Value})))
                {
                    writer.WriteRecord(propertyData);
                }
            }

            _edges.Clear();
            _edges = null;

            _verticesWithProperties.Clear();
            _verticesWithProperties = null;

            _verticesWithoutProperties.Clear();
            _verticesWithoutProperties = null;
        }
        #endregion

        #region Methods
        public IEnumerable<DataVertex> InitializeVertexProperties()
        {
            return LoadProperties().GroupBy(x => x.ID).Select(x =>
            {
                var vertex = DataVertex.Create(x.Key);
                foreach (var record in x)
                {
                    vertex.Properties.Add(new Property() { Key = record.Property, Value = record.Value });
                }

                return vertex;
            });
        }

        public IEnumerable<DataEdge> InitializeEdges()
        {
            return from relation in LoadRelations()
                let @from = GetOrCreateVertex(relation.From)
                let to = GetOrCreateVertex(relation.To)
                select new DataEdge(@from, to);           
        }

        private DataVertex GetOrCreateVertex(int id)
        {
            var vertex = VertecesWithProperties.FirstOrDefault(x => x.ID == id) ?? VertecesWithoutProperties.FirstOrDefault(x => x.ID == id);

            if (vertex == null)
            {
                vertex = DataVertex.Create(id);
                VertecesWithoutProperties.Add(vertex);
            }

            return vertex;
        }

        private IEnumerable<PropertyDataRecord> LoadProperties()
        {
            using (var fs = new FileStream(_config.VertexesFilePath, FileMode.Open))
            {
                using (var reader = new CsvReader(new StreamReader(fs)))
                {
                    while (reader.Read())
                    {
                        yield return reader.GetRecord<PropertyDataRecord>();
                    }
                }
            }
        }

        private IEnumerable<RelationDataRecord> LoadRelations()
        {
            using (var fs = new FileStream(_config.EdgesFilePath, FileMode.Open))
            {
                using (var reader = new CsvReader(new StreamReader(fs)))
                {
                    while (reader.Read())
                    {
                        yield return reader.GetRecord<RelationDataRecord>();
                    }
                }
            }
        }
        #endregion
    }
}