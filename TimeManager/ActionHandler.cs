using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace TimeManager
{
    public enum QrtrsMrkngMode { Planning, Reporting }
    public enum RemovingActivityCtrlErr { NothingActivityRemovingAttmpt, NoneActivitySelected, NoError }

    class ActionHandler
    {
        //------------------------- FIELDS DECLARATION AND PROPERTIES ------------------------------------------------------------------------
        bool _weekChngInPrgrss = false;
        private static ActionHandler _instance;
        private QrtrsMrkngMode _mode;
        ActivitiesManager _activitiesManager;
        IState _currentState;
        IState _defaultState;
        IState _QSstate;
        IState _SIPstate;
        Week _week;

        private List<Quarter> _selectedQuarters = new List<Quarter>();

        //-------------------------- METHODS - ENSURING SINGLETON PATTERN -------------------------------------------------------------------
        private ActionHandler()
        {
            
        }

        public static ActionHandler GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ActionHandler();
            }
            return _instance;
        }



        //-------------------------- START METHOD -------------------------------------------------------------------------------------------
        public void Start(ActivitiesManager activitiesManager)
        {
            _activitiesManager = activitiesManager;
            _week = Week.GetInstance();

            _defaultState = new States.Default(activitiesManager);
            _QSstate = new States.QuartersSelected();
            _SIPstate = new States.SelectionInProgress();

            _currentState = _defaultState;
        }



        //------------------- METHODS CONCERNING ADDING, CLICKING AND REMOVING ACTIVITYCONTROLS -----------------------
        public void ActivityControl_Click(object sender, RoutedEventArgs e)
        {
            _currentState.ActivityControl_Click(sender);
        }

        public void AddActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityInputBox activityInputBox = new ActivityInputBox();
            if (activityInputBox.ShowDialog() == true)
            {
                SolidColorBrush newActivityColour = activityInputBox.ChosenColor;
                string newActivityName = activityInputBox.GivenName;
                if (newActivityName.Length >= 40)
                {
                    System.Windows.Forms.MessageBox.Show("Name of activity cannot include more than 40 characters.",
                        "Adding activity failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_activitiesManager.IsColourBusy(newActivityColour))
                {
                    System.Windows.Forms.MessageBox.Show("Chosen colour is occupied, please select another.", "Color " +
                        "is occupied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_activitiesManager.IsNameBusy(newActivityName))
                {
                    System.Windows.Forms.MessageBox.Show("Chosen name is occupied, please type another.", "Name is " +
                        "occupied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (newActivityName.Equals("Nothing"))
                {
                    System.Windows.Forms.MessageBox.Show("\"Nothing\" is a special word in this programme and can not " +
                        "be used as an activity name.", "Wrong activity name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    _activitiesManager.AddActivity(newActivityName, newActivityColour); 
            }
        }

        public void RemoveActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            RemovingActivityCtrlErr error = _activitiesManager.RemoveSelectedActivity();
            switch(error)
            {
                case RemovingActivityCtrlErr.NoError:
                {
                    _week.SaveData();
                    _week.UpdateView();
                    break;
                }
                case RemovingActivityCtrlErr.NoneActivitySelected:
                {
                    System.Windows.Forms.MessageBox.Show("None of activities has been selected!", "None activity " +
                        "selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                case RemovingActivityCtrlErr.NothingActivityRemovingAttmpt:
                {
                    System.Windows.Forms.MessageBox.Show("You cannot delete \"Nothing\" pseudoactivity!", "Attempt" +
                        " to remove \"Nothing\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }            
        }



        //-------------------------- METHODS CONCERNING DIRECTLY _selectedQuarters LIST -------------------------------
        /// <summary>
        /// Adds given quarter to _selectedQuarter list if this list does not contain given quarter.
        /// </summary>
        /// <param name="selectedQuarter">Quarter described as 'given quarter' in summary.</param>
        public void ConditionallyAddToSelection(Quarter selectedQuarter)
        {
            if(_selectedQuarters.Contains(selectedQuarter) == false)
            _selectedQuarters.Add(selectedQuarter);
        }

        public void AssignActivityToSelectedQuarters(string activityName)
        {
            Activity desiredActivity = _activitiesManager.GetActivity(activityName);
            foreach (Quarter quarter in _selectedQuarters)
            {
                if (_mode == QrtrsMrkngMode.Planning)
                {
                    quarter.PlannedActivity = desiredActivity;
                }
                else
                {
                    quarter.RealActivity = desiredActivity;
                }
            }
            _currentState = _defaultState;
        }

        public void DeleteSelection()
        {
            foreach (Quarter quarter in _selectedQuarters)
                quarter.SetStandardFrame();
            _selectedQuarters.RemoveAll(quarter => quarter != null);
        }
        


        //-------------------------- METHODS HANDLING MOUSE EVENTS CONCERNING SELECTION -------------------------------
        public void WeekGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _currentState.WeekGrid_MouseLeave();
        }

        public void QuarterRectangle_MouseLeftButtonDown(Quarter quarterSender)
        {
            _currentState.QuarterRectangle_MouseLeftButtonDown(quarterSender);
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            _currentState.QuarterRectangle_MouseEnter(quarterSender);
        }

        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentState.Window_MouseLeftButtonDown();
        }

        public void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _currentState.Window_MouseLeftButtonUp();
        }



        //-------------------------- METHODS TO SET PARTICULAR STATES -------------------------------------------------
        public void SetDefaultState()
        {
            _currentState = _defaultState;
            _currentState.OnEnter();
        }

        public void SetSIPstate()
        {
            _currentState = _SIPstate;
            _currentState.OnEnter();
        }

        public void SetQSstate()
        {
            _currentState = _QSstate;
            _currentState.OnEnter();
        }


        //-------------------------- OTHER ----------------------------------------------------------------------------
        public void StatsBtn_Click(object sender, RoutedEventArgs e)
        {
            _week.SaveData();
            StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.ShowDialog();
        }

        public void PlanningRB_Checked(object sender, RoutedEventArgs e)
        {
            _mode = QrtrsMrkngMode.Planning;
        }

        public void ReportingRB_Checked(object sender, RoutedEventArgs e)
        {
            _mode = QrtrsMrkngMode.Reporting;
        }

        public void WeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_weekChngInPrgrss)
            {
                _weekChngInPrgrss = true;
                string message = "Are you sure that you want change week to another week?";
                string title = "Change of week - Ackonwledgement";
                DialogResult MssgBoxResult = System.Windows.Forms.MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (MssgBoxResult == DialogResult.OK)
                {
                    _week.SaveData();
                    _week.UpdateView();
                }
                else
                {
                    (sender as System.Windows.Controls.ComboBox).SelectedItem = e.RemovedItems[0];
                }
                _weekChngInPrgrss = false;
            }
        }
    }
}
