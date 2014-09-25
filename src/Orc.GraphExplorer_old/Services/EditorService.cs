#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Windows.Media;

    using GraphX;
    using Models;
    using Orc.GraphExplorer.ObjectModel;

    public class EditorService : IEditorService
    {
        private EdgeControl _edEdge;

        public EditorService()
        {
            
        }

        public DataEdge GetEdEdge()
        {
            return _edEdge.Edge as DataEdge;
        }

        public void ClearEdEdge()
        {
            _edEdge = null;
        }

        public bool IsEdgeEditing
        {
            get
            {
                return _edEdge != null;
            }
        }

        public void SetEdgePathManually(PathGeometry edGeo)
        {
            _edEdge.SetEdgePathManually(edGeo);
        }

        public void SetEdEdge(EdgeControl edgeControl)
        {
            _edEdge = edgeControl;
        }
    }
}