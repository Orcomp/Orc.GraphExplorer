#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IOverridableGraphDataGetter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System;
    using System.Collections.Generic;

    using Orc.GraphExplorer.Models;

    public interface IOverridableGraphDataGetter
    {
        void RedefineVertecesGetter(Func<IEnumerable<DataVertex>> getter);

        void RedefineEdgesGetter(Func<IEnumerable<DataEdge>> getter);
    }
}