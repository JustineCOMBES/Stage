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
using System.ComponentModel;

namespace DragDropSave
{
    /// <summary>
    /// Logique d'interaction pour Node.xaml
    /// </summary>

    public partial class Node : UserControl
    {
        #region declaration graphique

        PointD pt1;
        PointD pt2;

        public Ellipse ellipseDebut { get; set; }

        private Ellipse ellipseFin { get; set; }
        private Line line { get; set; }

        public List<Tuple<Ellipse, Line>> EllipseInputAndLineList = new List<Tuple<Ellipse, Line>>();
        #endregion

        public int _id;

        public double PositionX1;
        public double PositionY1;
        public double PositionX2;
        public double PositionY2;

        public PointD PositionEllipseOutput;
        public PointD PositionEllipseInput;

        public Node(int id)
        {
            _id = id;

            InitializeComponent();

            this.PositionEllipseOutput = new PointD(20, 20);
            this.PositionEllipseInput = new PointD(50, 50);

            #region Graphique
            pt1 = PositionEllipseOutput;
            pt2 = PositionEllipseInput;

            line = new Line
            {
                Name = "line",
                StrokeThickness = 10,
                Stroke = System.Windows.Media.Brushes.Gray,
                X1 = pt1.X,
                Y1 = pt1.Y,
                X2 = pt2.X,
                Y2 = pt2.Y
            };
            line.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            ellipseDebut = new Ellipse
            {
                Name = "debut",
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(Colors.Red)
            };
            ellipseDebut.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            ellipseFin = new Ellipse
            {
                Name = "fin",
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(Colors.Blue)
            };
            ellipseFin.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            var toAdd = new Tuple<Ellipse, Line>(ellipseFin, line);
            EllipseInputAndLineList.Add(toAdd);

            Canvas.SetLeft(ellipseDebut, pt1.X - ellipseDebut.Width / 2);
            Canvas.SetTop(ellipseDebut, pt1.Y - ellipseDebut.Height / 2);
            Canvas.SetLeft(ellipseFin, pt2.X - ellipseFin.Width / 2);
            Canvas.SetTop(ellipseFin, pt2.Y - ellipseFin.Height / 2);

            #endregion
        }

        public void MultiOutAdd()
        {
            // Pour ajouter une sortie liée à la même entrée
            Ellipse ellipseToAdd = new Ellipse
            {
                Name = "fin",
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(Colors.Blue)
            };
            ellipseToAdd.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            Line lineToAdd = new Line
            {
                Name = "line",
                StrokeThickness = 10,
                Stroke = System.Windows.Media.Brushes.Gray,
                X1 = pt1.X,
                Y1 = pt1.Y,
                X2 = pt2.X,
                Y2 = pt2.Y
            };
            lineToAdd.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            var toAdd = new Tuple<Ellipse, Line>(ellipseToAdd, lineToAdd);
            EllipseInputAndLineList.Add(toAdd);
        }

        #region Added functions

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(Node), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }
        #endregion

        #region Elements functions

        private void _OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                if (sender == ellipseDebut)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(ellipseDebut, new DataObject(DataFormats.Serializable, ellipseDebut), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                }

                if (sender == ellipseFin)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(ellipseFin, new DataObject(DataFormats.Serializable, ellipseFin), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                }

                if (sender == line)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(line, new DataObject(DataFormats.Serializable, line), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                }

                if (sender is Ellipse name)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(name, new DataObject(DataFormats.Serializable, name), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                }
                if (sender is Line name2)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(name2, new DataObject(DataFormats.Serializable, name2), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                }
            }

        }

        #endregion
    }


    #region Point double
    public class PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    #endregion
}
