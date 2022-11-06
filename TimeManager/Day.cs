using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimeManager
{
    class Day
    {
        #region Properties

        StackPanel _dayStackPanel { get; set; }

        public string[] DayPlan
        {
            get
            {
                string[] dayPlan = new string[96];

                for (int i = 0; i < 96; i++)
                    dayPlan[i] = _quarters[i].PlannedActivity.Name;

                return dayPlan;
            }

            set
            {
                if (value.Length == 96)
                {
                    for (int i = 0; i < 96; i++)
                    {
                        _quarters[i].PlannedActivity = _activitiesManager.GetActivity(value[i]);
                    }
                }
                else
                {
                    throw new ArgumentException("Parameter (array) must have length amounting to 96.", "value");
                }
            }
        }

        public string[] DayReport
        {
            get
            {
                string[] dayReport = new string[96];

                for (int i = 0; i < 96; i++)
                    dayReport[i] = _quarters[i].RealActivity.Name;

                return dayReport;
            }

            set
            {
                if (value.Length == 96)
                {
                    for (int i = 0; i < 96; i++)
                    {
                        _quarters[i].RealActivity = _activitiesManager.GetActivity(value[i]);
                    }
                }
                else
                {
                    throw new ArgumentException("Parameter (array) must have length amounting to 96.", "value");
                }
            }
        }

        #endregion

        #region Fields

        Quarter[] _quarters = new Quarter[96];
        ActivitiesManager _activitiesManager;
        byte _id;

        #endregion

        #region ctor

        public Day(StackPanel dayStackPanel, string name, string[] plan, string[] report, byte id)
        {
            _dayStackPanel = dayStackPanel;
            _id = id;
            _activitiesManager = ActivitiesManager.GetInstance();

            //Creating all quarters:
            //When some data was taken from database.
            if(plan != null)
            {
                for (byte i = 0; i < 96; i++)
                {
                    Activity plannedActivity = _activitiesManager.GetActivity(plan[i]);
                    Activity reportedActivity = _activitiesManager.GetActivity(report[i]);
                    _quarters[i] = new Quarter(PrepareQuartersArea(), plannedActivity, reportedActivity, new QuarterIdentifier(_id, i));
                }
            }
            else // When any data wasn't taken from database.
            {
                for (byte i = 0; i < 96; i++)
                {
                    Activity activity = ActivitiesManager.NullActivity;
                    _quarters[i] = new Quarter(PrepareQuartersArea(), activity, activity, new QuarterIdentifier(_id, i));
                }
            }
            
        }

        #endregion

        #region Methods

        StackPanel PrepareQuartersArea()
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);           

            StackPanel stackPanel = new StackPanel();  
            
            border.Child = stackPanel;
            _dayStackPanel.Children.Add(border);

            return stackPanel;
        }

        internal Quarter GetQuarter(byte quarterNumber)
        {
            return _quarters[quarterNumber];
        }

        #endregion
    }
}
