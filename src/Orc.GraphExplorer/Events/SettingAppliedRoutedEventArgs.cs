// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingAppliedRoutedEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.GraphExplorer
{
    using System.Windows;

    public class SettingAppliedRoutedEventArgs : RoutedEventArgs
    {
        #region Fields
        private readonly bool _needRefresh;
        #endregion

        #region Constructors
        public SettingAppliedRoutedEventArgs(RoutedEvent routedEvent, object source, bool needRefresh) 
            : base(routedEvent, source)
        {
            _needRefresh = needRefresh;
        }
        #endregion

        #region Properties
        public bool NeedRefresh
        {
            get { return _needRefresh; }
        }
        #endregion
    }
}