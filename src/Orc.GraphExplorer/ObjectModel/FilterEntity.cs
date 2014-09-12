namespace Orc.GraphExplorer.ObjectModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class FilterEntity
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        //public string PropertyName { get; set; }

        //public string PropertyValue { get; set; }

        public DataVertex Vertex { get; set; }

        public static IEnumerable<FilterEntity> GenerateFilterEntities(IEnumerable<DataVertex> vertexes)
        {
            //List<FilterEntity> list = new List<FilterEntity>();
            var enumerable = vertexes.Select(v =>
                new FilterEntity
                {
                    ID = v.ID,
                    Title = v.Title,
                    FirstName = v.Properties.Any(p => p.Key == "FirstName") ? v.Properties.First(p => p.Key == "FirstName").Value : string.Empty,
                    LastName = v.Properties.Any(p => p.Key == "LastName") ? v.Properties.First(p => p.Key == "LastName").Value : string.Empty,
                    Age = v.Properties.Any(p => p.Key == "Age" && IsInt(p.Value)) ? int.Parse((v.Properties.First(p => p.Key == "Age").Value)) : 0,
                    Vertex = v
                });
            //list.AddRange();

            //foreach (var v in vertexes)
            //{
            //    if (!list.Any(i => i.ID == v.ID))
            //        list.Add(new FilterEntity() { ID = v.Id,Title = v.Title,Vertex = v });
            //}
            return enumerable;
        }

        public static bool IsInt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            foreach (var c in str)
            {
                if (!"0123456789".Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
