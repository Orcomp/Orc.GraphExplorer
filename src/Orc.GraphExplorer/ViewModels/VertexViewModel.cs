#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class VertexViewModel : ViewModelBase
    {
        public VertexViewModel()
        {
            
        }

        public VertexViewModel(DataVertex dataVertex)
        {
            DataVertex = dataVertex;
            AddCommand = new Command(OnAddCommandExecute);
            DeleteCommand = new Command(OnDeleteCommandExecute, OnDeleteCommandCanExecute);

            DeleteVertexCommand = new Command(OnDeleteVertexCommandExecute, OnDeleteVertexCommandCanExecute);
        }

        protected override void Initialize()
        {
            base.Initialize();
            SyncWithAreaProperties();
        }

        private void SyncWithAreaProperties()
        {
            var graphAreaViewModel = GraphAreaViewModel;
            if (graphAreaViewModel == null)
            {
                return;
            }
            IsInEditing = graphAreaViewModel.IsInEditing;
            IsDragEnabled = graphAreaViewModel.IsDragEnabled;
        }

        public GraphAreaViewModel GraphAreaViewModel
        {
            get { return base.ParentViewModel as GraphAreaViewModel; }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataVertex DataVertex
        {
            get { return GetValue<DataVertex>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the DataVertex property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("DataVertex", typeof (DataVertex));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public ImageSource Icon
        {
            get { return GetValue<ImageSource>(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Register the Icon property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IconProperty = RegisterProperty("Icon", typeof (ImageSource));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public new string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Register the Title property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof (string));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public ObservableCollection<Property> Properties
        {
            get { return GetValue<ObservableCollection<Property>>(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// Register the Properties property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesProperty = RegisterProperty("Properties", typeof (ObservableCollection<Property>));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public double X
        {
            get { return GetValue<double>(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Register the X property so it is known in the class.
        /// </summary>
        public static readonly PropertyData XProperty = RegisterProperty("X", typeof (double));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public double Y
        {
            get { return GetValue<double>(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Register the Y property so it is known in the class.
        /// </summary>
        public static readonly PropertyData YProperty = RegisterProperty("Y", typeof (double));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsExpanded
        {
            get { return GetValue<bool>(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Register the IsExpanded property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsExpandedProperty = RegisterProperty("IsExpanded", typeof (bool), () => false);


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsDragging
        {
            get { return GetValue<bool>(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }

        /// <summary>
        /// Register the IsDragging property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDraggingProperty = RegisterProperty("IsDragging", typeof (bool), () => false);

        /// <summary>
        /// Gets the AddCommand command.
        /// </summary>
        public Command AddCommand { get; private set; }


        /// <summary>
        /// Method to invoke when the AddCommand command is executed.
        /// </summary>
        private void OnAddCommandExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the DeleteCommand command.
        /// </summary>
        public Command DeleteCommand { get; private set; }

        /// <summary>
        /// Method to check whether the DeleteCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnDeleteCommandCanExecute()
        {
            return Properties != null && Properties.Count > 0;
        }

        /// <summary>
        /// Method to invoke when the DeleteCommand command is executed.
        /// </summary>
        private void OnDeleteCommandExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the DeleteVertexCommand command.
        /// </summary>
        public Command DeleteVertexCommand { get; private set; }

        /// <summary>
        /// Method to check whether the DeleteVertexCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnDeleteVertexCommandCanExecute()
        {
            return IsInEditing;
        }

        /// <summary>
        /// Method to invoke when the DeleteVertexCommand command is executed.
        /// </summary>
        private void OnDeleteVertexCommandExecute()
        {
            var areaViewModel = AreaViewModel;

            if (areaViewModel != null)
            {
                areaViewModel.RemoveVertex(DataVertex);
            }
        }

        public GraphAreaViewModel AreaViewModel
        {
            get
            {
                return ParentViewModel as GraphAreaViewModel;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlightEnabled
        {
            get { return GetValue<bool>(IsHighlightEnabledProperty); }
            set { SetValue(IsHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlightEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightEnabledProperty = RegisterProperty("IsHighlightEnabled", typeof (bool), () => true);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlighted
        {
            get { return GetValue<bool>(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlighted property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightedProperty = RegisterProperty("IsHighlighted", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsDragEnabled
        {
            get { return GetValue<bool>(IsDragEnabledProperty); }
            set { SetValue(IsDragEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsDragEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsDragEnabledProperty = RegisterProperty("IsDragEnabled", typeof (bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof (bool), () => false, (sender, args) => ((VertexViewModel) sender).OnIsInEditingChanged());

        private void OnIsInEditingChanged()
        {
            foreach (var property in Properties)
            {
                property.IsInEditing = IsInEditing;
            }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("DataVertex")]
        public bool IsVisible
        {
            get { return GetValue<bool>(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsVisibleProperty = RegisterProperty("IsVisible", typeof (bool));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsEnabled
        {
            get { return GetValue<bool>(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsEnabledProperty = RegisterProperty("IsEnabled", typeof (bool), () => true);
    }
}