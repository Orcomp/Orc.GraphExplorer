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
        public EdgeViewModel()
        {
            
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataEdge Data
        {
            get { return GetValue<DataEdge>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the Data property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("Data", typeof(DataEdge));
    }
}