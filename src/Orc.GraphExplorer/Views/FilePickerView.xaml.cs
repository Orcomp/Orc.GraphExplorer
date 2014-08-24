namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Configuration;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Events;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for FilePickerView.xaml
    /// </summary>
    public partial class FilePickerView 
    {
        // TODO: Replace this SettingApplied event
      /*  // Create a custom routed event by first registering a RoutedEventID 
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
        }*/

        public FilePickerView()
        {
            InitializeComponent();
        }                
    }
}
