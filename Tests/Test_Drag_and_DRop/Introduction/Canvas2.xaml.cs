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
    /// Logique d'interaction pour Canvas2.xaml
    /// </summary>
    public partial class Canvas2 : UserControl
    {
        public static readonly DependencyProperty  IsChildHitTestVisibleProperty = DependencyProperty.Register("MyProperty", typeof(bool), typeof(Canvas2), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            get { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public Canvas2()
        {
            InitializeComponent();
        }

        private void blueRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //blueRectangle.IsHitTestVisible = false;
                DragDrop.DoDragDrop(blueRectangle, new DataObject(DataFormats.Serializable, blueRectangle), DragDropEffects.Move);
            }
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            
        }

        private void canvas_DragLeave(object sender, DragEventArgs e)
        {
            

            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                canvas.Children.Remove(element);
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
