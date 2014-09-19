namespace Orc.GraphExplorer.Services
{
    using System.Windows.Media;

    using GraphX;
    using Models;
    using Orc.GraphExplorer.ObjectModel;

    public interface IEditorService
    {
        DataEdge GetEdEdge();

        void ClearEdEdge();

        bool IsEdgeEditing { get; }

        void SetEdgePathManually(PathGeometry edGeo);

        void SetEdEdge(EdgeControl edgeControl);
    }
}