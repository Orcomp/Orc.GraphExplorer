namespace Orc.GraphExplorer.Models.Data
{
    using System;

    using Orc.GraphExplorer.Services.Interfaces;

    using QuickGraph;

    public class Graph : BidirectionalGraph<DataVertex, DataEdge>, IObserver<DataVertex>, IObserver<DataEdge>
    {
        private readonly IGraphDataService _graphDataService;

        public Graph()
        {
            
        }

        public Graph(IGraphDataService graphDataService)
        {
            _graphDataService = graphDataService;
        }

        public void ReloadGraph()
        {
            RemoveEdgeIf(e => true);
            RemoveVertexIf(v => true);

            _graphDataService.GetVerteces().Subscribe(this);
            _graphDataService.GetEdges().Subscribe(this);
        }

        public void OnNext(DataVertex value)
        {
            AddVertex(value);
        }

        public void OnNext(DataEdge value)
        {
            AddEdge(value);
        }

        void IObserver<DataEdge>.OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        void IObserver<DataEdge>.OnCompleted()
        {
            //throw new NotImplementedException();
        }

        void IObserver<DataVertex>.OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        void IObserver<DataVertex>.OnCompleted()
        {
            //throw new NotImplementedException();
        }
    }
}
