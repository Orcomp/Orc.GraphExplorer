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
    using System.ComponentModel;
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
        private const int FakeVertexId = -666;
        #region Constructors
        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        private DataVertex()
            : this(FakeVertexId)
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
            return new DataVertex(FakeVertexId);
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
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsFiltered { get; set; }

        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [YAXSerializableField]
        public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [YAXSerializableField]
        public string Title { get; set; }

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
        [DefaultValue(double.NaN)]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(double.NaN)]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }
        #endregion // Properties

        public static bool IsFakeVertex(DataVertex vertex)
        {
            return vertex.ID == FakeVertexId;
        }
    }
}