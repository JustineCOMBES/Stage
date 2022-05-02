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

namespace Test_Drag_and_DRop
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        //private void redRectangle_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if(e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragDrop.DoDragDrop(redRectangle, redRectangle, DragDropEffects.Move);
        //    }
        //}

        //private void Canvas_Drop(object sender, DragEventArgs e)
        //{
        //    //Point dropPosition = e.GetPosition(canvas);
        //    //Canvas.SetLeft(redRectangle, dropPosition.X);
        //    //Canvas.SetTop(redRectangle, dropPosition.Y);

        //}

        //private void canvas_DragOver(object sender, DragEventArgs e)
        //{
        //    Point dropPosition = e.GetPosition(canvas);
        //    Canvas.SetLeft(redRectangle, dropPosition.X);
        //    Canvas.SetTop(redRectangle, dropPosition.Y);
        //}
    }
}
