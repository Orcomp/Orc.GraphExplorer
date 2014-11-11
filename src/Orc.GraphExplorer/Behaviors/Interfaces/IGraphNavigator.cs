#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphNavigator.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors.Interfaces
{
    using Models;
    using Models.Data;

    public interface IGraphNavigator
    {
        void NavigateTo(DataVertex dataVertex);        
    }
}