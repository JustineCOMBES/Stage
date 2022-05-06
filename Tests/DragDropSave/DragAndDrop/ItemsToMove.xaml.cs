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
                Canvas.SetZIndex(element, numberOfElement + 1);
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

        #endregion
        #region Save & Load
        public List<double> Liste = new List<double>();

        public void addDB(double pos)
        {
           this.Liste.Add(pos);
        }

        public void save() 
        {
            string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.Truncate);
           // double RectangleLeft = Canvas.GetLeft(LabelRectangle);
            //double RectangleTop = Canvas.GetTop(LabelRectangle);
            

            IEnumerable<UIElement> uIElements = canvas.Children.OfType<UIElement>();
            foreach (UIElement element in uIElements) 
            {
                double posTop = Canvas.GetTop(element);
                double posLeft = Canvas.GetLeft(element);
                addDB(posTop);
                addDB(posLeft);
            }

            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                sw.WriteLine("Number of elements");
                sw.WriteLine(numberOfElement);
                sw.WriteLine("Points");
                foreach (double s in Liste)
                { 
                    sw.WriteLine(s);
                    //sw.WriteLine(RectangleTop);
                   // sw.WriteLine(" ");
               }
            }
        }

        public List<double> Liste2 = new List<double>();
        public void addDB2(double pos)
        {
            this.Liste2.Add(pos);
        }
        public void load()
        {
            string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(stream, false);

            string ligne;
            ligne = sr.ReadLine(); // title
            ligne = sr.ReadLine(); // number of elements
            double numberOfElement2 = Convert.ToDouble(ligne);
            numberOfElement = (int)numberOfElement2;

            ligne = sr.ReadLine(); // title

            while (ligne != null)
            {
                //Read the next line
                
                if (ligne != " ") 
                    ligne = sr.ReadLine();
                {   double coordonnee = 0;
                    if (ligne != "NAN")
                    {
                        coordonnee = Convert.ToDouble(ligne); 
                    }

                     // coordonnees
                    this.Liste2.Add(coordonnee);
                }
            }
            for(int i=10; i<= 2*numberOfElement; i=i+2) 
            {
                possItem(Liste2[i],Liste2[i+1]);
            }

            
            sr.Close();
        }

        #endregion
        #region Add item

        int numberOfElement = 1;
        public void addItem() 
        {
            numberOfElement += 1;
            UserControlJustine NewElement = new UserControlJustine();
            canvas.Children.Add(NewElement);
            
            Canvas.SetZIndex(NewElement, numberOfElement);
        }

        public void possItem(double possx, double possy)
        {
            numberOfElement += 1;
            UserControlJustine NewElement = new UserControlJustine();
            canvas.Children.Add(NewElement);
            Canvas.SetZIndex(NewElement, numberOfElement);
            Canvas.SetLeft(NewElement, possx);
            Canvas.SetTop(NewElement, possy);
        }
        #endregion
    }
}
