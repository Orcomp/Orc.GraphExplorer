#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDataRecord.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Csv.Data
{
    public class PropertyDataRecord
    {
        #region Properties
        public int ID { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }
        #endregion
    }
}