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

namespace DragDropSave
{
    /// <summary>
    /// Logique d'interaction pour UserControlJustine.xaml
    /// </summary>
    public partial class UserControlJustine : UserControl
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
    }
}
