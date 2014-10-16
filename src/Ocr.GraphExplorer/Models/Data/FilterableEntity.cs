namespace Orc.GraphExplorer.Models.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class FilterableEntity
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public DataVertex Vertex { get; set; }

        public static IEnumerable<FilterableEntity> GenerateFilterableEntities(IEnumerable<DataVertex> vertices)
        {
            var enumerable = vertices.Select(v =>
                new FilterableEntity
                {
                    ID = v.ID,
                    Title = v.Title,
                    FirstName = v.Properties.Any(p => p.Key == "FirstName") ? v.Properties.First(p => p.Key == "FirstName").Value : string.Empty,
                    LastName = v.Properties.Any(p => p.Key == "LastName") ? v.Properties.First(p => p.Key == "LastName").Value : string.Empty,
                    Age = v.Properties.Any(p => p.Key == "Age" && IsInt(p.Value)) ? int.Parse((v.Properties.First(p => p.Key == "Age").Value)) : 0,
                    Vertex = v
                });

            return enumerable;
        }

        private static bool IsInt(string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(c => "0123456789".Contains(c));
        }
    }
}
