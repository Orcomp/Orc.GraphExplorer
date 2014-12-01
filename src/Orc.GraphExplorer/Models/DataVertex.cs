// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVertex.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.GraphExplorer.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Media;
    using Catel.Data;
    using GraphX;
    using YAXLib;

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class DataVertex : ModelBase, IGraphXVertex
    {
        #region Constructors
        /// <summary>
        /// Default constructor for this class
        /// (required for serialization).
        /// </summary>
        public DataVertex()
        {
            Properties = new ObservableCollection<Property>();
        }

        public DataVertex(int id)
            : this()
        {
            ID = id;
            Title = ID.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        #region Properties
        public static int FakeVertexId
        {
            get { return -666; }
        }

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
        public ObservableCollection<Property> Properties { get; set; }

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

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDragging { get; set; }
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
    }
}