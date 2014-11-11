namespace Orc.GraphExplorer.Models.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class FilterableEntity
    {
        public FilterableEntity(DataVertex vertex)
        {
            ID = vertex.ID;
            Title = vertex.Title;
            FirstName = vertex.Properties.Any(p => p.Key == "FirstName") ? vertex.Properties.First(p => p.Key == "FirstName").Value : string.Empty;
            LastName = vertex.Properties.Any(p => p.Key == "LastName") ? vertex.Properties.First(p => p.Key == "LastName").Value : string.Empty;
            Age = vertex.Properties.Any(p => p.Key == "Age" && IsInt(p.Value)) ? int.Parse((vertex.Properties.First(p => p.Key == "Age").Value)) : 0;
            Vertex = vertex;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public DataVertex Vertex { get; set; }

        public static IEnumerable<FilterableEntity> GenerateFilterableEntities(IEnumerable<DataVertex> vertices)
        {
            var enumerable = vertices.Select(v => new FilterableEntity(v));

            return enumerable;
        }

        private static bool IsInt(string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(c => "0123456789".Contains(c));
        }
    }
}
