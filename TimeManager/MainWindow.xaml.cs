using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeManager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Week _week;        

        public MainWindow()
        {
            InitializeComponent();

            ActionHandler actionHandler = ActionHandler.GetInstance();
            ActivitiesManager activitiesManager = ActivitiesManager.GetInstance();
            activitiesManager.Start(ActivitiesPanel, actionHandler);
            actionHandler.Start(activitiesManager);
            _week = new Week(WeekGrid);

            AddActivityBtn.Click += actionHandler.AddActivityBtn_Click;
            RemoveActivityBtn.Click += actionHandler.RemoveActivityBtn_Click;
            TestBtn.Click += actionHandler.TestBtn_Click;

            string[] array = DB.Access.GetInstance().ReadData();
        }

        private void Window_Closing(object sender,
        System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Click \"OK\" and wait a moment. \nData saving will take about 10 seconds.");

            _week.SaveData();
            DB.Access.GetInstance().On_Window_Closing();
        }
    }

}
