namespace Orc.GraphExplorer.Events
{
    using System;

    public class VertexPositionChangedEventArgs : EventArgs
    {
        #region Constructors
        public VertexPositionChangedEventArgs(double x, double y, double offsetx, double offsety)
        {
            X = x;
            Y = y;
            OffsetX = offsetx;
            OffsetY = offsety;
        }
        #endregion

        #region Properties
        public double X { get; private set; }

        public double Y { get; private set; }

        public double OffsetX { get; private set; }

        public double OffsetY { get; private set; }
        #endregion
    }
}