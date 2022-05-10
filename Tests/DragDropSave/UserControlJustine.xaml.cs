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
using System.Windows.Controls.Primitives;

namespace DragDropSave
{
    /// <summary>
    /// Logique d'interaction pour UserControlJustine.xaml
    /// </summary>
    public partial class UserControlJustine : UserControl, INotifyPropertyChanged
    {
        public UserControlJustine()
        {
            InitializeComponent();
            this.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);
        }

        #region Added functions

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(UserControlJustine), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(UserControlJustine), new PropertyMetadata(Brushes.Black));

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

        private bool _isHighlighted { get; set; }
        public bool IsHighlighted
        {
            get
            {
                return _isHighlighted;
            }
            set
            {
                _isHighlighted = value;
                OnPropertyChanged("IsHighlighted");
            }
        }

        #endregion

        #region Elements functions

        private void _OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildHitTestVisible = false;
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
                IsChildHitTestVisible = true;
            }
        }

        #endregion

        #region Mouse enter and leave
        // Flag definition
        private bool _LeftFlag;
        public bool LeftFlag
        {
            get { return _LeftFlag; }
            set 
            { _LeftFlag = value;
                OnPropertyChanged("LeftFlag");
            }
        }
        private bool _MiddleFlag;
        public bool MiddleFlag
        {
            get { return _MiddleFlag; }
            set 
            { _MiddleFlag = value; 
                OnPropertyChanged("MiddleFlag");
            }
        }
        private bool _RightFlag;
        public bool RightFlag
        {
            get { return _RightFlag; }
            set 
            { _RightFlag = value; 
                OnPropertyChanged("RightFlag");
            }
        }

        // Mouse enter definition
        
        //private void LabelLeft_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    LeftFlag = true;
        //}

        //private void LabelMiddle_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    MiddleFlag = true;
        //}

        //private void LabelRight_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    RightFlag = true;
        //}

        //// Mouse leave definition

        //private void LabelLeft_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    LeftFlag = false;
        //}

        //private void LabelMiddle_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    MiddleFlag = false;
        //}

        //private void LabelRight_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    RightFlag = false;
        //}
        #endregion

        #region X and Y
        private double _x = 100;
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }
        private double _y = 100;
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }
        #endregion

        #region Mouse down and selection

        //private void LabelLeft_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.X = Canvas.GetLeft(LabelLeft);
        //    this.Y = Canvas.GetTop(LabelLeft);
        //}

        //private void LabelMiddle_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.X = Canvas.GetLeft(LabelMiddle);
        //    this.Y = Canvas.GetTop(LabelMiddle);
        //}

        //private void LabelRight_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.X = Canvas.GetLeft(LabelRight);
        //    this.Y = Canvas.GetTop(LabelRight);
        //}

        private bool _isSelected = false;
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("isSelected");
            }
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
                
                Canvas.SetZIndex(element, MaxZIndex());

                if (!canvas.Children.Contains(element) && !(element is Connecteur) && !(element is Ellipse))
                {
                    //canvas.Children.Add(element);
                }

                if (element is Connecteur element2)
                {
                    if (element2.ellipseDebut.Name == "debut")
                    {
                        double posx = Canvas.GetLeft(canvas) + canvas.Width / 2;
                        double posy = Canvas.GetTop(canvas);
                        if (!double.IsNaN(posx)) 
                        { 
                            Canvas.SetLeft(element2.ellipseDebut, posx);
                            element2.line.X1 = Canvas.GetLeft(element2.ellipseDebut) + element2.ellipseDebut.Width / 2;
                        }
                        if(!double.IsNaN(posy))
                        { 
                            Canvas.SetTop(element2.ellipseDebut, posy);
                            element2.line.Y1 = Canvas.GetTop(element2.ellipseDebut) + element2.ellipseDebut.Height / 2;
                        }
                    }
                    if (element2.ellipseFin.Name == "fin")
                    {
                        double posx = Canvas.GetLeft(canvas);
                        double posy = Canvas.GetTop(canvas) + canvas.Height / 2;
                        if (!double.IsNaN(posx))
                        {
                            Canvas.SetLeft(element2.ellipseFin, posx);
                            element2.line.X2 = Canvas.GetLeft(element2.ellipseFin) + element2.ellipseDebut.Width / 2;
                        }
                        if (!double.IsNaN(posy))
                        {
                            Canvas.SetTop(element2.ellipseFin, posy);
                            element2.line.Y2 = Canvas.GetTop(element2.ellipseFin) + element2.ellipseFin.Height / 2;
                        }
                            
                        
                    }
                }else
                {
                    dropPosition = e.GetPosition(canvas);
                    Canvas.SetLeft(element, dropPosition.X);
                    Canvas.SetTop(element, dropPosition.Y);
                }
            }
            
        }
        Point dropPosition;
        public Point getData()
        {
            return (dropPosition);
        }

        private int MaxZIndex()
        {
            int iMax = 0;
            foreach (UIElement element in canvas.Children)
            {
                int iZIndex = Canvas.GetZIndex(element);
                if (iZIndex > iMax)
                {
                    iMax = iZIndex;
                }
            }
            return iMax + 1;
        }
        #endregion


    }
}
