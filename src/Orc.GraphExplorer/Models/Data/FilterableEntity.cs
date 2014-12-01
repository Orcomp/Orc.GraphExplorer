namespace Orc.GraphExplorer.Models.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Catel;

    using Models;

    public class FilterableEntity
    {
        public FilterableEntity(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            ID = vertex.ID;
            Title = vertex.Title;
            FirstName = vertex.Properties.Any(p => string.Equals(p.Key,"FirstName")) ? vertex.Properties.First(p => string.Equals(p.Key, "FirstName")).Value : string.Empty;
            LastName = vertex.Properties.Any(p => string.Equals(p.Key, "LastName")) ? vertex.Properties.First(p => string.Equals(p.Key, "LastName")).Value : string.Empty;
            Age = vertex.Properties.Any(p => string.Equals(p.Key, "Age") && p.Value.IsInteger()) ? int.Parse((vertex.Properties.First(p => string.Equals(p.Key, "Age")).Value)) : 0;
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
            Argument.IsNotNull(() => vertices);

            var enumerable = vertices.Select(v => new FilterableEntity(v));

            return enumerable;
        }
    }
}
