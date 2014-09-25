#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="VertexViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;

    public class VertexViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public DataVertex Data
        {
            get { return GetValue<DataVertex>(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Register the Data property so it is known in the class.
        /// </summary>
        public static readonly PropertyData DataProperty = RegisterProperty("Data", typeof(DataVertex));
    }
}