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
using Test_Drag_and_DRop.Introduction;
using utile =  Test_Drag_and_DRop.Introduction.Utile;

namespace Test_Drag_and_DRop.Introduction
{
    public partial class Link : utile
    {
        
        public override double X
        {
            get { return 0; }
            set { }
        }

        public override double Y
        {
            get { return 0; }
            set { }
        }

        private Node _start;
        public Node Start
        {
            get { return _start; }
            set
            {
                _start = value;
                utile.OnPropertyChanged("Start");
            }
        }

        private Node _end;
        public Node End
        {
            get { return _end; }
            set
            {
                _end = value;
                Utile.OnPropertyChanged("End");
            }
        }
    }
}
