namespace Orc.GraphExplorer.DomainModel
{
    using GraphX;

    using Orc.GraphExplorer.Models;

    using QuickGraph;

    public class GraphArea : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        public GraphArea()
        {
            LogicCore = new GraphLogic();
        }
    }

}
