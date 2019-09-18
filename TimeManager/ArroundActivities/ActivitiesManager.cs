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
        public static Activity NullActivity { get; } = new Activity("Nothing", Colors.Yellow);

        private ActivitiesManager()
        {
            
        }

        public static ActivitiesManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ActivitiesManager();
            }
            return _instance;
        }

        public void Start(StackPanel activitiesPanel, ActionHandler actionHandler)
        {
            _activitiesPanel = activitiesPanel;
            _actionHandler = actionHandler;
            if (File.Exists("Activities.dat"))
            {
                using (Stream input = File.OpenRead("Activities.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    _activities = (List<Activity>)bf.Deserialize(input);
                }

                foreach (Activity activity in _activities)
                    AddActivityControl(activity.Name, activity.SquareColor);
            }
        }

        /// <summary>
        /// Saves all activities in a file.
        /// </summary>
        ~ActivitiesManager()
        {
            using (Stream output = File.Create("Activities.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(output, _activities);
            }
        }

        public void AddActivity(string name, SolidColorBrush color)
        {
            if (name != "Nothing")
            {
                AddActivityControl(name, color);
                _activities.Add(new Activity(name, color.Color));
            }
            else
            {
                MessageBox.Show("\"Nothing\" is a special word in this programme and can not be used as an activity name.", "Wrong activity name!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveSelectedActivity()
        {
            if (_selectedAC != null)
            {
                Activity selectedActivity = GetActivity(_selectedAC.ActivityName);
                _activitiesPanel.Children.Remove(_selectedAC);
                _activities.Remove(selectedActivity);
                _selectedAC = null;
            }
        }

        public void ChangeActivitySelection(object sender)
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

        public Activity GetActivity(string activityName)
        {
            if (activityName != "Nothing")
            {
                return _activities.Where(activity => activity.Name == activityName).Last();
            }
            else
            {
                return NullActivity;
            }
        }
    }
}
