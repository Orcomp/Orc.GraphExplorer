#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataEdge.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    using Catel;
    using Catel.Data;

    using GraphX;
    using GraphX.Models.XmlSerializer;

    using YAXLib;

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class DataEdge : ModelBase, IGraphXEdge<DataVertex>
    {
  
        #region Constructors
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
        {
            Argument.IsNotNull(() => source);
            Argument.IsNotNull(() => target);

            Source = source;
            Target = target;
            Weight = weight;
            ID = -1;
        }

        public DataEdge()
            : this(null, null, 1)
        {
        }
        #endregion

        #region IGraphXEdge<DataVertex> Members
        /// <summary>
        /// Unique edge ID
        /// </summary>
        public int ID { get; set; }

        public bool IsFiltered 
        {
            get
            {
                var source = Source;
                var target = Target;

                return source != null && target != null && (source.IsFiltered && target.IsFiltered);
            }
        }

        /// <summary>
        /// Returns true if Source vertex equals Target vertex
        /// </summary>
        [YAXDontSerialize]
        public bool IsSelfLoop
        {
            get { return Source.Equals(Target); }
        }

        /// <summary>
        /// Routing points collection used to make Path visual object
        /// </summary>
        [YAXCustomSerializer(typeof (YAXPointArraySerializer))]
        public Point[] RoutingPoints { get; set; }

        public DataVertex Source { get; set; }

        public DataVertex Target { get; set; }

        public double Weight { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsHighlightEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsInEditing { get; set; }
    }
}