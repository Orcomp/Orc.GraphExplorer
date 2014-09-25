#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableAreaViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class EditableAreaViewModel : ViewModelBase
    {
        public EditableAreaViewModel()
        {
            Editor = new GraphEditor();
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public GraphEditor Editor
        {
            get { return GetValue<GraphEditor>(EditorProperty); }
            private set { SetValue(EditorProperty, value); }
        }

        /// <summary>
        /// Register the Editor property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorProperty = RegisterProperty("Editor", typeof(GraphEditor));
    }
}