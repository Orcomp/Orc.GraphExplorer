#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class EdgeViewModel : ViewModelBase
    {
        public EdgeViewModel(DataEdge dataEdge)
        {
            DataEdge = dataEdge;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataEdge DataEdge
        {
            get { return GetValue<DataEdge>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the DataEdge property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("DataEdge", typeof(DataEdge));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlightEnabled
        {
            get { return GetValue<bool>(IsHighlightEnabledProperty); }
            set { SetValue(IsHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlightEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightEnabledProperty = RegisterProperty("IsHighlightEnabled", typeof(bool), () => true);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsHighlighted
        {
            get { return GetValue<bool>(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        /// <summary>
        /// Register the IsHighlighted property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsHighlightedProperty = RegisterProperty("IsHighlighted", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsVisible
        {
            get { return GetValue<bool>(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsVisibleProperty = RegisterProperty("IsVisible", typeof(bool), () => true);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsEnabled
        {
            get { return GetValue<bool>(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Register the IsEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsEnabledProperty = RegisterProperty("IsEnabled", typeof(bool), () => true);
    }
}