using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Drag_and_DRop.Introduction
{
    public class Link: Test_Drag_and_DRop.Introduction.Utile
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
                OnPropertyChanged("Start");
            }
        }

        private Node _end;
        public Node End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }
    }
}
