#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvGraphDataService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Csv.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Configuration;

    using CsvHelper;
    using Data;
    using Factories;
    using GraphExplorer.Services;
    using Models;
    using Models.Data;

    public class CsvGraphDataService : IGraphDataService
    {
        #region Fields
        private readonly IDataVertexFactory _dataVertexFactory;
        private readonly IDataLocationSettingsService _dataLocationSettingsService;

        private readonly IConfigurationService _configurationService;

        private List<DataVertex> _verticesWithProperties;

        private IList<DataVertex> _verticesWithoutProperties;

        private List<DataEdge> _edges;
        #endregion

        #region Constructors
        public CsvGraphDataService(IDataVertexFactory dataVertexFactory, IDataLocationSettingsService dataLocationSettingsService, IConfigurationService configurationService)
        {
            _dataVertexFactory = dataVertexFactory;
            _dataLocationSettingsService = dataLocationSettingsService;
            _configurationService = configurationService;

            _configurationService.ConfigurationChanged += _configurationService_ConfigurationChanged;
        }

        void _configurationService_ConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            _verticesWithProperties = null;
            _verticesWithoutProperties = null;
            _edges = null;
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
            Argument.IsNotNull(() => graph);

            var dataLocationSettings = _dataLocationSettingsService.GetCurrentOrLoad();

            using (var fs = new FileStream(dataLocationSettings.RelationshipsFile, FileMode.Truncate))
            {
                using (var writer = new CsvWriter(new StreamWriter(fs)))
                {
                    writer.WriteHeader<RelationDataRecord>();
                    foreach (var edge in graph.Edges)
                    {
                        writer.WriteRecord(new RelationDataRecord {From = edge.Source.ID, To = edge.Target.ID});
                    }
                }
            }

            if (dataLocationSettings.EnableProperty ?? false)
            {
                using (var fs = new FileStream(dataLocationSettings.PropertiesFile, FileMode.Truncate))
                {
                    using (var writer = new CsvWriter(new StreamWriter(fs)))
                    {
                        writer.WriteHeader<PropertyDataRecord>();
                        foreach (var propertyData in graph.Vertices.SelectMany(x => x.Properties.Select(prop => new PropertyDataRecord {ID = x.ID, Property = prop.Key, Value = prop.Value})))
                        {
                            writer.WriteRecord(propertyData);
                        }
                    }
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
        private IEnumerable<DataVertex> InitializeVertexProperties()
        {
            return LoadProperties().GroupBy(x => x.ID).Select(x =>
            {
                var vertex = _dataVertexFactory.CreateVertex(x.Key);
                foreach (var record in x)
                {
                    vertex.Properties.Add(new Property() {Key = record.Property, Value = record.Value});
                }

                return vertex;
            });
        }

        private IEnumerable<DataEdge> InitializeEdges()
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
                vertex = _dataVertexFactory.CreateVertex(id);
                VertecesWithoutProperties.Add(vertex);
            }

            return vertex;
        }

        private IEnumerable<PropertyDataRecord> LoadProperties()
        {
            var dataLocationSettings = _dataLocationSettingsService.GetCurrentOrLoad();
            if (!dataLocationSettings.EnableProperty ?? false)
            {
                yield break;
            }

            using (var fs = new FileStream(dataLocationSettings.PropertiesFile, FileMode.Open))
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
            var dataLocationSettings = _dataLocationSettingsService.GetCurrentOrLoad();
            using (var fs = new FileStream(dataLocationSettings.RelationshipsFile, FileMode.Open))
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