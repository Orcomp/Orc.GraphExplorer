using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public class NavigateHistoryItem
    {
        public List<DataVertex> Vertexes { get; set; }

        public List<DataEdge> Edges { get; set; }

        public DataVertex NavigateFrom { get; set; }
    }
}
