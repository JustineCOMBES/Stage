using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace essai_liste
{
    public class ToDo
    {
        
        private String _TaskName;
        public String TaskName
        {
            get { return _TaskName; }
            set
            {
                _TaskName = value;
            }
        }

        private String _Description;
        public String Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
            }
        }

        private string _Priority;
        public string Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
            }
        }
        public ToDo(String Name, String Desc, string Prio) 
        {
            this._TaskName = Name;
            this._Description = Desc;
            this._Priority = Prio;
        }

        public ToDo(String Name, string Prio)
        {
            this._TaskName = Name;
            this._Description = "No Description";
            this._Priority = Prio;
        }

        public ToDo() 
        {
            this._TaskName = "No Name";
            this._Description = "No Description";
            this._Priority = "0";
        }

        
    }
}
