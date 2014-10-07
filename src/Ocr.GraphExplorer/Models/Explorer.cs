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
    using Catel.Memento;

    public class Explorer : ModelBase
    {
        public Explorer(IMementoService mementoService)
        {
            EditorToolset = new GraphToolset("Editor", mementoService);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public GraphToolset EditorToolset
        {
            get { return GetValue<GraphToolset>(EditorToolsetProperty); }
            set { SetValue(EditorToolsetProperty, value); }
        }

        /// <summary>
        /// Register the EditorToolset property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorToolsetProperty = RegisterProperty("EditorToolset", typeof(GraphToolset), null);
    }
}