using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TimeManager
{
    class ActionHandler
    {
        //------------------------- FIELDS DECLARATION AND PROPERTIES --------------------------------------------------------------------------------------
        private static ActionHandler _instance;
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



        //-------------------------- METHODS HANDLING BUTTON AND ACTIVITIES CLICKS ----------------------------------------------------------
        public void AddActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityInputBox activityInputBox = new ActivityInputBox();
            if (activityInputBox.ShowDialog() == true)
            {
                _activitiesManager.AddActivity(activityInputBox.GivenName, activityInputBox.ChosenColor);                
            }
        }

        public void RemoveActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            _activitiesManager.RemoveSelectedActivity();
        }

        public void ActivityControl_Click(object sender, RoutedEventArgs e)
        {
            _currentState.ActivityControl_Click(sender);
        }



        //-------------------------- METHODS CONCERNING DIRECTLY _selectedQuarters LIST -----------------------------------------------------
        public void AddToSelection(Quarter selectedQuarter)
        {
            _selectedQuarters.Add(selectedQuarter);
        }

        public void ColourSelectedQuarters(string activityName)
        {
            foreach (Quarter quarter in _selectedQuarters)
                quarter.AssignedActivity = _activitiesManager.GetActivity(activityName);
            _currentState = _defaultState;
        }

        public void DeleteSelection()
        {
            foreach (Quarter quarter in _selectedQuarters)
                quarter.RemoveFrame();
            _selectedQuarters.RemoveAll(quarter => quarter != null);
        }



        //-------------------------- METHODS HANDLING MOUSE EVENTS CONCERNING SELECTION -----------------------------------------------------
        public void WeekGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _currentState.WeekGrid_MouseLeave();
        }

        public void QuarterRectangle_MouseLeftButtonDown(Quarter quarterSender)
        {
            SetSIPstate();
            
            AddToSelection(quarterSender);
            quarterSender.AddFrame();
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            _currentState.QuarterRectangle_MouseEnter(quarterSender);
        }

        public void QuarterRectangle_MouseLeftButtonUp()
        {
            SetQSstate();
        }



        //-------------------------- METHODS TO SET PARTICULAR STATES -----------------------------------------------------------------------
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

        //TEST -------------------------------------------------------------------------------------------------------------------------------------------------------
        public void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivitiesManager.NullActivity.SquareColor = new SolidColorBrush(Colors.Red);
        }
    }
}
