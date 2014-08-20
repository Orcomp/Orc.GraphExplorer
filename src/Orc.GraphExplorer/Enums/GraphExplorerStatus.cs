namespace Orc.GraphExplorer.Enums
{
    using System;

    [Flags]
    public enum GraphExplorerStatus
    {
        Ready,
        Editing,
        EnableDrag,
        DragToCreateVertex,
        CreateLinkSelectSource,
        CreateLinkSelectTarget,
        Dragging
    }
}