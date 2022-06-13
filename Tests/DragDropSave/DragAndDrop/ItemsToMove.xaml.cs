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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

using System.Windows.Controls.Primitives;


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

        Dictionary<int, Node> NodeDictionary = new Dictionary<int, Node>();
        Dictionary<int, Feature> FeatureDictionary = new Dictionary<int, Feature>();

        private bool _ok = false;
        private bool _go = false;
        private int estSurvolepos { get; set; }
        private int _numberOfElement = 0;
        private List<int> _areOnThisIn = new List<int>();
        private List<int> _areOnThisOut = new List<int>();

        #region Added functions

        public static readonly DependencyProperty IsChildHitTestVisibleProperty =
            DependencyProperty.Register("IsChildHitTestVisible", typeof(bool), typeof(ItemsToMove), new PropertyMetadata(true));

        public bool IsChildHitTestVisible
        {
            get { return (bool)GetValue(IsChildHitTestVisibleProperty); }
            set { SetValue(IsChildHitTestVisibleProperty, value); }
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
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            dropPosition = e.GetPosition(canvas);

            if (data is UIElement element)
            {
                if (element is Ellipse)
                {
                    Ellipse elementa = (Ellipse)element;
                    _ok = FeatureSurvole(elementa);
                }
                if (_ok)
                {
                    Ellipse elementa = (Ellipse)element;
                    Tuple<int, string> sortie;
                    sortie = whoIsTheConnector(elementa);
                    Node element2 = NodeDictionary[sortie.Item1];
                    Feature estSurvole = FeatureDictionary[estSurvolepos];

                    if (element == element2.ellipseDebut)
                    {
                        FeatureDictionary[estSurvolepos].OutputNodeId.Add(sortie.Item1);

                        double posx = Canvas.GetLeft(estSurvole) + estSurvole.Width - NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                        double posy = Canvas.GetTop(estSurvole) + estSurvole.Height / 2 - NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;
                        if (!double.IsNaN(posx))
                        {
                            Canvas.SetLeft(NodeDictionary[sortie.Item1].ellipseDebut, posx);
                            NodeDictionary[sortie.Item1].line.X1 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                        }
                        if (!double.IsNaN(posy))
                        {
                            Canvas.SetTop(NodeDictionary[sortie.Item1].ellipseDebut, posy);
                            NodeDictionary[sortie.Item1].line.Y1 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;
                        }
                    }
                    if (element == element2.ellipseFin)
                    {
                        FeatureDictionary[estSurvolepos].InputNodeId.Add(sortie.Item1);
                        double posx = Canvas.GetLeft(estSurvole) - NodeDictionary[sortie.Item1].ellipseFin.Width / 2;
                        double posy = Canvas.GetTop(estSurvole) + estSurvole.Height / 2 - NodeDictionary[sortie.Item1].ellipseFin.Height / 2;
                        if (!double.IsNaN(posx))
                        {
                            Canvas.SetLeft(NodeDictionary[sortie.Item1].ellipseFin, posx);
                            NodeDictionary[sortie.Item1].line.X2 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseFin) + NodeDictionary[sortie.Item1].ellipseFin.Width / 2;
                        }
                        if (!double.IsNaN(posy))
                        {
                            Canvas.SetTop(NodeDictionary[sortie.Item1].ellipseFin, posy);
                            NodeDictionary[sortie.Item1].line.Y2 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseFin) + NodeDictionary[sortie.Item1].ellipseFin.Height / 2;
                        }
                    }

                }
            }
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {
                
                dropPosition = e.GetPosition(canvas);

                int thisOne = new int();
                
                if (element is Line)
                {
                    Line elementa = (Line)element;
                    int sortie = whoIsTheConnectorLine(elementa);
                    thisOne = sortie;
                    double h = new double();
                    double w = new double();

                    h = Math.Abs(NodeDictionary[sortie].line.Y2 - NodeDictionary[sortie].line.Y1);
                    Canvas.SetTop(NodeDictionary[sortie].ellipseDebut, dropPosition.Y - h / 2 - NodeDictionary[sortie].ellipseDebut.Height / 2);
                    Canvas.SetTop(NodeDictionary[sortie].ellipseFin, dropPosition.Y + h / 2 - NodeDictionary[sortie].ellipseFin.Height / 2);

                    w = Math.Abs(NodeDictionary[sortie].line.X2 - NodeDictionary[sortie].line.X1);
                    Canvas.SetLeft(NodeDictionary[sortie].ellipseDebut, dropPosition.X - w / 2 - NodeDictionary[sortie].ellipseDebut.Width / 2);
                    Canvas.SetLeft(NodeDictionary[sortie].ellipseFin, dropPosition.X + w / 2 - NodeDictionary[sortie].ellipseFin.Width / 2);

                    _go = true;
}
                if (element is Node)
                {
                    Node elementa = (Node)element;
                    int sortie = new int();
                    for (int i = 0; i < NodeDictionary.Count; i++)
                    {
                        if (elementa == NodeDictionary[i])
                        {
                            sortie = i;
                        }
                    }

                    double h = new double();
                    double w = new double();

                    h = Math.Abs(NodeDictionary[sortie].line.Y2 - NodeDictionary[sortie].line.Y1);
                    Canvas.SetTop(NodeDictionary[sortie].ellipseDebut, dropPosition.Y - h / 2 - NodeDictionary[sortie].ellipseDebut.Height / 2);
                    Canvas.SetTop(NodeDictionary[sortie].ellipseFin, dropPosition.Y + h / 2 - NodeDictionary[sortie].ellipseFin.Height / 2);

                    w = Math.Abs(NodeDictionary[sortie].line.X2 - NodeDictionary[sortie].line.X1);
                    Canvas.SetLeft(NodeDictionary[sortie].ellipseDebut, dropPosition.X - w / 2 - NodeDictionary[sortie].ellipseDebut.Width / 2);
                    Canvas.SetLeft(NodeDictionary[sortie].ellipseFin, dropPosition.X + w / 2 - NodeDictionary[sortie].ellipseFin.Width / 2);

                    NodeDictionary[sortie].line.X1 = Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Width / 2;
                    NodeDictionary[sortie].line.Y1 = Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Height / 2;
                    NodeDictionary[sortie].line.X2 = Canvas.GetLeft(NodeDictionary[sortie].ellipseFin) + NodeDictionary[sortie].ellipseFin.Width / 2;
                    NodeDictionary[sortie].line.Y2 = Canvas.GetTop(NodeDictionary[sortie].ellipseFin) + NodeDictionary[sortie].ellipseFin.Height / 2;
                }
                if (!(element is Line) && !(element is Node))
                {

                    double w = element.RenderSize.Width;
                    double h = element.RenderSize.Height;

                    if (double.IsNaN(h)) h = 0;
                    if (double.IsNaN(w)) w = 0;

                    Canvas.SetLeft(element, dropPosition.X - (w / 2));
                    Canvas.SetTop(element, dropPosition.Y - (h / 2));

                    if (element is Ellipse)
                    {
                        Tuple<int, string> sortie = whoIsTheConnector((Ellipse)element);

                        double possx1 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut);
                        double possy1 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut);
                        double possx2 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseFin);
                        double possy2 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseFin);
                    }

                }

                if (!canvas.Children.Contains(element))
                {

                    if (element is Node)
                    {
                        Node element1 = (Node)element;

                        canvas.Children.Add(NodeDictionary[whoIsTheConnectorLine(element1.line)].line);
                        canvas.Children.Add(NodeDictionary[whoIsTheConnectorLine(element1.line)].ellipseDebut);
                        canvas.Children.Add(NodeDictionary[whoIsTheConnectorLine(element1.line)].ellipseFin);
                    }
                    else
                    {
                        canvas.Children.Add(element);
                    }
                }

                Canvas.SetZIndex(element, MaxZIndex());

                if (element is Line)
                {
                    Node element1 = NodeDictionary[whoIsTheConnectorLine((Line)element)];
                    Canvas.SetZIndex(NodeDictionary[whoIsTheConnectorLine(element1.line)].ellipseDebut, MaxZIndex());
                    Canvas.SetZIndex(NodeDictionary[whoIsTheConnectorLine(element1.line)].ellipseFin, MaxZIndex());
                }

                if (_go)
                {
                    int sortie = thisOne;
                    NodeDictionary[sortie].line.X1 = Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Width / 2;
                    NodeDictionary[sortie].line.Y1 = Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Height / 2;
                    NodeDictionary[sortie].line.X2 = Canvas.GetLeft(NodeDictionary[sortie].ellipseFin) + NodeDictionary[sortie].ellipseFin.Width / 2;
                    NodeDictionary[sortie].line.Y2 = Canvas.GetTop(NodeDictionary[sortie].ellipseFin) + NodeDictionary[sortie].ellipseFin.Height / 2;
                    double possx1 = Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut);
                    double possy1 = Canvas.GetTop(NodeDictionary[sortie].ellipseDebut);
                    double possx2 = Canvas.GetLeft(NodeDictionary[sortie].ellipseFin);
                    double possy2 = Canvas.GetTop(NodeDictionary[sortie].ellipseFin);
                }

                if (element is Ellipse)
                {
                    Ellipse elementa = (Ellipse)element;
                    Tuple<int, string> sortie;
                    sortie = whoIsTheConnector(elementa);
                    NodeDictionary[sortie.Item1].line.X1 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                    NodeDictionary[sortie.Item1].line.Y1 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;
                    NodeDictionary[sortie.Item1].line.X2 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseFin) + NodeDictionary[sortie.Item1].ellipseFin.Width / 2;
                    NodeDictionary[sortie.Item1].line.Y2 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseFin) + NodeDictionary[sortie.Item1].ellipseFin.Height / 2;
                }

                if (element is Feature)
                {
                    Feature elementa = (Feature)element;
                    int nbin = elementa.InputNodeId.Count;
                    int nbout = elementa.OutputNodeId.Count;

                    if (nbin > 0)
                    {
                        foreach (int i in elementa.InputNodeId)
                        {
                            Node element2 = NodeDictionary[i];


                            double posx = Canvas.GetLeft(elementa) - NodeDictionary[i].ellipseFin.Width / 2;
                            double posy = Canvas.GetTop(elementa) + elementa.Height / 2 - NodeDictionary[i].ellipseFin.Height / 2;
                            if (!double.IsNaN(posx))
                            {
                                Canvas.SetLeft(NodeDictionary[i].ellipseFin, posx);
                                NodeDictionary[i].line.X2 = Canvas.GetLeft(NodeDictionary[i].ellipseFin) + NodeDictionary[i].ellipseFin.Width / 2;
                            }
                            if (!double.IsNaN(posy))
                            {
                                Canvas.SetTop(NodeDictionary[i].ellipseFin, posy);
                                NodeDictionary[i].line.Y2 = Canvas.GetTop(NodeDictionary[i].ellipseFin) + NodeDictionary[i].ellipseFin.Height / 2;
                            }
                        }
                    }

                    if (nbout > 0)
                    {
                        foreach (int i in elementa.OutputNodeId)
                        {
                            Node element2 = NodeDictionary[i];

                            double posx = Canvas.GetLeft(elementa) + elementa.Width - NodeDictionary[i].ellipseDebut.Width / 2;
                            double posy = Canvas.GetTop(elementa) + elementa.Height / 2 - NodeDictionary[i].ellipseDebut.Height / 2;
                            if (!double.IsNaN(posx))
                            {
                                Canvas.SetLeft(NodeDictionary[i].ellipseDebut, posx);
                                NodeDictionary[i].line.X1 = Canvas.GetLeft(NodeDictionary[i].ellipseDebut) + NodeDictionary[i].ellipseDebut.Width / 2;
                            }
                            if (!double.IsNaN(posy))
                            {
                                Canvas.SetTop(NodeDictionary[i].ellipseDebut, posy);
                                NodeDictionary[i].line.Y1 = Canvas.GetTop(NodeDictionary[i].ellipseDebut) + NodeDictionary[i].ellipseDebut.Height / 2;
                            }

                        }
                    }
                    

                }
                if (element is Ellipse && FeatureDictionary.Count > 0) // vider les connecteurs des listes s'il ne sont plus sur le node
                {
                    for (int j = 0; j < FeatureDictionary.Count; j++)
                    {
                        IsOnThis(FeatureDictionary[j]);
                        bool exist;
                        bool exist2;
                        for (int i = 0; i < FeatureDictionary[j].InputNodeId.Count; i++)
                               {
                                exist = false;
                                foreach (int vin in _areOnThisIn)
                                {
                                    if (vin == FeatureDictionary[j].InputNodeId[i])
                                    {
                                        exist = true;
                                    }
                                }
                                if (!exist)
                                {
                                    FeatureDictionary[j].InputNodeId.Remove(FeatureDictionary[j].InputNodeId[i]);
                                }
                            }
                            for (int i = 0; i < FeatureDictionary[j].OutputNodeId.Count; i++)
                            {
                                exist2 = false;
                                foreach (int vin in _areOnThisOut)
                                {
                                    if (vin == FeatureDictionary[j].OutputNodeId[i])
                                    {
                                        exist2 = true;
                                    }
                                }
                                if (!exist2)
                                {
                                    FeatureDictionary[j].OutputNodeId.Remove(FeatureDictionary[j].OutputNodeId[i]);
                                }
                            }
                    }
                }
                _ok = false;
                _go = false;
            }
        }

        #endregion

        #region Node finder & Zindex

        public Tuple<int, string> whoIsTheConnector(Ellipse ellipse)
        {
            Tuple<int, string> sortie = new Tuple<int, string>(0, "Not a connector");
            for (int i = 0; i < NodeDictionary.Count; i++)
            {
                if (ellipse == NodeDictionary[i].ellipseDebut)
                {
                    sortie = new Tuple<int, string>(i, "debut");
                }
                if (ellipse == NodeDictionary[i].ellipseFin)
                {
                    sortie = new Tuple<int, string>(i, "fin");
                }
            }
            return (sortie);
        }

        public int whoIsTheConnectorLine(Line line)
        {
            int sortie = new int();
            for (int i = 0; i < NodeDictionary.Count; i++)
            {
                if (line == NodeDictionary[i].line)
                {
                    sortie = i;
                }
            }
            return (sortie);
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

        #region Add & clear

        public void AddFeature()
        {
            _numberOfElement += 1;
            Feature NewElement = new Feature();

            if (FeatureDictionary.Count > 0)
                FeatureDictionary.Add(FeatureDictionary.Keys.Max() + 1, NewElement);
            else
                FeatureDictionary.Add(0, NewElement);

            canvas.Children.Add(NewElement);
            Canvas.SetZIndex(NewElement, _numberOfElement);
        }

        public void AddNode()
        {
            _numberOfElement += 1;
            Node NewElement = new Node(1); // (new Node.PointD(10, 10), new Node.PointD(50, 50));
            if (NodeDictionary.Count > 0)
                NodeDictionary.Add(NodeDictionary.Keys.Max() + 1, NewElement);
            else
                NodeDictionary.Add(0, NewElement);

            canvas.Children.Add(NewElement.line);
            canvas.Children.Add(NewElement.ellipseDebut);
            canvas.Children.Add(NewElement.ellipseFin);

            Canvas.SetZIndex(NewElement, _numberOfElement);
        }

        public void clearAll()
        {
            FeatureDictionary.Clear();
            NodeDictionary.Clear();
            canvas.Children.Clear();

            _numberOfElement = 0;
        }

        #endregion

        #region FeatureSurvole & IsOnThis

        public bool FeatureSurvole(Ellipse link)
        {

            Point hg = new Point(Canvas.GetLeft(link), Canvas.GetTop(link));
            Point hd = new Point(hg.X + link.Width, hg.Y);
            Point bg = new Point(hg.X, hg.Y + link.Height);
            Point bd = new Point(hg.X + link.Width, hg.Y + link.Height);

            for (int i = 0; i < FeatureDictionary.Count; i++)
            {
                Feature features = FeatureDictionary[i];
                Point hgf = new Point(Canvas.GetLeft(features), Canvas.GetTop(features));
                Point hdf = new Point(hgf.X + features.Width, hgf.Y);
                Point bgf = new Point(hgf.X, hgf.Y + features.Height);
                Point bdf = new Point(hgf.X + features.Width, hgf.Y + features.Height);

                bool left = hgf.X < hg.X;
                bool up = hgf.Y < hg.Y;
                bool right = hdf.X > hd.X;
                bool down = bgf.Y > bg.Y;

                if (left && up && right && down)
                {
                    estSurvolepos = i;
                    return (true);
                }
                else
                {
                    estSurvolepos = 0;
                }
            }
            return (false);
        }

        public void IsOnThis(Feature feature)
        {
            _areOnThisIn.Clear();
            _areOnThisOut.Clear();

            for (int i = 0; i < NodeDictionary.Count; i++)
            {
                double w = NodeDictionary[i].ellipseDebut.Width + NodeDictionary[i].ellipseDebut.Width / 2;
                double h = NodeDictionary[i].ellipseDebut.Height + NodeDictionary[i].ellipseDebut.Height / 2;

                Point hg = new Point(Canvas.GetLeft(feature) - w / 2, Canvas.GetTop(feature) - h / 2);
                Point hd = new Point(hg.X + feature.RenderSize.Width + w, hg.Y - h / 2);
                Point bg = new Point(hg.X - w / 2, hg.Y + feature.RenderSize.Height + h / 2);

                Point hgf = new Point(Canvas.GetLeft(NodeDictionary[i].ellipseDebut), Canvas.GetTop(NodeDictionary[i].ellipseDebut));
                Point hdf = new Point(hgf.X + NodeDictionary[i].ellipseDebut.Width, hgf.Y);
                Point bgf = new Point(hgf.X, hgf.Y + NodeDictionary[i].ellipseDebut.Height);

                bool left = hgf.X > hg.X;
                bool up = hgf.Y > hg.Y;
                bool right = hdf.X < hd.X;
                bool down = bgf.Y < bg.Y;

                if (left && up && right && down)
                {
                    _areOnThisOut.Add(i);
                }

                double w2 = NodeDictionary[i].ellipseFin.Width + NodeDictionary[i].ellipseFin.Width / 2;
                double h2 = NodeDictionary[i].ellipseFin.Height + NodeDictionary[i].ellipseFin.Height / 2;

                Point hg2 = new Point(Canvas.GetLeft(feature) - w2 / 2, Canvas.GetTop(feature) - h2 / 2);
                Point hd2 = new Point(hg2.X + feature.RenderSize.Width + w2 / 2, hg2.Y - h2 / 2);
                Point bg2 = new Point(hg2.X - w2 / 2, hg2.Y + feature.RenderSize.Height + h2 / 2);

                Point hgf2 = new Point(Canvas.GetLeft(NodeDictionary[i].ellipseFin), Canvas.GetTop(NodeDictionary[i].ellipseFin));
                Point hdf2 = new Point(hgf2.X + NodeDictionary[i].ellipseFin.Width, hgf2.Y);
                Point bgf2 = new Point(hgf2.X, hgf2.Y + NodeDictionary[i].ellipseFin.Height);

                bool left2 = hgf2.X > hg2.X;
                bool up2 = hgf2.Y > hg2.Y;
                bool right2 = hdf2.X < hd2.X;
                bool down2 = bgf2.Y < bg2.Y;

                if (left2 && up2 && right2 && down2)
                {
                    _areOnThisIn.Add(i);
                }
            }
        }

        #endregion   
    }
}
