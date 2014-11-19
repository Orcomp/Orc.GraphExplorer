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
        public PropertyViewModel()
        {
            
        }
        public PropertyViewModel(Property property)
        {
            Property = property;
        }

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

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Property")]
        public string Key
        {
            get { return GetValue<string>(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// Register the Key property so it is known in the class.
        /// </summary>
        public static readonly PropertyData KeyProperty = RegisterProperty("Key", typeof(string));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Property")]
        public string Value
        {
            get { return GetValue<string>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Register the Value property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ValueProperty = RegisterProperty("Value", typeof(string));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Property")]
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsSelected
        {
            get { return GetValue<bool>(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Register the IsSelected property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsSelectedProperty = RegisterProperty("IsSelected", typeof(bool), () => false);
    }
}