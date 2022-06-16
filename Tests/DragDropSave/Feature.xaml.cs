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
    /// Logique d'interaction pour Feature.xaml
    /// </summary>
    public partial class Feature : UserControl, INotifyPropertyChanged
    {
        private List<int> _InputNodeId = new List<int>();
        public List<int> InputNodeId
        {
            get { return _InputNodeId; }
            set
            {
                _InputNodeId = value;
                OnPropertyChanged("InputNodeId");
            }
        }

        private List<int> _OutputNodeId = new List<int>();
        public List<int> OutputNodeId
        {
            get { return _OutputNodeId; }
            set
            {
                _OutputNodeId = value;
                OnPropertyChanged("InputNodeId");
            }
        }
        public int id;
        public Feature()
        {
            InitializeComponent();
            this.MouseMove += new System.Windows.Input.MouseEventHandler(_OnMouseMove);
            this.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(_OnRightClick);
        }

        private void _OnRightClick(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = new ContextMenu();

            cm.Items.Clear();

            MenuItem Remove = new MenuItem();
            Remove.Header = "Remove";
            cm.Items.Add(Remove);

            ComboBox Option = new ComboBox();
            Option.Items.Add(1);
            Option.Items.Add(2);
            Option.Items.Add(3);
            Option.Items.Add(4);
            Option.Items.Add(5);
            Option.SelectedIndex = 0;
            cm.Items.Add(Option);

            this.ContextMenu = cm;

            Remove.Click += OnRemove;
            Option.SelectionChanged += OnOption;
        }

        public event EventHandler<EventArgs> OnRemoveEvent;
        public virtual void OnRemove(object sender, EventArgs e)
        {
            var handler = OnRemoveEvent;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> OnOptionEvent;
        public virtual void OnOption(object sender, EventArgs e)
        {
            var handler = OnOptionEvent;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #region Added functions

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(Feature), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(Feature), new PropertyMetadata(Brushes.Black));

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
    }
}
