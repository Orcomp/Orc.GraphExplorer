using System;
using System.Collections.Generic;
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

namespace Orc.GraphExplorer.Views
{
    using Orc.GraphExplorer.Messages;

    /// <summary>
    /// Логика взаимодействия для GraphToolsetView.xaml
    /// </summary>
    public partial class GraphToolsetView
    {
        public GraphToolsetView()
        {
            InitializeComponent();
            SaveToXmlMessage.Register(this, OnSaveToXmlMesage);
            LoadFromXmlMessage.Register(this, OnLoadFromXmlMessage);
            SaveToImageMessage.Register(this, OnSaveToImageMessage);
        }

        private void OnSaveToImageMessage(SaveToImageMessage message)
        {
            AreaView.ExportAsImage(message.Data);
        }

        private void OnLoadFromXmlMessage(LoadFromXmlMessage message)
        {
            // TODO: test and fix it
            AreaView.ClearLayout();
            AreaView.LoadFromFile(message.Data);
            AreaView.RelayoutGraph();
        }

        private void OnSaveToXmlMesage(SaveToXmlMessage message)
        {
            AreaView.SaveIntoFile(message.Data);
        }

        protected override void OnViewModelChanged()
        {
            DataContext = ViewModel;
            base.OnViewModelChanged();
        }
    }
}
