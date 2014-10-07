#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimitivesCreator.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;

    public class PrimitivesCreator : ModelBase
    {
        public DataVertex NewDataVertex()
        {
            return DataVertex.Create();
        }
    }
}