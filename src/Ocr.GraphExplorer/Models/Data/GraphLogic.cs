#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphLogic.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models.Data
{
    using GraphX.Logic;

    using Orc.GraphExplorer.Services.Interfaces;

    public class GraphLogic : GXLogicCore<DataVertex, DataEdge, Graph>
    {
        private readonly IGraphDataService _graphDataService;

        public GraphLogic(IGraphDataService graphDataService)
        {
            _graphDataService = graphDataService;
        }

        public void ReloadGraph()
        {
        }
    }
}