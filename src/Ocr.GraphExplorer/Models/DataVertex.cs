#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVertex.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Media;
    using Catel.Data;

    using GraphX;

    public class DataVertex : ModelBase, IGraphXVertex
    {
        #region Constructors
        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        public DataVertex()
            : this(-666)
        {
        }

        public DataVertex(int id)
        {
            ID = id;
            Title = ID.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        #region IGraphXVertex Members
        /// <summary>
        /// Unique vertex ID
        /// </summary>
        public int ID { get; set; }

        public bool Equals(IGraphXVertex other)
        {
            if (other == null)
            {
                return false;
            }
            return ID == other.ID;
        }
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            return Equals(obj as IGraphXVertex);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ID.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ImageSource Icon
        {
            get { return GetValue<ImageSource>(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Register the Icon property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IconProperty = RegisterProperty("Icon", typeof(ImageSource), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Register the Title property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), null);

        public ObservableCollection<Property> Properties
        {
            get { return GetValue<ObservableCollection<Property>>(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// Register the Properties property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesProperty = RegisterProperty("Properties", typeof(ObservableCollection<Property>), () => new ObservableCollection<Property>());
        #endregion // Properties
    }
}