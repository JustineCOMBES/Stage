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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;


namespace DragDropSave
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropCanvas.AddFeature();
        }

        private void clearbutton_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropCanvas.clearAll();
        }

        private void addConnectorbutton_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropCanvas.AddNode();
        }
    }
}
