using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_Drag_and_DRop.Introduction
{
    /// <summary>
    /// Logique d'interaction pour Canvas1xaml.xaml
    /// </summary>
    public partial class Canvas1xaml : UserControl
    {
        public static readonly DependencyProperty IsChildHitTestVisibleProperty = DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(Canvas1xaml), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public Canvas1xaml()
        {
            InitializeComponent();
        }

        private void redRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;
                DragDrop.DoDragDrop(redRectangle, new DataObject(DataFormats.Serializable, redRectangle), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            
        }

        private void canvas_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == canvas)
            {

                object data = e.Data.GetData(DataFormats.Serializable);

                if (data is UIElement element)
                {
                    canvas.Children.Remove(element);
                }
            }
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                {
                    canvas.Children.Add(element);
                }
            }
        }
    }
}
