using System;
using System.Windows;

namespace TimeManager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Week _week;
        DBAccess _dbAccess;

        public MainWindow()
        {
            InitializeComponent();

            ActionHandler actionHandler = ActionHandler.GetInstance();
            ActivitiesManager activitiesManager = ActivitiesManager.GetInstance();
            _week = Week.GetInstance();            
            _dbAccess = DBAccess.GetInstance();
            
            activitiesManager.Start(ActivitiesPanel, actionHandler, _week);
            actionHandler.Start(activitiesManager);
            _week.ActualInitialisation(WeekGrid, WeekComboBox);

            StatsBtn.Click += actionHandler.StatsBtn_Click;
            WeekComboBox.SelectionChanged += actionHandler.WeekComboBox_SelectionChanged;
            PlanningRB.Checked += actionHandler.PlanningRB_Checked;
            ReportingRB.Checked += actionHandler.ReportingRB_Checked;
            AddActivityBtn.Click += actionHandler.AddActivityBtn_Click;
            RemoveActivityBtn.Click += actionHandler.RemoveActivityBtn_Click;
            TestBtn.Click += actionHandler.TestBtn_Click;          
        }   

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _week.SaveData();
        }
    }
}
