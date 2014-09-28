#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
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
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataVertex Data
        {
            get { return GetValue<DataVertex>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the Data property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("Data", typeof(DataVertex));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Data")]
        public ImageSource Icon
        {
            get { return GetValue<ImageSource>(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Register the Icon property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IconProperty = RegisterProperty("Icon", typeof(ImageSource));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Data")]
        public new string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Register the Title property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Data")]
        public ObservableCollection<Property> Properties
        {
            get { return GetValue<ObservableCollection<Property>>(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// Register the Properties property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesProperty = RegisterProperty("Properties", typeof(ObservableCollection<Property>));

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
        public static readonly PropertyData IsExpandedProperty = RegisterProperty("IsExpanded", typeof(bool), () => false);


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double X
        {
            get { return GetValue<double>(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Register the X property so it is known in the class.
        /// </summary>
        public static readonly PropertyData XProperty = RegisterProperty("X", typeof(double), () => double.NaN);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public double Y
        {
            get { return GetValue<double>(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Register the Y property so it is known in the class.
        /// </summary>
        public static readonly PropertyData YProperty = RegisterProperty("Y", typeof(double), () => double.NaN);

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
        public static readonly PropertyData IsDraggingProperty = RegisterProperty("IsDragging", typeof(bool), () => false);       
    }
}