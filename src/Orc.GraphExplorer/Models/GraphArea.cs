using GraphX;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Models
{
    public class GraphArea : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        public GraphArea()
        {
            Logic = new GraphLogic();
        }
        public GraphLogic Logic { get; private set; }
    }

}
