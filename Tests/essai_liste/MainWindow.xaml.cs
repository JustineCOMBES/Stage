using System;
using System.IO;
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
using System.Reflection;

namespace essai_liste
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ToDo> Liste = new List<ToDo>();
        public MainWindow()
        {
            InitializeComponent();
            ToDo task = new ToDo();
            ToDo task2 = new ToDo("task2","description","1");
            ToDo task3 = new ToDo("task3", "1");

            this.addTask(task);
            this.addTask(task2);
            this.addTask(task3);
            updateList();
        }

        public void addTask(ToDo task)
        {
            this.Liste.Add(task);
        }

        public void RemoveTask(ToDo task)
        {
            this.Liste.Remove(task);
        }

        public void updateList() 
        {
            mylistbox.ItemsSource = Liste;
            mylistbox.Items.Refresh();
        }


        #region TextBoxs
        private void name_MouseEnter(object sender, MouseEventArgs e)
        {
            if(LabelName.Text == "TaskName") 
            {
                LabelName.Text = "";
            }
        }

        private void name_MouseLeave(object sender, MouseEventArgs e)
        {
            if (LabelName.Text == "")
            {
                LabelName.Text = "TaskName";
            }
        }

        private void desc_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LabelDesc.Text == "Description")
            {
                LabelDesc.Text = "";
            }
        }

        private void desc_MouseLeave(object sender, MouseEventArgs e)
        {
            if (LabelDesc.Text == "")
            {
                LabelDesc.Text = "Description";
            }
        }

        private void prio_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LabelPrio.Text == "Priority")
            {
                LabelPrio.Text = "";
            }
        }

        private void prio_MouseLeave(object sender, MouseEventArgs e)
        {
            if (LabelPrio.Text == "")
            {
                LabelPrio.Text = "Priority";
            }
        }
#endregion
        #region Buttons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LabelName.Text != "" && LabelPrio.Text != "" && LabelDesc.Text != "")
            {
                ToDo task = new ToDo(LabelName.Text, LabelDesc.Text, LabelPrio.Text);
                this.addTask(task);
                updateList();
            }
        }
        private void LabelSupp_Click(object sender, RoutedEventArgs e)
        {
            var obj = mylistbox.SelectedItem;
            ToDo m = (ToDo)obj;
            RemoveTask(m);
            updateList();
        }

        private void LabelSave_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"C:\Github\Stage\Tests\essai_liste\myFile.txt";
            FileStream stream = null;
            stream = new FileStream(fileName, FileMode.Truncate);

            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (ToDo s in Liste)
                {
                    string name = s.TaskName;
                    string desc = s.Description;
                    string prio = s.Priority;
                    sw.WriteLine(name);
                    sw.WriteLine(desc);
                    sw.WriteLine(prio);
                    sw.WriteLine(" ");
                }
            }
        }
        #endregion


    }
}
