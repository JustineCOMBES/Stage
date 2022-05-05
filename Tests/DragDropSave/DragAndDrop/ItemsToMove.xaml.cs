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
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using System.Runtime;

namespace DragDropSave.DragAndDrop
{
    /// <summary>
    /// Logique d'interaction pour ItemsToMove.xaml
    /// </summary>
    public partial class ItemsToMove : UserControl
    {
        public ItemsToMove()
        {
            InitializeComponent();
        }
        #region Added functions
        public static readonly DependencyProperty IsChildHitTestVisibleProperty = 
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(ItemsToMove), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = 
            DependencyProperty.Register("Color", typeof(Brush), typeof(ItemsToMove), new PropertyMetadata(Brushes.Black));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        //public Shape shape
        //{
        //    get { return (shape) NameProperty(Shape)}
        //}


        Point dropPosition;
        public Point getData()
        {
            return (dropPosition);
        }

        #endregion
        #region Canvas functions
        private void canvas_DragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                canvas.Children.Remove(element);
            }
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            // empty
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                {
                    canvas.Children.Add(element);
                }
                
            }
        }

        
    
    #endregion
        #region Elements functions

            private void LabelRectangle_MouseMove(object sender, MouseEventArgs e)
            {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;
                DragDrop.DoDragDrop(LabelRectangle, new DataObject(DataFormats.Serializable, LabelRectangle), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        //    private void LabelEllipse_MouseMove(object sender, MouseEventArgs e)
        //    {
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //        {
        //            IsChildHitTestVisible = false;
        //            DragDrop.DoDragDrop(LabelEllipse, new DataObject(DataFormats.Serializable, LabelEllipse), DragDropEffects.Move);
        //            IsChildHitTestVisible = true;
        //        }
        //}
        #endregion
        #region Save
        //public List<double> Liste = new List<double>();

        //public void addDB(double pos)
        //{
        //    this.Liste.Add(pos);
        //}

        public void save() 
        {
            string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.Truncate);
            double RectangleLeft = Canvas.GetLeft(LabelRectangle);
            double RectangleTop = Canvas.GetTop(LabelRectangle);
            //addDB(RectangleLeft);
            //addDB(RectangleTop);

            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
               // foreach (double s in Liste)
               // {
                    sw.WriteLine(RectangleLeft);
                    sw.WriteLine(RectangleTop);
                   // sw.WriteLine(" ");
               // }
            }
        }

       // public List<double> Liste2 = new List<double>();
        public void load()
        {
            string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(stream, false);

            string ligne;
            ligne = sr.ReadLine();
            int i = 0;
            while (ligne != null)
            {
                //Read the next line
                
                if (ligne != " ") 
                { 
                    double coordonnee = Convert.ToDouble(ligne); 
                
                
                //this.Liste2.Add(coordonnee);
                    if(i % 2 == 0) 
                    {
                        Canvas.SetLeft(LabelRectangle, coordonnee);  
                    }
                    if (i % 2 == 1)
                    {
                        Canvas.SetTop(LabelRectangle, coordonnee);
                    }
                    ligne = sr.ReadLine();
                }

                i++;
            }
            sr.Close();
        }
        #endregion
    }
}
