#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdgeDrwingCanvas.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors.Interfaces
{
    using System.Windows;

    public interface IEdgeDrwingCanvas
    {
        bool SetLastPoint(Point point);
    }
}