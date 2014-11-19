#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IDropable.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System;
    using System.Windows;

    public interface IDropable
    {
        Type DataTypeFormat { get; }
        #region Methods
        /// <summary>
        /// Gets the effects.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        DragDropEffects GetDropEffects(IDataObject dataObject);

        /// <summary>
        /// Drops the specified data object
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="position"></param>
        void Drop(IDataObject dataObject, Point position);
        #endregion
    }
}