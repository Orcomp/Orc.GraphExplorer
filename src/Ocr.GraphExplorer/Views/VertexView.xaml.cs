using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Orc.GraphExplorer.Views
{
    using Catel.MVVM.Views;
    using GraphX;

    /// <summary>
    /// Логика взаимодействия для VertexView.xaml
    /// </summary>
    public partial class VertexView
    {
        public VertexView(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true)
            : base(vertexData, tracePositionChange, bindToDataObject)
        {
        }

        [ViewToViewModel]
        public bool IsHighlightEnabled
        {
            get { return HighlightBehaviour.GetIsHighlightEnabled(this); }
            set { HighlightBehaviour.SetIsHighlightEnabled(this, value); }
        }

        [ViewToViewModel]
        public bool IsHighlighted
        {
            get { return HighlightBehaviour.GetHighlighted(this); }
            set { HighlightBehaviour.SetHighlighted(this, value); }
        }

        [ViewToViewModel]
        public bool IsDragEnabled
        {
            get { return DragBehaviour.GetIsDragEnabled(this); }
            set { DragBehaviour.SetIsDragEnabled(this, value); }
        }

        [ViewToViewModel]
        public new bool IsVisible
        {
            get { return base.IsVisible; }
            set { base.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        } 
        
        [ViewToViewModel]
        public new bool IsEnabled
        {
            get { return base.IsEnabled; }
            set { base.IsEnabled = value; }
        } 
    }
}
