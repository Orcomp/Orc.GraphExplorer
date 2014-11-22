﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Catel.Data;
    using Catel.Fody;
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
        [Expose("Key")]
        [Expose("Value")]
        [Expose("IsInEditing")]
        public Property Property { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [DefaultValue(false)]
        public bool IsSelected { get; set; }
    }
}