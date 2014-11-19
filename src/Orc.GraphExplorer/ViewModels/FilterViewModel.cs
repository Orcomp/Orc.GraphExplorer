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

    using Catel.Data;
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
        public Filter Filter
        {
            get { return GetValue<Filter>(FilterProperty); }
            private set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Register the Filter property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterProperty = RegisterProperty("Filter", typeof(Filter));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter", Mode= ViewModelToModelMode.Explicit)]
        public ObservableCollection<FilterableEntity> FilterableEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilterableEntitiesProperty); }
            set { SetValue(FilterableEntitiesProperty, value); }
        }

        /// <summary>
        /// Register the FilterableEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilterableEntitiesProperty = RegisterProperty("FilterableEntities", typeof(ObservableCollection<FilterableEntity>));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public ObservableCollection<FilterableEntity> FilteredEntities
        {
            get { return GetValue<ObservableCollection<FilterableEntity>>(FilteredEntitiesProperty); }
            set { SetValue(FilteredEntitiesProperty, value); }
        }



        /// <summary>
        /// Register the FilteredEntities property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredEntitiesProperty = RegisterProperty("FilteredEntities", typeof(ObservableCollection<FilterableEntity>));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Filter")]
        public bool IsHideVertexes
        {
            get { return GetValue<bool>(IsHideVertexesProperty); }
            set { SetValue(IsHideVertexesProperty, value); }
        }

        /// <summary>
        /// Register the IsHideVertexes property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHideVertexesProperty = RegisterProperty("IsHideVertexes", typeof(bool));
    }
}