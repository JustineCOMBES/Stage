﻿using System;
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
       // List<UserControlJustine> listFeatures = new List<UserControlJustine>();
        Dictionary<int, Connecteur> ConnectorDictionary = new Dictionary<int, Connecteur>();
        Dictionary<int, UserControlJustine> featuresDictionary = new Dictionary<int, UserControlJustine>();
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

                if (!canvas.Children.Contains(element) && !(element is Ellipse) && !(element is Connecteur))
                {
                    canvas.Children.Add(element);
                }
                Canvas.SetZIndex(element, MaxZIndex());

                if (element is Ellipse)
                {
                    Ellipse elementa = (Ellipse)element;
                    ok = itemSurvole(elementa);
                }
                if (ok)
                {
                    Ellipse elementa = (Ellipse)element;
                    Tuple<int, string> sortie;
                    sortie = whoIsTheConnector(elementa);
                    Connecteur element2 = ConnectorDictionary[sortie.Item1];
                    UserControlJustine estSurvole = featuresDictionary[estSurvolepos];

                    if (element == ConnectorDictionary[sortie.Item1].ellipseDebut)
                    {
                        
                        double posx = Canvas.GetLeft(estSurvole) - estSurvole.Width / 2;
                        double posy = Canvas.GetTop(estSurvole);
                        if (!double.IsNaN(posx))
                        {
                            Canvas.SetLeft(ConnectorDictionary[sortie.Item1].ellipseDebut, posx);
                            ConnectorDictionary[sortie.Item1].line.X1 = Canvas.GetLeft(ConnectorDictionary[sortie.Item1].ellipseDebut) + ConnectorDictionary[sortie.Item1].ellipseDebut.Width / 2;
                        }
                        if (!double.IsNaN(posy))
                        {
                            Canvas.SetTop(ConnectorDictionary[sortie.Item1].ellipseDebut, posy);
                            ConnectorDictionary[sortie.Item1].line.Y1 = Canvas.GetTop(ConnectorDictionary[sortie.Item1].ellipseDebut) + ConnectorDictionary[sortie.Item1].ellipseDebut.Height / 2;
                        }
                    }
                    if (element == element2.ellipseFin)
                    {
                        double posx = Canvas.GetLeft(estSurvole);
                        double posy = Canvas.GetTop(estSurvole) - estSurvole.Height / 2;
                        if (!double.IsNaN(posx))
                        {
                            Canvas.SetLeft(ConnectorDictionary[sortie.Item1].ellipseFin, posx);
                            ConnectorDictionary[sortie.Item1].line.X2 = Canvas.GetLeft(ConnectorDictionary[sortie.Item1].ellipseFin) + ConnectorDictionary[sortie.Item1].ellipseDebut.Width / 2;
                        }
                        if (!double.IsNaN(posy))
                        {
                            Canvas.SetTop(ConnectorDictionary[sortie.Item1].ellipseFin, posy);
                            ConnectorDictionary[sortie.Item1].line.Y2 = Canvas.GetTop(ConnectorDictionary[sortie.Item1].ellipseFin) + ConnectorDictionary[sortie.Item1].ellipseFin.Height / 2;
                        }
                    }
                }
                ok = false;
            }
        }
        bool ok = false;

        public Tuple<int, string> whoIsTheConnector(Ellipse ellipse)
        {
            Tuple<int,string> sortie = new Tuple<int, string>(0,"Not a connector");
            for(int i =0 ; i<ConnectorDictionary.Count; i++)
            {
                if(ellipse == ConnectorDictionary[i].ellipseDebut)
                {
                    sortie = new Tuple<int, string>(i, "debut");
                }
                if (ellipse == ConnectorDictionary[i].ellipseFin)
                {
                    sortie = new Tuple<int, string>(i, "fin");
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

        #region Listes

        public List<double> Liste = new List<double>();

        public void addDB(double pos)
        {
            this.Liste.Add(pos);
        }
        public void clearDB()
        {
            for (int i = 0; i < Liste.Count; i++)
            {
                var pos = Liste[i];
                this.Liste.Remove(pos);
            }
        }
        public List<double> Liste2 = new List<double>();
        public void addDB2(double pos)
        {
            this.Liste2.Add(pos);
        }
        public void clearDB2()
        {
            for (int i = 0; i < Liste2.Count; i++)
            {
                var pos = Liste2[i];
                this.Liste2.Remove(pos);
            }
        }
        #endregion

        #region Save & Load

        int numberOfElement = 0;

        public void save()
        { //https://docs.microsoft.com/fr-fr/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
            clearDB();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xaml"; // Default file extension
            dlg.Filter = "XAML (.xaml)|*.xaml|All Files (*.*)|*.*"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            string fileName = "";
            if (result == true)
            {
                // Save document
                fileName = dlg.FileName;
            }
            else { return; }
            // string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.OpenOrCreate);
            //// double RectangleLeft = Canvas.GetLeft(LabelRectangle);
            // //double RectangleTop = Canvas.GetTop(LabelRectangle);


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
                sw.Close();
            }
        }


        public void load()
        {
            clearAll();
            clearDB2();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            // Process Open file dialog box results
            string fileName = "";
            if (result == true)
            {
                // Open document
                fileName = dlg.FileName;
            }
            else { return; }

            //string fileName = @"C:\Github\Stage\Tests\DragDropSave\DragAndDrop\myFile.txt";
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
                { double coordonnee = 0;
                    if (ligne != "NAN")
                    {
                        coordonnee = Convert.ToDouble(ligne);
                    }

                    // coordonnees
                    this.Liste2.Add(coordonnee);
                }
            }

            for (int i = 0; i <= numberOfElement; i = i + 2)
            {
                possItem(Liste2[i], Liste2[i + 1]);
            }


            sr.Close();
        }

        #endregion

        #region Add item

        public void addItem()
        {
            numberOfElement += 1;
            UserControlJustine NewElement = new UserControlJustine();
            //listFeatures.Add(NewElement);

            if (featuresDictionary.Count > 0)
                featuresDictionary.Add(featuresDictionary.Keys.Max() + 1, NewElement);
            else
                featuresDictionary.Add(0, NewElement);

            canvas.Children.Add(NewElement);
            Canvas.SetZIndex(NewElement, numberOfElement);
        }

        public void possItem(double possx, double possy)
        {
            UserControlJustine NewElement = new UserControlJustine();
            //listFeatures.Add(NewElement);
            if (featuresDictionary.Count > 0)
                featuresDictionary.Add(featuresDictionary.Keys.Max() + 1, NewElement);
            else
                featuresDictionary.Add(0, NewElement);
            canvas.Children.Add(NewElement);
            Canvas.SetLeft(NewElement, possy);
            Canvas.SetTop(NewElement, possx);
        }

        public void possConnector(double possx, double possy)
        {
            Connecteur NewElement = new Connecteur();
            canvas.Children.Add(NewElement);
            Canvas.SetLeft(NewElement, possy);
            Canvas.SetTop(NewElement, possx);
        }

        #endregion

        #region Add Connector

        public void addConnector()
        {
            numberOfElement += 1;
            Connecteur NewElement = new Connecteur(new Connecteur.PointD(10, 10), new Connecteur.PointD(50, 50));
            if (ConnectorDictionary.Count > 0)
                ConnectorDictionary.Add(ConnectorDictionary.Keys.Max() + 1, NewElement);
            else
                ConnectorDictionary.Add(0, NewElement);

            Canvas.SetZIndex(NewElement, numberOfElement);
            canvas.Children.Add(NewElement);
        }
        #endregion

        #region Clear
        public void clearAll()
        {
            int cpt = canvas.Children.Count;
            for (int i = cpt - 1; i >= 0; i += -1)
            {
                UIElement Child = canvas.Children[i];
                canvas.Children.Remove(Child);
            }
            numberOfElement = 0;
        }

        public void clearOne()
        {

        }
        #endregion


        #region Item survolé

        private int _estSurvolepos;
        public int estSurvolepos
        {
        get { return _estSurvolepos; }
        set 
        {
                _estSurvolepos = value;
        }
        }

        public bool itemSurvole(Ellipse link)//, string str)
         {
            Point hg;
            Point hd;
            Point bg;
            Point bd;
            //if (str == "debut")
            //{
            //    hg = new Point(Canvas.GetLeft(link.ellipseDebut), Canvas.GetTop(link.ellipseDebut));
            //    hd = new Point(hg.X + link.ellipseDebut.Width, hg.Y);
            //    bg = new Point(hg.X, hg.Y + link.ellipseDebut.Height);
            //    bd = new Point(hg.X + link.ellipseDebut.Width, hg.Y + link.ellipseDebut.Height);
            //}
            //else
           // {
                //if (str == "fin")
               // {
                    hg = new Point(Canvas.GetLeft(link), Canvas.GetTop(link));
                    hd = new Point(hg.X + link.Width, hg.Y);
                    bg = new Point(hg.X, hg.Y + link.Height);
                    bd = new Point(hg.X + link.Width, hg.Y + link.Height);
            // }
            //    else
            //    {
            //        hg = new Point(0, 0);
            //        hd = new Point(0, 0);
            //        bg = new Point(0, 0);
            //        bd = new Point(0, 0);
            //    }
            //}
            for(int i = 0; i < featuresDictionary.Count; i++)
             {
                UserControlJustine features = featuresDictionary[i];
                Point hgf = new Point(Canvas.GetLeft(features), Canvas.GetTop(features));
                Point hdf = new Point(hgf.X + features.Width, hgf.Y);
                Point bgf = new Point(hgf.X, hgf.Y + features.Height);
                Point bdf = new Point(hgf.X + features.Width, hgf.Y + features.Height);

                bool left = hgf.X < hg.X;
                bool up = hgf.Y < hg.Y;
                bool right = hdf.X > hd.X;
                bool down = bgf.Y > bg.Y;

                if(left && up && right && down)
                {
                    return (true);
                    estSurvolepos = i;
                }
                else
                {
                    estSurvolepos = 0;
                }
            }
            return (false);
         }
        #endregion

    }
}
