namespace Orc.GraphExplorer.Services
{
    using System.Configuration;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Services.Interfaces;

    class FilePickerService : IFilePickerService
    {
        public FilePicker Load()
        {
            var exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var section = (GraphExplorerSection)exeConfiguration.GetSection("graphExplorer");

            var config = section.CsvGraphDataServiceConfig;

            return new FilePicker
                   {
                       RelationshipsText = config.EdgesFilePath, 
                       PropertiesText = config.VertexesFilePath, 
                       EnableProperty = config.EnableProperty
                   };
        }

        public void Save(FilePicker filePicker)
        {
            var config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            config.EdgesFilePath = filePicker.RelationshipsText;

            config.VertexesFilePath = filePicker.PropertiesText;

            config.EnableProperty = filePicker.EnableProperty ?? false;

            GraphExplorerSection.Current.Save();
        }
    }
}