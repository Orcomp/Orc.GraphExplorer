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
    using System.Windows;
    using Catel.Data;
    using GraphX;
    using GraphX.Models.XmlSerializer;
    using ObjectModel;
    using YAXLib;

    [Serializable]
    public class DataEdge : ModelBase, IGraphXEdge<DataVertex>
    {
  
        #region Constructors
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
        {
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

        #region Properties
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsVisible
        {
            get { return GetValue<bool>(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsVisibleProperty = RegisterProperty("IsVisible", typeof(bool), () => true);

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
        public static readonly PropertyData IsHighlightEnabledProperty = RegisterProperty("IsHighlightEnabled", typeof(bool), () => false);
        #endregion // Properties
    }
}