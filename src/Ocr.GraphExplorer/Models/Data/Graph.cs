namespace Orc.GraphExplorer.Models.Data
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

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

        private ReplaySubject<Unit> _observableLoading;

        private ReplaySubject<DataEdge> _edgesBuffer; 

        public IObservable<Unit> ReloadGraph()
        {
            _observableLoading = new ReplaySubject<Unit>();
            _edgesBuffer = new ReplaySubject<DataEdge>();

            RemoveEdgeIf(e => true);
            RemoveVertexIf(v => true);

            _graphDataService.GetVerteces().Subscribe(this);
            _graphDataService.GetEdges().Subscribe(_edgesBuffer);

            return _observableLoading;
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
            _observableLoading.OnError(error);
        }

        void IObserver<DataEdge>.OnCompleted()
        {
            _observableLoading.OnNext(Unit.Default);
            _observableLoading.OnCompleted();
        }

        void IObserver<DataVertex>.OnError(Exception error)
        {
            _observableLoading.OnError(error);
        }

        void IObserver<DataVertex>.OnCompleted()
        {
            _edgesBuffer.Subscribe(this);
        }
    }
}
