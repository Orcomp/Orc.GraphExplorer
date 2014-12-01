namespace Orc.GraphExplorer
{
    using System.Windows;

    public class SettingAppliedRoutedEventArgs : RoutedEventArgs
    {
        readonly bool _needRefresh;

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
