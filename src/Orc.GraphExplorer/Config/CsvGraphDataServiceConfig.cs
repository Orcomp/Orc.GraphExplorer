namespace Orc.GraphExplorer.Config
{
    using System.Configuration;

    public class CsvGraphDataServiceConfig : ConfigurationElement
    {
        public static CsvGraphDataServiceConfig Current
        {
            get
            {
                return GraphExplorerSection.Current.CsvGraphDataServiceConfig;
            }
        }

        [ConfigurationProperty("vertexesFilePath", IsRequired = true)]
        public string VertexesFilePath
        {
            get { return (string)base["vertexesFilePath"]; }
            set { base["vertexesFilePath"] = value; }
        }

        [ConfigurationProperty("edgesFilePath", IsRequired = true)]
        public string EdgesFilePath
        {
            get { return (string)base["edgesFilePath"]; }
            set { base["edgesFilePath"] = value; }
        }

        [ConfigurationProperty("enableCache",DefaultValue = true)]
        public bool EnableCache
        {
            get { return (bool)base["enableCache"]; }
            set { base["enableCache"] = value; }
        }

        [ConfigurationProperty("enableProperty", DefaultValue = false)]
        public bool EnableProperty
        {
            get { return (bool)base["enableProperty"]; }
            set { base["enableProperty"] = value; }
        }
    }
}
