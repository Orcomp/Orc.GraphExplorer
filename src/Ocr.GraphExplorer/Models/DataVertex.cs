﻿#region Copyright (c) 2014 Orcomp development team.
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
    using System.Runtime.Serialization;
    using System.Windows.Media;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    using GraphX;
    using GraphX.Models.XmlSerializer;
    using YAXLib;

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class DataVertex : ModelBase, IGraphXVertex
    {
        #region Constructors
        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        private DataVertex()
            : this(-666)
        {
        }

        private DataVertex(int id)
        {
            ID = id;
            Title = ID.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        private static int _maxId = 0;
        public static DataVertex CreateFakeVertex()
        {
            return new DataVertex(-666);
        }

        public static DataVertex Create()
        {
            return new DataVertex(++_maxId);
        }

        public static DataVertex Create(int id)
        {
            if (id > _maxId)
            {
                _maxId = id + 1;
            }

            return new DataVertex(id);
        }

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
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [YAXSerializableField]
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
        [YAXSerializableField]
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Register the Title property so it is known in the class.
        /// </summary>
        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), null);

        [YAXSerializableField]
        public ObservableCollection<Property> Properties
        {
            get { return GetValue<ObservableCollection<Property>>(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// Register the Properties property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesProperty = RegisterProperty("Properties", typeof(ObservableCollection<Property>), () => new ObservableCollection<Property>());

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
        #endregion // Properties

        public static bool IsFakeVertex(DataVertex vertex)
        {
            return vertex.ID == -666;
        }
    }
}