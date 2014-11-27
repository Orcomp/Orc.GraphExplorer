#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeDrawingService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Messages;
    using Models;
    using Models.Data;

    public class EdgeDrawingService : IEdgeDrawingService
    {
        private readonly IDataVertexFactory _dataVertexFactory;

        #region Fields
        private DataVertex _startVertex;
        private DataVertex _fakeEndVertex;
        private PathGeometry _pathGeometry;
        private Graph _graph;
        #endregion

        public EdgeDrawingService(IDataVertexFactory dataVertexFactory)
        {
            Argument.IsNotNull(() => dataVertexFactory);

            _dataVertexFactory = dataVertexFactory;
        }

        #region IEdgeDrawingService Members
        public bool IsInDrawing()
        {
            return _startVertex != null;
        }

        public void StartEdgeDrawing(Graph graph, DataVertex startVertex, Point startPoint, Point lastPoint)
        {
            Argument.IsNotNull(() => graph);
            Argument.IsNotNull(() => startVertex);

            _graph = graph;
            _startVertex = startVertex;
            _fakeEndVertex = _dataVertexFactory.CreateFakeVertex();

            var pathFigureCollection = new PathFigureCollection
            {
                new PathFigure
                {
                    IsClosed = false, StartPoint = startPoint, Segments = new PathSegmentCollection
                    {
                        new PolyLineSegment(new List<Point> {new Point()}, true)
                    }
                }
            };
            _pathGeometry = new PathGeometry(pathFigureCollection);

            SetLastPoint(lastPoint);

            var dedge = new DataEdge(_startVertex, _fakeEndVertex);

            _graph.AddVertex(_fakeEndVertex);
            _graph.AddEdge(dedge);

            StatusMessage.SendWith("Select Target Node");
        }

        public bool IsStartVertex(DataVertex dataVertex)
        {
            Argument.IsNotNull(() =>dataVertex);

            return Equals(_startVertex, dataVertex);
        }

        public void FinishEdgeDrawing(DataVertex endVertex)
        {
            Argument.IsNotNull(() => endVertex);

            _graph.RemoveVertex(_fakeEndVertex);

            _startVertex = null;
            _fakeEndVertex = null;
            _pathGeometry.Clear();
            _pathGeometry = null;
            _graph = null;
        }

        public DataVertex GetStartVertex()
        {
            return _startVertex;
        }

        public void MoveBrush(Point point)
        {
            if (_pathGeometry == null || _startVertex == null || _fakeEndVertex == null)
            {
                return;
            }

            SetLastPoint(point);
        }

        public PathGeometry GetEdgeGeometry()
        {
            return _pathGeometry;
        }
        #endregion

        #region Methods
        private void SetLastPoint(Point point)
        {
            var lastseg = _pathGeometry.Figures[0].Segments[_pathGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
            if (lastseg != null)
            {
                lastseg.Points[lastseg.Points.Count - 1] = point;
            }
        }
        #endregion
    }
}