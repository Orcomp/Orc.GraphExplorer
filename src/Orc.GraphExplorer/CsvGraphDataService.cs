using CsvHelper;
using Orc.GraphExplorer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    // Summary:
    //     Represents a service loading data 
    //     from csv file which can be used to generate graph
    public class CsvGraphDataService : IGraphDataService
    {
        // Summary:
        //     Represents data struct in properties
        //     csv file 
        public class PropertyData
        {
            public int ID { get; set; }
            public string Property { get; set; }
            public string Value { get; set; }
        }

        CsvGraphDataServiceConfig _config;
        List<DataVertex> vlist;
        Dictionary<int, DataVertex> vCache;

        public CsvGraphDataServiceConfig Config
        {
            get { return _config; }
        }

        public CsvGraphDataService()
        {
            _config = CsvGraphDataServiceConfig.Current;
        }

        public CsvGraphDataService(CsvGraphDataServiceConfig config)
        {
            _config = config;
        }

        // Summary:
        //     get vertexes data by mapping and grouping from csv file 
        //
        // Parameters:
        //   onSuccess:
        //     callback function which will be invoked when 
        //     data successfully loaded and mapped
        //   onFail:
        //     callback function which will be invoked when 
        //     error occured during the processe
        public void GetVertexes(Action<IEnumerable<DataVertex>> onSuccess, Action<Exception> onFail)
        {
            try
            {
                if (vCache == null)
                    InnerGetVertxes();

                if (onSuccess != null)
                    onSuccess.Invoke(vCache.OrderBy(i=>i.Key).Select(i => i.Value));
            }
            catch (Exception error)
            {
                if (onFail != null)
                    onFail.Invoke(error);
            }
        }

        // Summary:
        //     get vertexes data by mapping and grouping from csv file and generate cache
        private void InnerGetVertxes()
        {
            var vertexesPath = ExamVertexesFilePath();

            using (var fs = new FileStream(vertexesPath, FileMode.Open))
            using (var reader = new CsvReader(new StreamReader(fs)))
            {
                var records = reader.GetRecords<PropertyData>().ToList();

                vlist = PopulateVertexes(records);
            }

            vCache = MergeVertexesCache(vlist, new Dictionary<int, DataVertex>());
        }

        private string ExamVertexesFilePath()
        {
            var vertexesPath = _config.VertexesFilePath;

            if (!File.Exists(vertexesPath))
                throw new Exception("Vertexes data file not found");
            return vertexesPath;
        }

        // Summary:
        //   keep & maintain a dictionary for caching DataVertex in case edges creation
        private static Dictionary<int, DataVertex> MergeVertexesCache(IEnumerable<DataVertex> vlist, Dictionary<int, DataVertex> cache)
        {
            foreach (var item in vlist)
            {
                if (cache.ContainsKey(item.Id))
                {
                    cache[item.Id] = item;
                }
                else
                {
                    cache.Add(item.Id, item);
                }
            }

            return cache;
        }

        // Summary:
        //   populate vertexes data from properties data loaded from csv file
        public static List<DataVertex> PopulateVertexes(List<PropertyData> records)
        {
            var query = from record in records
                        group record by record.ID into g
                        select new { g.Key, Properties = g.ToDictionary(d => d.Property, d => d.Value) };
            List<DataVertex> vlist = new List<DataVertex>();

            foreach (var result in query)
            {
                var vertex = new DataVertex(result.Key);

                vertex.SetProperties(result.Properties);
                //vertex.Properties = GenerateProperties(result.Properties, vertex);
                vlist.Add(vertex);
            }

            return vlist;
        }

        // Summary:
        //     get edges data by mapping and grouping from csv file 
        //
        // Parameters:
        //   onSuccess:
        //     callback function which will be invoked when data successfully loaded and mapped
        //   onFail:
        //     callback function which will be invoked when error occured during the processe
        public void GetEdges(Action<IEnumerable<DataEdge>> onSuccess, Action<Exception> onFail)
        {
            try
            {
                if (vlist == null)
                    InnerGetVertxes();

                var edgesFilePath = ExamEdgeFilePath();

                using (var fs = new FileStream(edgesFilePath, FileMode.Open))
                using (var reader = new CsvReader(new StreamReader(fs)))
                {
                    List<DataEdge> list = new List<DataEdge>();

                    while (reader.Read())
                    {
                        var fromId = reader.GetField<int>(0);
                        var toId = reader.GetField<int>(1);

                        DataVertex from;
                        DataVertex to;

                        UpdateEdgeRef(fromId, toId, out from, out to);

                        list.Add(new DataEdge(from, to));
                        //}
                    }

                    if (onSuccess != null)
                        onSuccess.Invoke(list);
                }
            }
            catch (Exception error)
            {
                if (onFail != null)
                    onFail.Invoke(error);
            }
        }

        private void UpdateEdgeRef(int fromId, int toId, out DataVertex from, out DataVertex to)
        {
            from = null;
            to = null;

            if (!vCache.TryGetValue(fromId, out from))
            {
                from = new DataVertex(fromId);
                vCache.Add(fromId, from);
            }

            if (!vCache.TryGetValue(toId, out to))
            {
                to = new DataVertex(toId);
                vCache.Add(toId, to);
            }
        }

        private string ExamEdgeFilePath()
        {

            var edgesFilePath = _config.EdgesFilePath;

            if (!File.Exists(edgesFilePath))
                throw new Exception("Edges data file not found");
            return edgesFilePath;
        }

        // Summary:
        //     clear vertex cache
        public void Clear()
        {
            vCache.Clear();
        }

        // Summary:
        //     update source
        //
        // Parameters:
        //   vertexes:
        //     vertexes to be updated to source file
        //   onComplete:
        //     callback function which will be invoked when update is complete
        public void UpdateVertexes(IEnumerable<DataVertex> vertexes, Action<bool, Exception> onComplete)
        {
            try
            {
                var path = ExamVertexesFilePath();

                using (var fs = new FileStream(path, FileMode.Truncate))
                using (var writer = new CsvWriter(new StreamWriter(fs)))
                {
                    writer.WriteField("ID");
                    writer.WriteField("Property");
                    writer.WriteField("Value");
                    writer.NextRecord();

                    foreach (var v in vertexes)
                    {
                        var id = v.Id;

                        if (v.Properties != null && v.Properties.Count>0)
                        {
                            foreach (var p in v.Properties)
                            {
                                writer.WriteField(id.ToString());
                                writer.WriteField(p.Key);
                                writer.WriteField(p.Value);
                                writer.NextRecord();
                            }
                        }
                        else
                        {
                            writer.WriteField(id.ToString());
                            writer.NextRecord();
                        }
                    }
                }

                foreach (var v in vertexes)
                {
                    v.Commit();
                }

                vCache = MergeVertexesCache(vertexes, new Dictionary<int, DataVertex>());

                if (onComplete != null)
                {
                    onComplete.Invoke(true, null);
                }
            }
            catch (Exception ex)
            {
                if (onComplete != null)
                {
                    onComplete.Invoke(false, new Exception("error occured during update vertexes", ex));
                }
            }

        }

        // Summary:
        //     update edges to csv file
        //
        // Parameters:
        //   edges:
        //     edges to be updated to source file
        //   onComplete:
        //     callback function which will be invoked when update is complete
        public void UpdateEdges(IEnumerable<DataEdge> edges, Action<bool, Exception> onComplete)
        {
            try
            {
                var path = ExamEdgeFilePath();

                using (var fs = new FileStream(path, FileMode.Truncate))
                using (var writer = new CsvWriter(new StreamWriter(fs)))
                {
                    writer.WriteField("From");
                    writer.WriteField("To");
                    writer.NextRecord();

                    foreach (var v in edges)
                    {
                        writer.WriteField(v.Source.Id);
                        writer.WriteField(v.Target.Id);
                        writer.NextRecord();
                    }
                }

                if (onComplete != null)
                {
                    onComplete.Invoke(true, null);
                }
            }
            catch (Exception ex)
            {
                if (onComplete != null)
                {
                    onComplete.Invoke(false, new Exception("error occured during update edges", ex));
                }
            }
        }

        public void UpdateVertex(DataVertex vertex, Action<bool, DataVertex, Exception> onComplete)
        {
            if (vertex == null)
                return;

            try
            {
                if (vCache.ContainsKey(vertex.Id))
                {
                    vCache[vertex.Id] = vertex;
                }
                else
                {
                    vCache.Add(vertex.Id, vertex);
                }

                var list = vCache.Values.ToList();

                UpdateVertexes(list, (r, e) =>
                {
                    if (onComplete != null)
                    {
                        onComplete.Invoke(r, vertex, e);
                    }
                });
            }
            catch (Exception ex)
            {
                if (onComplete != null)
                {
                    onComplete.Invoke(true, vertex, new Exception(string.Format("error occuered during updating vertex to csv. vertex id [{0}]", vertex.ID), ex));
                }
                else
                    throw new Exception(string.Format("error occuered during updating vertex to csv. vertex id [{0}]", vertex.ID), ex);
            }
        }


        public void UpdateVertex(DataVertex vertex, Action<bool, Exception> onComplete)
        {
            throw new NotImplementedException();
        }

        public void UpdateEdges(IEnumerable<DataEdge> vertexes, Action<bool, DataVertex, Exception> onComplete)
        {
            throw new NotImplementedException();
        }
    }
}
