using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeManager
{
    class ActivitiesManager
    {
        static ActivitiesManager _instance;
        StackPanel _activitiesPanel;
        ActionHandler _actionHandler;
        List<Activity> _activities = new List<Activity>();
        ActivityControl _selectedAC;
        Week _week;
        public static Activity NullActivity { get; } = new Activity("Nothing", Colors.White);

        public List<Activity> Activities
        {
            get
            {
                return _activities;
            }
        }

        private ActivitiesManager()
        {

        }

        /// <summary>
        /// Saves all activities in a file.
        /// </summary>
        ~ActivitiesManager()
        {
            using (Stream output = File.Create("Activities.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                _activities = _activities.Where(activity => activity.Name != "Nothing").ToList();
                if (_activities.Count >= 1)
                    bf.Serialize(output, _activities);
            }
        }

        public static ActivitiesManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ActivitiesManager();
            }
            return _instance;
        }

        public bool IsColourBusy(SolidColorBrush colour)
        {
            return _activities.Exists( activity => new SolidColorBrushComparer().Equals(activity.SquareColor, colour));
        }

        public bool IsNameBusy(string name)
        {
            return _activities.Exists(activity => activity.Name.Equals(name));
        }

        public void Start(StackPanel activitiesPanel, ActionHandler actionHandler, Week week)
        {
            _activitiesPanel = activitiesPanel;
            _actionHandler = actionHandler;
            _week = week;

            AddActivity("Nothing", new SolidColorBrush(Colors.White));
            if (File.Exists("Activities.dat"))
            {
                List<Activity> temp = new List<Activity>();
                using (Stream input = File.OpenRead("Activities.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    if(input.Length != 0)
                    temp = (List<Activity>)bf.Deserialize(input);
                }

                if(temp.Count > 0)
                foreach (Activity activity in temp)
                    AddActivity(activity.Name, activity.SquareColor);
            }            
        }

        public void AddActivity(string name, SolidColorBrush color)
        {
            AddActivityControl(name, color);
            _activities.Add(new Activity(name, color.Color));
        }

        public RemovingActivityCtrlErr RemoveSelectedActivity()
        {
            if (_selectedAC == null)
            {
                return RemovingActivityCtrlErr.NoneActivitySelected;
            }
            if (_selectedAC.ActivityName.Equals("Nothing"))
            {
                return RemovingActivityCtrlErr.NothingActivityRemovingAttmpt;
            }

            Activity selectedActivity = GetActivity(_selectedAC.ActivityName);
            _activitiesPanel.Children.Remove(_selectedAC);
            _activities.Remove(selectedActivity);
            _selectedAC = null;

            return RemovingActivityCtrlErr.NoError;
            
        }

        public void HandleActivityControlClick(object sender)
        {
            if (_selectedAC != null)
                _selectedAC.BorderThickness = new Thickness(0);
            if (_selectedAC != sender as ActivityControl)
            {
                _selectedAC = sender as ActivityControl;
                _selectedAC.BorderThickness = new Thickness(2);
            }
            else
                _selectedAC = null;
        }

        public void AddActivityControl(String name, SolidColorBrush color)
        {
            ActivityControl activityControl = new ActivityControl();
            activityControl.ActivityName = name;
            activityControl.ActivityColor = color;
            activityControl.MouseLeftButtonDown += _actionHandler.ActivityControl_Click;
            _activitiesPanel.Children.Add(activityControl);
        }

        public void DeleteSelection()
        {
            if(_selectedAC != null)
            _selectedAC.BorderThickness = new Thickness(0);
        }

        public Activity GetActivity(string activityName)
        {
            //desiredActivity list should be list with one element
            List<Activity> desiredActivity = _activities.Where(activity => activity.Name == activityName).ToList();
            if (desiredActivity.Count() > 0)
            {
                return desiredActivity.Last();
            }
            else
            {
                return NullActivity;
            }
        }
    }
}
