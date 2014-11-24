#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphAreaLoadingService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Threading.Tasks;

    using Orc.GraphExplorer.Models;

    public interface IGraphAreaLoadingService
    {
        void ReloadGraphArea(GraphArea graphArea, int offsetY);

        Task<bool> TryRefresh(GraphArea area);
    }
}