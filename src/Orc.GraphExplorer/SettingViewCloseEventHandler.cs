using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Orc.GraphExplorer
{
    public delegate void SettingAppliedEventHandler(object sender, SettingAppliedRoutedEventArgs e);

    public class SettingAppliedRoutedEventArgs : RoutedEventArgs
    {
        bool _needRefresh;

        public bool NeedRefresh
        {
            get { return _needRefresh; }
        }
        public SettingAppliedRoutedEventArgs(RoutedEvent routedEvent,object source,bool needRefresh):base(routedEvent,source)
        {
            _needRefresh = needRefresh;
        }
    }
}
