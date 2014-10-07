#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DragableButtonViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Windows;
    using Behaviors.Interfaces;

    using Catel.Data;
    using Catel.MVVM;
    using Models;
    using Orc.GraphExplorer.Behaviors;

    public class DragableButtonViewModel : ViewModelBase, IDragable
    {
        public DragableButtonViewModel(GraphToolsetViewModel toolsetViewModel)
        {
            PrimitivesCreator = toolsetViewModel.PrimitivesCreator;
        }

        public DragableButtonViewModel(PrimitivesCreator primitivesCreator)
        {
            PrimitivesCreator = primitivesCreator;
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public PrimitivesCreator PrimitivesCreator
        {
            get { return GetValue<PrimitivesCreator>(PrimitivesCreatorProperty); }
            private set { SetValue(PrimitivesCreatorProperty, value); }
        }

        /// <summary>
        /// Register the PrimitivesCreator property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PrimitivesCreatorProperty = RegisterProperty("PrimitivesCreator", typeof(PrimitivesCreator));

        public DragDropEffects GetDragEffects()
        {
            return DragDropEffects.Copy;
        }

        public object GetData()
        {
            return PrimitivesCreator.NewDataVertex();
        }

        public Type DataType {
            get
            {
                return typeof(DataVertex);
            }
        }
    }
}