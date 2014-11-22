#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Collections.ObjectModel;

    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public class FilterViewModel : ViewModelBase
    {
        public FilterViewModel(Filter filter)
        {
            Filter = filter;
        }


        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        [Expose("FilterableEntities")]
        [Expose("FilteredEntities")]
        [Expose("IsHideVertexes")]
        public Filter Filter { get; set; }
    }
}