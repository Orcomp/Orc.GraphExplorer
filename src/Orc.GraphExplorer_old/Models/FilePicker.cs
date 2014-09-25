#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePicker.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using System.Configuration;
    using Catel.Data;
    using Catel.MVVM;

    public class FilePicker : ModelBase
    {
        public FilePicker()
        {
            
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string RelationshipsText
        {
            get { return GetValue<string>(RelationshipsTextProperty); }
            set { SetValue(RelationshipsTextProperty, value); }
        }

        /// <summary>
        /// Register the RelationshipsText property so it is known in the class.
        /// </summary>
        public static readonly PropertyData RelationshipsTextProperty = RegisterProperty("RelationshipsText", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public string PropertiesText
        {
            get { return GetValue<string>(PropertiesTextProperty); }
            set { SetValue(PropertiesTextProperty, value); }
        }

        /// <summary>
        /// Register the PropertiesText property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PropertiesTextProperty = RegisterProperty("PropertiesText", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool? EnableProperty
        {
            get { return GetValue<bool?>(EnablePropertyProperty); }
            set { SetValue(EnablePropertyProperty, value); }
        }

        /// <summary>
        /// Register the EnableProperty property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EnablePropertyProperty = RegisterProperty("EnableProperty", typeof(bool?), null);
    }
}