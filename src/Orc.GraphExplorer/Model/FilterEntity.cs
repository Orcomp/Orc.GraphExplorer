using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Model
{
    public class FilterEntity
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public DataVertex Vertex { get; set; }

        public static IEnumerable<FilterEntity> GenerateFilterEntities(IEnumerable<DataVertex> vertexes)
        {
            List<FilterEntity> list = new List<FilterEntity>();

            list.AddRange(vertexes.SelectMany(v => v.Properties).Select(p => new FilterEntity { ID = p.Data.Id, Title = p.Data.Title, PropertyName = p.Key, PropertyValue = p.Value, Vertex = p.Data }));

            foreach (var v in vertexes)
            {
                if (!list.Any(i => i.ID == v.ID))
                    list.Add(new FilterEntity() { ID = v.Id,Title = v.Title,Vertex = v });
            }
            return list;
        }
    }
}
