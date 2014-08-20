namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Configuration;
    using System.Windows;
    using System.Windows.Controls;
    using Events;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for FilePickerView.xaml
    /// </summary>
    public partial class FilePickerView : UserControl
    {
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        public static readonly RoutedEvent SettingAppliedEvent = EventManager.RegisterRoutedEvent(
            "SettingApplied", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilePickerView));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler SettingApplied
        {
            add { AddHandler(SettingAppliedEvent, value); }
            remove { RemoveHandler(SettingAppliedEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseSettingAppliedEvent(bool neeedRefresh)
        {
            RoutedEventArgs newEventArgs = new SettingAppliedRoutedEventArgs(FilePickerView.SettingAppliedEvent, this, neeedRefresh);
            RaiseEvent(newEventArgs);
        }

        public FilePickerView()
        {
            InitializeComponent();

            Loaded += FilePicker_Loaded;
        }

        void FilePicker_Loaded(object sender, RoutedEventArgs e)
        {
            Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            GraphExplorerSection section = (GraphExplorerSection)exeConfiguration.GetSection("graphExplorer");

            var config = section.CsvGraphDataServiceConfig;

            tbRelationships.Text = config.EdgesFilePath;

            tbProperties.Text = config.VertexesFilePath;

            cbProperties.IsChecked = config.EnableProperty;
            //throw new NotImplementedException();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

                config.EdgesFilePath = tbRelationships.Text;

                config.VertexesFilePath = tbProperties.Text;

                config.EnableProperty = cbProperties.IsChecked.HasValue?cbProperties.IsChecked.Value:false;

                GraphExplorerSection.Current.Save();

                RaiseSettingAppliedEvent(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void btnRelationship_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog() { Filter = "All files|*.csv", Title = "Select Relationship File" };
            if (dlg.ShowDialog() == true)
            {
                tbRelationships.Text = dlg.FileName;
            }
        }

        private void btnProperties_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog() { Filter = "All files|*.csv", Title = "Select Properties File" };
            if (dlg.ShowDialog() == true)
            {
                tbProperties.Text = dlg.FileName;
            }
        }
    }
}
