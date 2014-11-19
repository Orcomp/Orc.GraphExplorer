#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Property.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;

    public class Property :ModelBase
    {
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string Key
        {
            get { return GetValue<string>(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// Register the Key property so it is known in the class.
        /// </summary>
        public static readonly PropertyData KeyProperty = RegisterProperty("Key", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string Value
        {
            get { return GetValue<string>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Register the Value property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ValueProperty = RegisterProperty("Value", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsInEditing
        {
            get { return GetValue<bool>(IsInEditingProperty); }
            set { SetValue(IsInEditingProperty, value); }
        }

        /// <summary>
        /// Register the IsInEditing property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsInEditingProperty = RegisterProperty("IsInEditing", typeof(bool), () => false);
    }
}