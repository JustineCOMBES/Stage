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
            mylistbox.ItemsSource = Liste;
        }

        public void addTask(ToDo task)
        {
            this.Liste.Add(task);
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
                mylistbox.ItemsSource = Liste;
                mylistbox.Items.Refresh();

            }
        }
        private void LabelSupp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LabelSave_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("myFile.txt"))
            {
                foreach (ToDo s in Liste)
                {
                    string name = s.TaskName;
                    string desc = s.Description;
                    string prio = s.Priority;
                    sw.WriteLine(name);
                    sw.WriteLine(desc);
                    sw.WriteLine(prio);
                }
            }
        }
        #endregion
    }
}
