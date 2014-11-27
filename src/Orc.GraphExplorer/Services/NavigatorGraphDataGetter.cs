#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigatorGraphDataGetter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Models;

    public class NavigatorGraphDataGetter : IGraphDataGetter, IOverridableGraphDataGetter
    {
        #region Fields
        private Func<IEnumerable<DataVertex>> _vertecesGetter;

        private Func<IEnumerable<DataEdge>> _edgesGetter;
        #endregion

        #region IGraphDataGetter Members
        public IEnumerable<DataVertex> GetVerteces()
        {
            return _vertecesGetter == null ? Enumerable.Empty<DataVertex>() : _vertecesGetter();
        }

        public IEnumerable<DataEdge> GetEdges()
        {
            return _edgesGetter == null ? Enumerable.Empty<DataEdge>() : _edgesGetter();
        }
        #endregion

        #region IOverridableGraphDataGetter Members
        public void RedefineVertecesGetter(Func<IEnumerable<DataVertex>> getter)
        {
            Argument.IsNotNull(() => getter);

            _vertecesGetter = getter;
        }

        public void RedefineEdgesGetter(Func<IEnumerable<DataEdge>> getter)
        {
            Argument.IsNotNull(() => getter);

            _edgesGetter = getter;
        }
        #endregion
    }
}