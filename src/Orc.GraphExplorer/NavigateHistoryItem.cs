using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    using DomainModel;
    using Models;

    public class NavigateHistoryItem
    {
        public List<DataVertex> Vertexes { get; set; }

        public List<DataEdge> Edges { get; set; }

        public DataVertex NavigateFrom { get; set; }
    }
}
