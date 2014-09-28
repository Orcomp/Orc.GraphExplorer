#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel.Data;
    using Catel.MVVM;
    using Models;

    public class PropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public Property Property
        {
            get { return GetValue<Property>(PropertyProperty); }
            private set { SetValue(PropertyProperty, value); }
        }

        /// <summary>
        /// Register the Property property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertyProperty = RegisterProperty("Property", typeof(Property));
    }
}