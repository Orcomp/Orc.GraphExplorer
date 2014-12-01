#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVertexFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using Catel;
    using Models;

    public class DataVertexFactory : IDataVertexFactory
    {
        #region Constants
        private const int FakeVertexId = -666;

        private static int _maxId = 0;
        #endregion

        #region IDataVertexFactory Members
        public DataVertex CreateFakeVertex()
        {
            return new DataVertex(FakeVertexId);
        }

        public DataVertex CreateVertex()
        {
            return new DataVertex(++_maxId);
        }

        public DataVertex CreateVertex(int id)
        {
            if (id > _maxId)
            {
                _maxId = id + 1;
            }

            return new DataVertex(id);
        }

        public bool IsFakeVertex(DataVertex vertex)
        {
            Argument.IsNotNull(() => vertex);

            return vertex.ID == FakeVertexId;
        }
        #endregion
    }
}