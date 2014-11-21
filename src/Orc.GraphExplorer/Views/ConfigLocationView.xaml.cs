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

    /// <summary>
    /// Interaction logic for FilePickerView.xaml
    /// </summary>
    public partial class ConfigLocationView
    {
        // TODO: Replace this SettingApplied event
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        private static readonly RoutedEvent SettingAppliedEvent = EventManager.RegisterRoutedEvent(
            "SettingApplied", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConfigLocationView));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler SettingApplied
        {
            add { AddHandler(SettingAppliedEvent, value); }
            remove { RemoveHandler(SettingAppliedEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseSettingAppliedEvent(bool neeedRefresh)
        {
            RoutedEventArgs newEventArgs = new SettingAppliedRoutedEventArgs(ConfigLocationView.SettingAppliedEvent, this, neeedRefresh);
            RaiseEvent(newEventArgs);
        }

        public ConfigLocationView()
        {
            InitializeComponent();
        }
    }
}
