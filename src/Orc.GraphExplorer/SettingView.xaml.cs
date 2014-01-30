﻿using System;
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

namespace Orc.GraphExplorer
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            "SettingApplied", RoutingStrategy.Bubble, typeof(SettingAppliedEventHandler), typeof(SettingView));

        // Provide CLR accessors for the event 
        public event SettingAppliedEventHandler SettingApplied
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }

        // This method raises the Close event
        void RaiseSettingAppliedEvent(bool needRefresh)
        {
            RoutedEventArgs newEventArgs = new SettingAppliedRoutedEventArgs(SettingView.CloseEvent, this, needRefresh);
            RaiseEvent(newEventArgs);
        } 

        public SettingView()
        {
            InitializeComponent();
        }

        private void FileLocationSaved_Loaded(object sender, RoutedEventArgs e)
        {
            RaiseSettingAppliedEvent(true);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            RaiseSettingAppliedEvent(false);
        }
    }
}
