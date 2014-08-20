using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orc.GraphExplorer;
using System.Configuration;

namespace Orc.GraphExplorer.Demo.Mock
{
    using DomainModel;
    using Orc.GraphExplorer.Models;
    using Services.Interfaces;

    //mock up service for generating moce vetexes and edges
    public class MockGraphDataService : IGraphDataService
    {
        public MockGraphDataService() { }

        List<DataVertex> vlist = new List<DataVertex>();
        public void GetVertexes(Action<IEnumerable<DataVertex>> onSuccess, Action<Exception> onFail)
        {
            MakeVertex();

            if (onSuccess != null)
            {
                onSuccess.Invoke(vlist);
            }
            //throw new NotImplementedException();
        }

        private void MakeVertex()
        {
            vlist.Clear();
            for (var i = 0; i < 10; i++)
            {
                var v = new DataVertex(i, "DataVertex" + i);
                vlist.Add(v);
            }
        }
        
        public void GetEdges(Action<IEnumerable<DataEdge>> onSuccess, Action<Exception> onFail)
        {
            if (vlist.Count == 0)
                MakeVertex();

            Random r = new Random();

            List<DataEdge> list = new List<DataEdge>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 3 == 0) continue;

                var e = new DataEdge(vlist[i],vlist[0]);
                list.Add(e);
            }

            if (onSuccess != null)
            {
                onSuccess.Invoke(list);
            }
        }

        public void Clear()
        {
            //throw new NotImplementedException();
        }

        public void UpdateVertexes(IEnumerable<DataVertex> vertexes, Action<bool, Exception> onComplete)
        {
            throw new NotImplementedException();
        }

        public void UpdateEdges(IEnumerable<DataEdge> vertexes, Action<bool, Exception> onComplete)
        {
            throw new NotImplementedException();
        }

        public void UpdateVertex(DataVertex vertex, Action<bool, DataVertex, Exception> onComplete)
        {

        }

        void IGraphDataService.Config(ConfigurationElement config)
        {
            throw new NotImplementedException();
        }
    }
}
