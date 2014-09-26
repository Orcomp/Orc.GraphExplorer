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
    using System.IO;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using CsvHelper;

    using Orc.GraphExplorer.Config;
    using Orc.GraphExplorer.Csv.Data;
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Services.Interfaces;

    public class CsvGraphDataService : IGraphDataService
    {
        #region Fields
        private readonly CsvGraphDataServiceConfig _config;

        private ReplaySubject<DataVertex> _verticesWithProperties;

        private ReplaySubject<DataVertex> _verticesWithoutProperties;

        private ReplaySubject<DataEdge> _edges;

        private int _vertecesWitoutPropertiesCount;

        private ISubject<RelationDataRecord> _loadedRelations;
        #endregion

        #region Constructors
        public CsvGraphDataService()
        {
            _config = CsvGraphDataServiceConfig.Current;
        }
        #endregion

        #region Properties
        private IObservable<DataVertex> VertecesWithProperties
        {
            get
            {
                if (_verticesWithProperties == null)
                {
                    _verticesWithProperties = new ReplaySubject<DataVertex>();
                    InitializeVertexProperties().Subscribe(_verticesWithProperties);
                }

                return _verticesWithProperties;
            }
        }

        private ISubject<DataVertex> VertecesWithoutProperties
        {
            get
            {
                if (_verticesWithoutProperties == null)
                {
                    _verticesWithoutProperties = new ReplaySubject<DataVertex>();
                }

                return _verticesWithoutProperties;
            }
        }

        private IObservable<DataEdge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new ReplaySubject<DataEdge>();
                    InitializeEdges().Subscribe(_edges);
                }

                return _edges;
            }
        }
        #endregion

        #region IGraphDataService Members
        public IObservable<DataVertex> GetVerteces()
        {
            return VertecesWithProperties.Concat(VertecesWithoutProperties);
        }

        public IObservable<DataEdge> GetEdges()
        {
            return Edges;
        }
        #endregion

        #region Methods
        public IObservable<DataVertex> InitializeVertexProperties()
        {
            return LoadProperties().GroupBy(x => x.ID).Select(x =>
            {
                // TODO: use properties later to set them in the vertex
                var propertiers = x.Select(r => new { r.Property, r.Value }).ToList();
                return new DataVertex(x.Key);
            });
        }

        public IObservable<DataEdge> InitializeEdges()
        {
            return Observable.Create<DataEdge>(observer => LoadRelations().Subscribe(e =>
            {
                var from = GetOrCreateVertex(e.From);
                var to = GetOrCreateVertex(e.To);
                var edge = new DataEdge(@from, to);
                observer.OnNext(edge);
            }, observer.OnError, () =>
            {
                observer.OnCompleted();
                VertecesWithoutProperties.OnCompleted();
            }));
        }

        private DataVertex GetOrCreateVertex(int id)
        {
            var vertex = VertecesWithProperties.Concat(VertecesWithoutProperties.Take(_vertecesWitoutPropertiesCount)).FirstOrDefault(x => x.ID == id);
            if (vertex == null)
            {
                try
                {
                    vertex = new DataVertex(id);
                    _vertecesWitoutPropertiesCount++;
                    VertecesWithoutProperties.OnNext(vertex);
                }
                catch (Exception ex)
                {
                    VertecesWithoutProperties.OnError(ex);
                }
            }

            return vertex;
        }

        private IObservable<PropertyDataRecord> LoadProperties()
        {
            return Observable.Create<PropertyDataRecord>(observer =>
            {
                bool disposed = false;
                using (var fs = new FileStream(_config.VertexesFilePath, FileMode.Open))
                {
                    using (var reader = new CsvReader(new StreamReader(fs)))
                    {
                        while (reader.Read() && !disposed)
                        {
                            observer.OnNext(reader.GetRecord<PropertyDataRecord>());
                        }
                    }
                }

                observer.OnCompleted();
                return Disposable.Create(() => disposed = true);
            });
        }

        private IObservable<RelationDataRecord> LoadRelations()
        {
            return Observable.Create<RelationDataRecord>(observer =>
            {
                bool disposed = false;
                using (var fs = new FileStream(_config.EdgesFilePath, FileMode.Open))
                {
                    using (var reader = new CsvReader(new StreamReader(fs)))
                    {
                        while (reader.Read() && !disposed)
                        {
                            observer.OnNext(reader.GetRecord<RelationDataRecord>());
                        }
                    }
                }

                observer.OnCompleted();
                return Disposable.Create(() => disposed = true);
            });
        }
        #endregion
    }
}