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
    /// Logique d'interaction pour Connecteur.xaml
    /// </summary>
    public partial class Connecteur : UserControl, INotifyPropertyChanged
    {
        PointD pt1;
        PointD pt2;

        private Ellipse _ellipseDebut;
        public Ellipse ellipseDebut
        {
            get {return _ellipseDebut; }
            set
            {
                _ellipseDebut = value;
                OnPropertyChanged("_ellipseDebut");
            }
        }
        private Ellipse _ellipseFin;
        public Ellipse ellipseFin
        {
            get { return _ellipseFin; }
            set
            {
                _ellipseFin = value;
                OnPropertyChanged("_ellipseFin");
            }
        }

        private Line _line;
        public Line line
        {
            get { return _line; }
            set
            {
                _line = value;
                OnPropertyChanged("_line");
}
        }

        public Connecteur()
        {
            InitializeComponent();
            this.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);
            Line line = new Line();
            line.StrokeThickness = 2;
            line.Stroke = System.Windows.Media.Brushes.Black;
        }

        public Connecteur(PointD p1, PointD p2)
        {
            InitializeComponent();

            pt1 = p1;
            pt2 = p2;

            line = new Line();
            line.StrokeThickness = 4;
            line.Stroke = System.Windows.Media.Brushes.Gray;
            line.X1 = pt1.X;
            line.Y1 = pt1.Y;
            line.X2 = pt2.X;
            line.Y2 = pt2.Y;
            line.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            ellipseDebut = new Ellipse();
            ellipseDebut.Name = "debut";
            ellipseDebut.Width = 20;
            ellipseDebut.Height = 20;
            ellipseDebut.Fill = new SolidColorBrush(Colors.Red);
            ellipseDebut.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            ellipseFin = new Ellipse();
            ellipseFin.Name = "fin";
            ellipseFin.Width = 20;
            ellipseFin.Height = 20;
            ellipseFin.Fill = new SolidColorBrush(Colors.Blue);
            ellipseFin.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);

            canvas.Children.Add(ellipseDebut);
            canvas.Children.Add(ellipseFin);
            canvas.Children.Add(line);
            Canvas.SetLeft(ellipseDebut, pt1.X - ellipseDebut.Width / 2);
            Canvas.SetTop(ellipseDebut, pt1.Y - ellipseDebut.Height / 2);
            Canvas.SetLeft(ellipseFin, pt2.X - ellipseFin.Width / 2);
            Canvas.SetTop(ellipseFin, pt2.Y - ellipseFin.Height / 2);
        }
        #region Added functions

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(Connecteur), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(Connecteur), new PropertyMetadata(Brushes.Black));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
                    line.X1 = Canvas.GetLeft(ellipseDebut) + ellipseDebut.Width / 2;
                    line.Y1 = Canvas.GetTop(ellipseDebut) + ellipseDebut.Height / 2;
                }

                if (sender == ellipseFin)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(ellipseFin, new DataObject(DataFormats.Serializable, ellipseFin), DragDropEffects.Move);
                    IsChildHitTestVisible = true;
                    line.X2 = Canvas.GetLeft(ellipseFin) + ellipseDebut.Width / 2;
                    line.Y2 = Canvas.GetTop(ellipseFin) + ellipseFin.Height / 2;
                }

                if (sender == line)
                {
                    IsChildHitTestVisible = false;
                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
                    IsChildHitTestVisible = true;

                }

            }

        }

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

        Point dropPosition;
        public Point getData()
        {
            return (dropPosition);
        }
        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);
                if (!canvas.Children.Contains(element) && !(element is UserControlJustine) && !(element is Connecteur))
                {
                    canvas.Children.Add(element);
                }

            }
        }


        #endregion

        #region Start and end
        private UserControlJustine _start;
        public UserControlJustine Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }

        private UserControlJustine _end;
        public UserControlJustine End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }
        #endregion

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

    
}

