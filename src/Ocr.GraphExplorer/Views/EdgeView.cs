#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.MVVM.Views;
    using Catel.Windows;

    using GraphX;

    using Orc.GraphExplorer.Helpers;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views.Base;

    public class EdgeView : EdgeControl, IUserControl
    {
        private readonly UserControlLogic _logic;

        private event EventHandler<EventArgs> _viewLoaded;
        private event EventHandler<EventArgs> _viewUnloaded;
        private event EventHandler<EventArgs> _viewDataContextChanged;
        private event PropertyChangedEventHandler _propertyChanged;

        public EdgeView(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true)
            : base(source, target, edge, showLabels, showArrows) 
        {
            _logic = new UserControlLogic(this, typeof(EdgeViewModel));            
        }

        public override void BeginInit()
        {
            _logic.ViewModelChanged += (sender, args) => this.InvokeEvent(ViewModelChanged, args);
            _logic.Loaded += (sender, args) => _viewLoaded.SafeInvoke(this);
            _logic.Unloaded += (sender, args) => _viewUnloaded.SafeInvoke(this);

            _logic.PropertyChanged += (sender, args) => _propertyChanged.SafeInvoke(this, args);

            this.AddDataContextChangedHandler((sender, e) => this.InvokeEvent(_viewDataContextChanged, EventArgs.Empty));         

            base.BeginInit();
        }

        [ViewToViewModel]
        public bool IsHighlightEnabled {
            get
            {
                return (bool)GetValue(HighlightBehaviour.IsHighlightEnabledProperty);
            }
            set
            {
                SetValue(HighlightBehaviour.IsHighlightEnabledProperty, value);
            }
        }

        IViewModel IViewModelContainer.ViewModel
        {
            get { return _logic.ViewModel; }
        }

        public EdgeViewModel ViewModel
        {
            get { return _logic.ViewModel as EdgeViewModel; }
        }



        public event EventHandler<EventArgs> ViewModelChanged;

        event EventHandler<EventArgs> IView.Loaded
        {
            add
            {
                _viewLoaded += value;
            }
            remove
            {
                _viewLoaded -= value;
            }
        }

        event EventHandler<EventArgs> IView.Unloaded
        {
            add
            {
                _viewUnloaded += value;
            }
            remove
            {
                _viewUnloaded -= value;
            }
        }

        event EventHandler<EventArgs> IView.DataContextChanged
        {
            add
            {
                _viewDataContextChanged += value;
            }
            remove
            {
                _viewDataContextChanged -= value;
            }
        }

        public bool CloseViewModelOnUnloaded
        {
            get
            {
                return _logic.CloseViewModelOnUnloaded;
            }
            set
            {
                _logic.CloseViewModelOnUnloaded = value;
            }
        }

        public bool SupportParentViewModelContainers
        {
            get
            {
                return _logic.SupportParentViewModelContainers;
            }
            set
            {
                _logic.SupportParentViewModelContainers = value;
            }
        }

        public bool SkipSearchingForInfoBarMessageControl
        {
            get
            {
                return _logic.SkipSearchingForInfoBarMessageControl;
            }
            set
            {
                _logic.SkipSearchingForInfoBarMessageControl = value;
            }
        }

        public bool DisableWhenNoViewModel
        {
            get
            {
                return _logic.DisableWhenNoViewModel;
            }
            set
            {
                _logic.DisableWhenNoViewModel = value;
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }

        /// <summary>
        /// Content Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(EdgeView),
                new FrameworkPropertyMetadata((object)null));
        

        /// <summary>
        /// Gets or sets the CustomContent property.
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }    
    }
}