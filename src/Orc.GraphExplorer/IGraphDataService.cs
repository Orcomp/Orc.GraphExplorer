﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer
{
    public interface IGraphDataService
    {
        void GetVertexes(Action<IEnumerable<DataVertex>> onSuccess, Action<Exception> onFail);

        void GetEdges(Action<IEnumerable<DataEdge>> onSuccess, Action<Exception> onFail);

        void Clear();

        void UpdateVertex(DataVertex vertex, Action<bool, DataVertex, Exception> onComplete);

        void UpdateVertexes(IEnumerable<DataVertex> vertexes, Action<bool,Exception> onComplete);

        void UpdateEdges(IEnumerable<DataEdge> vertexes, Action<bool, Exception> onComplete); 
    }
}
