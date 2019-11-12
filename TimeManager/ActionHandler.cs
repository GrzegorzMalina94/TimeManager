using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace TimeManager
{
    public enum QrtrsMrkngMode { Planning, Reporting }

    class ActionHandler
    {
        //------------------------- FIELDS DECLARATION AND PROPERTIES ------------------------------------------------------------------------
        private static ActionHandler _instance;
        private QrtrsMrkngMode _mode;
        ActivitiesManager _activitiesManager;
        IState _currentState;
        IState _defaultState;
        IState _QSstate;
        IState _SIPstate;

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

            _defaultState = new States.Default(activitiesManager);
            _QSstate = new States.QuartersSelected();
            _SIPstate = new States.SelectionInProgress();

            _currentState = _defaultState;
        }



        //-------------------------- METHODS HANDLING BUTTONS, RADIOBUTTONS AND ACTIVITIES CLICKS --------------------------------
        public void ActivityControl_Click(object sender, RoutedEventArgs e)
        {
            _currentState.ActivityControl_Click(sender);
        }

        public void AddActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityInputBox activityInputBox = new ActivityInputBox();
            if (activityInputBox.ShowDialog() == true)
            {
                if(activityInputBox.GivenName.Length <= 40)
                    _activitiesManager.AddActivity(activityInputBox.GivenName, activityInputBox.ChosenColor);
                else
                    System.Windows.Forms.MessageBox.Show("Name of activity cannot include more than 40 characters.",
                        "Adding activity failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemoveActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            _activitiesManager.RemoveSelectedActivity();
        }
        
        public void StatsBtn_Click(object sender, RoutedEventArgs e)
        {
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



        //-------------------------- METHODS CONCERNING DIRECTLY _selectedQuarters LIST -------------------------------
        /// <summary>
        /// Adds given quarter to _selectedQuarter list if this list does not contain given quarter.
        /// </summary>
        /// <param name="selectedQuarter">Quarter described as 'given quarter' in summary.</param>
        public void ConditionallyAddToSelection(Quarter selectedQuarter)
        {
            if(!_selectedQuarters.Contains(selectedQuarter))
            _selectedQuarters.Add(selectedQuarter);
        }

        public void AssignActivityToSelectedQuarters(string activityName)
        {
            foreach (Quarter quarter in _selectedQuarters)
            {
                Activity desiredActivity = _activitiesManager.GetActivity(activityName);
                if (_mode == QrtrsMrkngMode.Planning)
                {
                    Activity prevActivity = quarter.PlannedActivity;
                    prevActivity.DeleteQuarterFromPlan(quarter.Identifier);
                    quarter.PlannedActivity = desiredActivity;
                    desiredActivity.AssignQuarterToPlan(quarter.Identifier);
                }
                else
                {
                    Activity prevActivity = quarter.RealActivity;
                    prevActivity.DeleteQuarterFromReport(quarter.Identifier);
                    quarter.RealActivity = desiredActivity;
                    desiredActivity.AssignQuarterToReport(quarter.Identifier);
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
            SetSIPstate();

            ConditionallyAddToSelection(quarterSender);
            quarterSender.SetSelectionFrame();
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            _currentState.QuarterRectangle_MouseEnter(quarterSender);
        }

        public void QuarterRectangle_MouseLeftButtonUp()
        {
            SetQSstate();
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
       



        //TEST --------------------------------------------------------------------------------------------------------
        public void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
