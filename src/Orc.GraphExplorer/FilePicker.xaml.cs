using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Orc.GraphExplorer
{
    /// <summary>
    /// Interaction logic for FilePicker.xaml
    /// </summary>
    public partial class FilePicker : UserControl
    {
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        public static readonly RoutedEvent SettingAppliedEvent = EventManager.RegisterRoutedEvent(
            "SettingApplied", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilePicker));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler SettingApplied
        {
            add { AddHandler(SettingAppliedEvent, value); }
            remove { RemoveHandler(SettingAppliedEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseSettingAppliedEvent(bool neeedRefresh)
        {
            RoutedEventArgs newEventArgs = new SettingAppliedRoutedEventArgs(FilePicker.SettingAppliedEvent, this, neeedRefresh);
            RaiseEvent(newEventArgs);
        }

        public FilePicker()
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
            //throw new NotImplementedException();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                GraphExplorerSection section = (GraphExplorerSection)exeConfiguration.GetSection("graphExplorer");

                var config = section.CsvGraphDataServiceConfig;

                config.EdgesFilePath = tbRelationships.Text;

                config.VertexesFilePath = tbProperties.Text;

                exeConfiguration.Save(ConfigurationSaveMode.Modified);

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
