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
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Services.Interfaces;

    public class CsvGraphDataService : IGraphDataService
    {
        #region Fields
        private readonly CsvGraphDataServiceConfig _config;

        private ReplaySubject<DataVertex> _vertices;

        private ReplaySubject<DataEdge> _edges;
        #endregion

        #region Constructors
        public CsvGraphDataService()
        {
            _config = CsvGraphDataServiceConfig.Current;
        }
        #endregion

        #region IGraphDataService Members
        public IObservable<DataVertex> GetVerteces()
        {
            if (_vertices == null)
            {
                _vertices = new ReplaySubject<DataVertex>();
                InitializeVerteces().Subscribe(_vertices);
            }

            return _vertices;
        }

        public IObservable<DataEdge> GetEdges()
        {
            if (_edges == null)
            {
                _edges = new ReplaySubject<DataEdge>();
                InitializeEdges().Subscribe(_edges);
            }

            return _edges;
        }
        #endregion

        #region Methods
        public IObservable<DataVertex> InitializeVerteces()
        {
            return LoadVerteces().GroupBy(x => x.ID).Select(x =>
            {
                // TODO: use properties later to set them in the vertex
                var propertiers = x.Select(r => new { r.Property, r.Value }).ToList();
                return new DataVertex(x.Key);
            });
        }

        public IObservable<DataEdge> InitializeEdges()
        {
            return LoadEdges().Select(e =>
            {
                var from = GetVerteces().FirstOrDefault(x => x.ID == e.From);
                var to = GetVerteces().FirstOrDefault(x => x.ID == e.To);
                return new DataEdge(from, to);
            });
        }

        private IObservable<VertexDataRecord> LoadVerteces()
        {
            return Observable.Create<VertexDataRecord>(observer =>
            {
                bool disposed = false;
                using (var fs = new FileStream(_config.VertexesFilePath, FileMode.Open))
                {
                    using (var reader = new CsvReader(new StreamReader(fs)))
                    {
                        while (reader.Read() && !disposed)
                        {
                            observer.OnNext(reader.GetRecord<VertexDataRecord>());
                        }
                    }
                }

                observer.OnCompleted();
                return Disposable.Create(() => disposed = true);
            });
        }

        private IObservable<EdgeDataRecord> LoadEdges()
        {
            return Observable.Create<EdgeDataRecord>(observer =>
            {
                bool disposed = false;
                using (var fs = new FileStream(_config.EdgesFilePath, FileMode.Open))
                {
                    using (var reader = new CsvReader(new StreamReader(fs)))
                    {
                        while (reader.Read() && !disposed)
                        {
                            observer.OnNext(reader.GetRecord<EdgeDataRecord>());
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