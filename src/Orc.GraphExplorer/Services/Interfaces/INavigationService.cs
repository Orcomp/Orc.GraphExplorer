#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="INavigationService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using Orc.GraphExplorer.Models;

    public interface INavigationService
    {
        void NavigateTo(Explorer explorer, DataVertex dataVertex);
    }
}