#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Explorer.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;

    public class Explorer : ModelBase
    {
        public Explorer()
        {            
            Toolset = new GraphToolset();
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset Toolset
        {
            get { return GetValue<GraphToolset>(ToolsetProperty); }
            set { SetValue(ToolsetProperty, value); }
        }

        /// <summary>
        /// Register the Toolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ToolsetProperty = RegisterProperty("Toolset", typeof(GraphToolset), null);
    }
}