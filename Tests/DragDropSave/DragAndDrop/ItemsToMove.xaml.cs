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
        Dictionary<int, TextBlock> TextLabelDictionary = new Dictionary<int, TextBlock>();

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
                if (_ok) // si un ellement est survole
                {
                    Ellipse elementa = (Ellipse)element;
                    Tuple<int, string> sortie;
                    sortie = whoIsTheConnector(elementa);
                    Node element2 = NodeDictionary[sortie.Item1];
                    Feature estSurvole = FeatureDictionary[estSurvolepos];

                    if (element == element2.ellipseDebut)
                    {
                        FeatureDictionary[estSurvolepos].OutputNodeId.Add(sortie.Item1);

                        foreach (var tf in NodeDictionary[sortie.Item1].EllipseInputAndLineList)
                        {
                            double posx = Canvas.GetLeft(estSurvole) + estSurvole.Width - NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                            double posy = Canvas.GetTop(estSurvole) + estSurvole.Height / 2 - NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;
                            if (!double.IsNaN(posx))
                            {
                                Canvas.SetLeft(NodeDictionary[sortie.Item1].ellipseDebut, posx);
                                tf.Item2.X1 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                            }
                            if (!double.IsNaN(posy))
                            {
                                Canvas.SetTop(NodeDictionary[sortie.Item1].ellipseDebut, posy);
                                tf.Item2.Y1 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;
                            }
                        }
                    }
                    foreach(var tupleFin in element2.EllipseInputAndLineList) {

                        var ellipseFin = tupleFin.Item1;

                        if (element == ellipseFin)
                        {
                            int key = 1000 * element2._id + int.Parse(tupleFin.Item1.Tag.ToString());

                            FeatureDictionary[estSurvolepos].InputNodeId.Add(key);

                            double posx = Canvas.GetLeft(estSurvole) - ellipseFin.Width / 2;
                            double posy = Canvas.GetTop(estSurvole) + estSurvole.Height / 2 - ellipseFin.Height / 2;
                            if (!double.IsNaN(posx))
                            {
                                Canvas.SetLeft(tupleFin.Item1, posx);
                                tupleFin.Item2.X2 = Canvas.GetLeft(tupleFin.Item1) + ellipseFin.Width / 2;
                            }
                            if (!double.IsNaN(posy))
                            {
                                Canvas.SetTop(tupleFin.Item1, posy);
                                tupleFin.Item2.Y2 = Canvas.GetTop(tupleFin.Item1) + ellipseFin.Height / 2;
                            }
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

                dropPosition = e.GetPosition(canvas); // pour connaitre la position de la souris

               int thisOne = new int(); // utile dans _go

                if (element is Line) // deplacer chaque ligne indépendemment !! -> pas utile
                {
                    Line elementa = (Line)element;
                    int sortie = whoIsTheConnectorLine(elementa); // on prend le connecteur a qui appartirnt la ligne
                    thisOne = sortie;

                    foreach (var tupleFin in NodeDictionary[sortie].EllipseInputAndLineList)
                    {
                        double h = Math.Abs(tupleFin.Item2.Y2 - tupleFin.Item2.Y1);
                        double w = Math.Abs(tupleFin.Item2.X2 - tupleFin.Item2.X1);

                        Canvas.SetTop(NodeDictionary[sortie].ellipseDebut, dropPosition.Y - h / 2 - NodeDictionary[sortie].ellipseDebut.Height / 2);
                        Canvas.SetLeft(NodeDictionary[sortie].ellipseDebut, dropPosition.X - w / 2 - NodeDictionary[sortie].ellipseDebut.Width / 2);

                        if (elementa == tupleFin.Item2)
                        {
                            Canvas.SetTop(tupleFin.Item1, dropPosition.Y + h / 2 - tupleFin.Item1.RenderSize.Height / 2);
                            Canvas.SetLeft(tupleFin.Item1, dropPosition.X + w / 2 - tupleFin.Item1.RenderSize.Width / 2);
                        }
                        else // a explorer
                        {
                            Canvas.SetTop(tupleFin.Item1, dropPosition.Y + h / 2 - tupleFin.Item1.RenderSize.Height / 2);
                            Canvas.SetLeft(tupleFin.Item1, dropPosition.X + w / 2 - tupleFin.Item1.RenderSize.Width / 2);
                        }
                        tupleFin.Item2.X1 = Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Width / 2;
                        tupleFin.Item2.Y1 = Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Height / 2;
                        tupleFin.Item2.X2 = Canvas.GetLeft(tupleFin.Item1) + tupleFin.Item1.RenderSize.Width / 2;
                        tupleFin.Item2.Y2 = Canvas.GetTop(tupleFin.Item1) + tupleFin.Item1.RenderSize.Height / 2;

                        Canvas.SetLeft(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie]._id], Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + (Canvas.GetLeft(tupleFin.Item1) - Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + tupleFin.Item1.Width / 2 - NodeDictionary[sortie].ellipseDebut.Width / 2) / 2 + TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 100 * NodeDictionary[sortie]._id].RenderSize.Width / 2);
                        Canvas.SetTop(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie]._id], Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + (Canvas.GetTop(tupleFin.Item1) - Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + tupleFin.Item1.Height / 2 - NodeDictionary[sortie].ellipseDebut.Height / 2) / 2);

                    }

                    _go = true;
                }
                if (!(element is Line) && !(element is Node))
                {

                    double w = element.RenderSize.Width;
                    double h = element.RenderSize.Height;

                    if (double.IsNaN(h)) h = 0;
                    if (double.IsNaN(w)) w = 0;

                    Canvas.SetLeft(element, dropPosition.X - (w / 2));
                    Canvas.SetTop(element, dropPosition.Y - (h / 2));
                }

                if (!canvas.Children.Contains(element))
                {

                    if (element is Node)
                    {
                        Node element1 = (Node)element;

                        canvas.Children.Add(NodeDictionary[whoIsTheConnectorLine(element1.EllipseInputAndLineList[0].Item2)].ellipseDebut);

                        foreach (var tupleFin in NodeDictionary[whoIsTheConnectorLine(element1.EllipseInputAndLineList[0].Item2)].EllipseInputAndLineList)
                        {
                            canvas.Children.Add(tupleFin.Item1);
                            canvas.Children.Add(tupleFin.Item2);
                        }
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

                    Canvas.SetZIndex(NodeDictionary[whoIsTheConnectorLine(element1.EllipseInputAndLineList[0].Item2)].ellipseDebut, MaxZIndex());

                    foreach (var tupleFin in NodeDictionary[whoIsTheConnectorLine(element1.EllipseInputAndLineList[0].Item2)].EllipseInputAndLineList)
                    {
                        Canvas.SetZIndex(tupleFin.Item1, MaxZIndex());
                    }
                }

                if (_go)
                {
                    int sortie = thisOne;

                    foreach (var tupleFin in NodeDictionary[sortie].EllipseInputAndLineList)
                    {
                        tupleFin.Item2.X1 = Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Width / 2;
                        tupleFin.Item2.Y1 = Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + NodeDictionary[sortie].ellipseDebut.Height / 2;

                        tupleFin.Item2.X2 = Canvas.GetLeft(tupleFin.Item1) + tupleFin.Item1.RenderSize.Width / 2;
                        tupleFin.Item2.Y2 = Canvas.GetTop(tupleFin.Item1) + tupleFin.Item1.RenderSize.Height / 2;

                        Canvas.SetLeft(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie]._id], Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + (Canvas.GetLeft(tupleFin.Item1) - Canvas.GetLeft(NodeDictionary[sortie].ellipseDebut) + tupleFin.Item1.Width / 2 - NodeDictionary[sortie].ellipseDebut.Width / 2) / 2 + TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 100 * NodeDictionary[sortie]._id].RenderSize.Width / 2);
                        Canvas.SetTop(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie]._id], Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + (Canvas.GetTop(tupleFin.Item1) - Canvas.GetTop(NodeDictionary[sortie].ellipseDebut) + tupleFin.Item1.Height / 2 - NodeDictionary[sortie].ellipseDebut.Height / 2) / 2 );

                    }
                }

                if (element is Ellipse)
                {
                    Ellipse elementa = (Ellipse)element;
                    Tuple<int, string> sortie;
                    sortie = whoIsTheConnector(elementa);
                    
                    foreach (var tupleFin in NodeDictionary[sortie.Item1].EllipseInputAndLineList)
                    {
                        tupleFin.Item2.X1 = Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Width / 2;
                        tupleFin.Item2.Y1 = Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + NodeDictionary[sortie.Item1].ellipseDebut.Height / 2;

                        tupleFin.Item2.X2 = Canvas.GetLeft(tupleFin.Item1) + tupleFin.Item1.Width / 2;
                        tupleFin.Item2.Y2 = Canvas.GetTop(tupleFin.Item1) + tupleFin.Item1.Height / 2;

                        Canvas.SetLeft(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie.Item1]._id], Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + (Canvas.GetLeft(tupleFin.Item1) - Canvas.GetLeft(NodeDictionary[sortie.Item1].ellipseDebut) + tupleFin.Item1.Width / 2 - NodeDictionary[sortie.Item1].ellipseDebut.Width / 2) / 2 + TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 100 * NodeDictionary[sortie.Item1]._id].RenderSize.Width / 2);
                        Canvas.SetTop(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[sortie.Item1]._id], Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + (Canvas.GetTop(tupleFin.Item1) - Canvas.GetTop(NodeDictionary[sortie.Item1].ellipseDebut) + tupleFin.Item1.Height / 2 - NodeDictionary[sortie.Item1].ellipseDebut.Height / 2) / 2);

                    }
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
                            Tuple<int, int> answer = KeyReader(i);
                            int nodeId = answer.Item1;
                            int inputTag = answer.Item2;

                            Tuple<Ellipse, Line> tupleFin = NodeDictionary[nodeId].EllipseInputAndLineList[inputTag];

                            double posx = Canvas.GetLeft(elementa) - tupleFin.Item1.Width / 2;
                            double posy = Canvas.GetTop(elementa) + elementa.Height / 2 - tupleFin.Item1.Height / 2;
                            if (!double.IsNaN(posx))
                            {
                                Canvas.SetLeft(tupleFin.Item1, posx);
                                tupleFin.Item2.X2 = Canvas.GetLeft(tupleFin.Item1) + tupleFin.Item1.Width / 2;
                            }
                            if (!double.IsNaN(posy))
                            {
                                Canvas.SetTop(tupleFin.Item1, posy);
                                tupleFin.Item2.Y2 = Canvas.GetTop(tupleFin.Item1) + tupleFin.Item1.Height / 2;
                            }
                            Canvas.SetLeft(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[nodeId]._id], Canvas.GetLeft(NodeDictionary[nodeId].ellipseDebut) + (Canvas.GetLeft(tupleFin.Item1) - Canvas.GetLeft(NodeDictionary[nodeId].ellipseDebut) + tupleFin.Item1.Width / 2 - NodeDictionary[nodeId].ellipseDebut.Width / 2) / 2 + TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 100 * NodeDictionary[nodeId]._id].RenderSize.Width / 2);
                            Canvas.SetTop(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[nodeId]._id], Canvas.GetTop(NodeDictionary[nodeId].ellipseDebut) + (Canvas.GetTop(tupleFin.Item1) - Canvas.GetTop(NodeDictionary[nodeId].ellipseDebut) + tupleFin.Item1.Height / 2 - NodeDictionary[nodeId].ellipseDebut.Height / 2) / 2);

                        }
                    }

                    if (nbout > 0)
                    {
                        foreach (int i in elementa.OutputNodeId)
                        {
                            Node element2 = NodeDictionary[i];

                            double posx = Canvas.GetLeft(elementa) + elementa.Width - NodeDictionary[i].ellipseDebut.Width / 2;
                            double posy = Canvas.GetTop(elementa) + elementa.Height / 2 - NodeDictionary[i].ellipseDebut.Height / 2;

                            foreach (var tupleFin in NodeDictionary[i].EllipseInputAndLineList)
                            {
                                if (!double.IsNaN(posx))
                                {
                                    Canvas.SetLeft(NodeDictionary[i].ellipseDebut, posx);
                                    tupleFin.Item2.X1 = Canvas.GetLeft(NodeDictionary[i].ellipseDebut) + NodeDictionary[i].ellipseDebut.Width / 2;
                                }
                                if (!double.IsNaN(posy))
                                {
                                    Canvas.SetTop(NodeDictionary[i].ellipseDebut, posy);
                                    tupleFin.Item2.Y1 = Canvas.GetTop(NodeDictionary[i].ellipseDebut) + NodeDictionary[i].ellipseDebut.Height / 2;
                                }
                                Canvas.SetLeft(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[i]._id], Canvas.GetLeft(NodeDictionary[i].ellipseDebut) + (Canvas.GetLeft(tupleFin.Item1) - Canvas.GetLeft(NodeDictionary[i].ellipseDebut) + tupleFin.Item1.Width / 2 - NodeDictionary[i].ellipseDebut.Width / 2) / 2 + TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 100 * NodeDictionary[i]._id].RenderSize.Width / 2);
                                Canvas.SetTop(TextLabelDictionary[int.Parse(tupleFin.Item2.Tag.ToString()) + 1000 * NodeDictionary[i]._id], Canvas.GetTop(NodeDictionary[i].ellipseDebut) + (Canvas.GetTop(tupleFin.Item1) - Canvas.GetTop(NodeDictionary[i].ellipseDebut) + tupleFin.Item1.Height / 2 - NodeDictionary[i].ellipseDebut.Height / 2) / 2);

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
                                Tuple<int, int> answer = KeyReader(vin);
                                int nodeId = answer.Item1;
                                int inputTag = answer.Item2;

                                Tuple<int, int> reference = KeyReader(FeatureDictionary[j].InputNodeId[i]);
                                int refNodeId = reference.Item1;
                                int refInputTag = reference.Item2;

                                if ((nodeId == refNodeId) && (inputTag == refInputTag))
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

  
        #region Node finder & Zindex + KeyReader

        public Tuple<int, string> whoIsTheConnector(Ellipse ellipse)
        {
            Tuple<int, string> sortie = new Tuple<int, string>(0, "Not a connector");
            for (int i = 0; i < NodeDictionary.Count; i++)
            {
                if (ellipse == NodeDictionary[i].ellipseDebut)
                {
                    sortie = new Tuple<int, string>(i, "debut");
                }
                foreach (var element in NodeDictionary[i].EllipseInputAndLineList)
                {
                    if (ellipse == element.Item1)
                    {
                        sortie = new Tuple<int, string>(i, "fin");
                    }
                }
            }
            return (sortie);
        }

        public int whoIsTheConnectorLine(Line line)
        {
            int sortie = new int();
            for (int i = 0; i < NodeDictionary.Count; i++)
            {
                foreach (var element in NodeDictionary[i].EllipseInputAndLineList)
                {
                    if (line == element.Item2)
                    {
                        sortie = i;
                    }
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

        private Tuple<int,int> KeyReader(int key)
        {
            /// Retourne l'ID du node en Item1 et le Tag de la Line en Item2
            
            int tag = key % 1000;
            int id = key / 1000;
            Tuple<int, int> answer = new Tuple<int, int>(id,tag);
            return answer;
        }

        #endregion

        #region Add & clear

        public void AddFeature()
        {
            _numberOfElement += 1;
            Feature NewElement = new Feature();



            if (FeatureDictionary.Count == 0)
                NewElement.id = 0;
            else
                NewElement.id = FeatureDictionary.Keys.Max() + 1;

          FeatureDictionary.Add(NewElement.id, NewElement);


            canvas.Children.Add(NewElement);
            Canvas.SetZIndex(NewElement, _numberOfElement);

            NewElement.OnRemoveEvent += RemoveFeature;
            NewElement.OnOptionEvent += OpenOptions;
        }

        private void RemoveFeature(object FeatureToRemove, EventArgs e)
        {
            _numberOfElement -= 1;
            canvas.Children.Remove((Feature)FeatureToRemove);
            FeatureDictionary.Remove(((Feature)FeatureToRemove).id);
        }

        private void OpenOptions(object Feature, EventArgs e)
        {

        }

        // ------------------------ Node

        public void AddNode()
        {
            
            _numberOfElement += 1;
            Node NewElement = new Node(NodeDictionary.Count);
            if (NodeDictionary.Count > 0)
                NodeDictionary.Add(NodeDictionary.Keys.Max() + 1, NewElement);
            else
                NodeDictionary.Add(0, NewElement);

            canvas.Children.Add(NewElement.EllipseInputAndLineList[0].Item2);
            canvas.Children.Add(NewElement.ellipseDebut);
            canvas.Children.Add(NewElement.EllipseInputAndLineList[0].Item1);

            Canvas.SetZIndex(NewElement, _numberOfElement);

            
        }

        public void AddMultiNode(int NumberOfInput)
        {
            if (NumberOfInput > 0)
            {
                _numberOfElement += 1;
                Node NewElement = new Node(NodeDictionary.Count);

                if (NodeDictionary.Count > 0) NodeDictionary.Add(NewElement._id, NewElement);
                else NodeDictionary.Add(0, NewElement);

                for (int i = 2; i <= NumberOfInput; i++)
                {
                    NewElement.MultiOutAdd();
                }

                canvas.Children.Add(NewElement.ellipseDebut);

                int pos = 50;
                int inc = pos;

                foreach (var element in NewElement.EllipseInputAndLineList)
                {
                    canvas.Children.Add(element.Item1);
                    canvas.Children.Add(element.Item2);

                    Canvas.SetLeft(element.Item1, 1000);
                    Canvas.SetTop(element.Item1, pos);
                    pos += inc;

                }

                Canvas.SetTop(NewElement.ellipseDebut, pos / 2);

                foreach (var element in NewElement.EllipseInputAndLineList)
                {
                    element.Item2.X1 = Canvas.GetLeft(NewElement.ellipseDebut) + NewElement.ellipseDebut.Width / 2;
                    element.Item2.Y1 = Canvas.GetTop(NewElement.ellipseDebut) + NewElement.ellipseDebut.Height / 2;

                    element.Item2.X2 = Canvas.GetLeft(element.Item1) + element.Item1.Width / 2;
                    element.Item2.Y2 = Canvas.GetTop(element.Item1) + element.Item1.Height / 2;

                    TextBlock IdTag = new TextBlock();
                    string msg = NewElement._id + "." + element.Item2.Tag;
                    IdTag.Text = msg;
                    canvas.Children.Add(IdTag);

                    int key = 1000 * NewElement._id + int.Parse(element.Item2.Tag.ToString());
                    TextLabelDictionary.Add(key, IdTag);

                    Canvas.SetLeft(IdTag, Canvas.GetLeft(NewElement.ellipseDebut) + (Canvas.GetLeft(element.Item1) - Canvas.GetLeft(NewElement.ellipseDebut) + element.Item1.Width / 2 - NewElement.ellipseDebut.Width / 2) / 2 + IdTag.RenderSize.Width/2);
                    Canvas.SetTop(IdTag, Canvas.GetTop(NewElement.ellipseDebut) + (Canvas.GetTop(element.Item1) - Canvas.GetTop(NewElement.ellipseDebut) + element.Item1.Height / 2 - NewElement.ellipseDebut.Height / 2) / 2 - (inc/10));

                    Canvas.SetZIndex(element.Item1, Canvas.GetZIndex(element.Item2) + 1);
                    Canvas.SetZIndex(NewElement.ellipseDebut, Canvas.GetZIndex(element.Item2) + 1);
                }
                Canvas.SetZIndex(NewElement, _numberOfElement);

                NewElement.OnAddOneEvent += AddBranch;
                NewElement.OnRemoveAllEvent += RemoveNode;
                NewElement.OnRemoveOneEvent += RemoveBranch;
            }
        }

        private void AddBranch(object nodeToChange, EventArgs e)
        {
            ((Node)nodeToChange).MultiOutAdd();
            var branch = ((Node)nodeToChange).EllipseInputAndLineList.Last();

            canvas.Children.Add(branch.Item1);
            canvas.Children.Add(branch.Item2);

            Canvas.SetLeft(branch.Item1, Canvas.GetLeft(((Node)nodeToChange).ellipseDebut) + 100);
            Canvas.SetTop(branch.Item1, Canvas.GetTop(((Node)nodeToChange).ellipseDebut) + (2 * ((Node)nodeToChange).ellipseDebut.Height));

            branch.Item2.X1 = Canvas.GetLeft(((Node)nodeToChange).ellipseDebut) + ((Node)nodeToChange).ellipseDebut.Width / 2;
            branch.Item2.Y1 = Canvas.GetTop(((Node)nodeToChange).ellipseDebut) + ((Node)nodeToChange).ellipseDebut.Height / 2;
            branch.Item2.X2 = Canvas.GetLeft(branch.Item1) + branch.Item1.Width / 2;
            branch.Item2.Y2 = Canvas.GetTop(branch.Item1) + branch.Item1.Height / 2;

            TextBlock IdTag = new TextBlock();
            string msg = ((Node)nodeToChange)._id + "." + branch.Item2.Tag;
            IdTag.Text = msg;
            canvas.Children.Add(IdTag);

            int key = 1000 * ((Node)nodeToChange)._id + int.Parse(branch.Item2.Tag.ToString());
            TextLabelDictionary.Add(key, IdTag);

            Canvas.SetLeft(IdTag, Canvas.GetLeft(((Node)nodeToChange).ellipseDebut) + (Canvas.GetLeft(branch.Item1) - Canvas.GetLeft(((Node)nodeToChange).ellipseDebut) + branch.Item1.Width / 2 - ((Node)nodeToChange).ellipseDebut.Width / 2) / 2 + IdTag.RenderSize.Width / 2);
            Canvas.SetTop(IdTag, Canvas.GetTop(((Node)nodeToChange).ellipseDebut) + (Canvas.GetTop(branch.Item1) - Canvas.GetTop(((Node)nodeToChange).ellipseDebut) + branch.Item1.Height / 2 - ((Node)nodeToChange).ellipseDebut.Height / 2) / 2);

            Canvas.SetZIndex(branch.Item1, Canvas.GetZIndex(branch.Item2) + 1);
            Canvas.SetZIndex(((Node)nodeToChange).ellipseDebut, Canvas.GetZIndex(branch.Item2) + 1);
        }

        private void RemoveBranch(object nodeToChange, EventArgs e)
        {
            var branch = ((Node)nodeToChange).EllipseInputAndLineList.Last();

            int id = ((Node)nodeToChange)._id;
            int tag = int.Parse(branch.Item1.Tag.ToString());

            int IdTag = (1000 * id) + tag;

            canvas.Children.Remove(branch.Item1);
            canvas.Children.Remove(branch.Item2);
            canvas.Children.Remove(TextLabelDictionary[IdTag]);

            ((Node)nodeToChange).EllipseInputAndLineList.Remove(branch);
            TextLabelDictionary.Remove(IdTag);
        }

        private void RemoveNode(object nodeToRemove, EventArgs e)
        {
            int id = ((Node)nodeToRemove)._id;
            int i = ((Node)nodeToRemove).EllipseInputAndLineList.Count - 1;

            while (i != -1)
            {
                var branch = ((Node)nodeToRemove).EllipseInputAndLineList[i];

                int tag = int.Parse(branch.Item1.Tag.ToString());
                int IdTag = (1000 * id) + tag;

                canvas.Children.Remove(branch.Item1);
                canvas.Children.Remove(branch.Item2);
                canvas.Children.Remove(TextLabelDictionary[IdTag]);

                ((Node)nodeToRemove).EllipseInputAndLineList.Remove(branch);
                TextLabelDictionary.Remove(IdTag);

                i--;
            }
            canvas.Children.Remove(((Node)nodeToRemove).ellipseDebut);
            NodeDictionary.Remove(id);
            _numberOfElement -= 1;
        }

        public void ClearAll()
        {
            FeatureDictionary.Clear();
            NodeDictionary.Clear();
            TextLabelDictionary.Clear();
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
                foreach (var tupleFin in NodeDictionary[i].EllipseInputAndLineList)
                {
                    double w2 = tupleFin.Item1.Width + tupleFin.Item1.Width / 2;
                    double h2 = tupleFin.Item1.Height + tupleFin.Item1.Height / 2;

                    Point hg2 = new Point(Canvas.GetLeft(feature) - w2 / 2, Canvas.GetTop(feature) - h2 / 2);
                    Point hd2 = new Point(hg2.X + feature.RenderSize.Width + w2 / 2, hg2.Y - h2 / 2);
                    Point bg2 = new Point(hg2.X - w2 / 2, hg2.Y + feature.RenderSize.Height + h2 / 2);

                    Point hgf2 = new Point(Canvas.GetLeft(tupleFin.Item1), Canvas.GetTop(tupleFin.Item1));
                    Point hdf2 = new Point(hgf2.X + tupleFin.Item1.Width, hgf2.Y);
                    Point bgf2 = new Point(hgf2.X, hgf2.Y + tupleFin.Item1.Height);

                    bool left2 = hgf2.X > hg2.X;
                    bool up2 = hgf2.Y > hg2.Y;
                    bool right2 = hdf2.X < hd2.X;
                    bool down2 = bgf2.Y < bg2.Y;

                    if (left2 && up2 && right2 && down2)
                    {
                        int key = i * 1000 + int.Parse(tupleFin.Item1.Tag.ToString());
                        _areOnThisIn.Add(key);
                    }
                }
            }
        }

        #endregion   
    }
}
