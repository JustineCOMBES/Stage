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

using System.IO;
using Microsoft.Win32;
using System.Xaml;
using System.Windows.Markup;

// https://www.youtube.com/watch?v=kI3ILsnt7JE
// https://askcodez.com/comment-creer-et-connecter-des-boutons-controles-utilisateur-personnalises-avec-des-lignes-a-laide-de-formulaires-windows.html
// https://www.codeproject.com/Articles/182683/NetworkView-A-WPF-custom-control-for-visualizing-a
namespace Test_Drag_and_DRop
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
