using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Drag_and_DRop.Introduction
{
    public class Node: Test_Drag_and_DRop.Introduction.Utile
    {
        private double _x;
        public override double X
        {
            get { return _x; }
            set { }
        }

        private double _y;
        public override double Y
        {
            get { return _y; }
            set {}
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


        


    }
}
