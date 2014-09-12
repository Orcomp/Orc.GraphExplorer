#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IDragable.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System;
    using System.Windows;

    public interface IDragable
    {
        DragDropEffects GetDragEffects();
      //  object GetData(object dataContext);
        Type DataType { get; }
    }
}