using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeManager
{    
    class Day
    {
        Quarter[] _quarters = new Quarter[96];
        StackPanel _dayStackPanel { get; set; }
        ActivitiesManager _activitiesManager;
        string _name;
        byte _id;

        public Day(StackPanel dayStackPanel, string name, string[] plan, string[] report, byte id)
        {
            _dayStackPanel = dayStackPanel;
            _name = name;
            _id = id;
            _activitiesManager = ActivitiesManager.GetInstance();
            
            //Creating header.
            Label label = new Label();
            label.Content = _name;
            label.Height = 25;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.FontSize = 12;
            _dayStackPanel.Children.Add(label);

            //Creating all quarters:
            //When some data was taken from database.
            if(plan != null)
            {
                for (byte i = 0; i < 96; i++)
                {
                    Activity plannedAactivity = _activitiesManager.GetActivity(plan[i] != null ? plan[i] : "Nothing");
                    Activity reportedActivity = _activitiesManager.GetActivity(report[i] != null ? report[i] : "Nothing");
                    _quarters[i] = new Quarter(PrepareQuartersArea(), plannedAactivity, reportedActivity, new QuarterIdentifier(_id, i));                  
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

        public string[] GetDayPlan()
        {
            string[] dayPlan = new string[96];

            for (int i = 0; i < 96; i++)
                dayPlan[i] = _quarters[i].PlannedActivity.Name;

            return dayPlan;
        }

        public string[] GetDayReport()
        {
            string[] dayReport = new string[96];

            for (int i = 0; i < 96; i++)
                dayReport[i] = _quarters[i].RealActivity.Name;

            return dayReport;
        }

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

        public void NullAssigment(QrtrsMrkngMode markingMode, byte qrtNumber)
        {
            switch(markingMode)
            {
                case QrtrsMrkngMode.Planning:
                    _quarters[qrtNumber].PlannedActivity = ActivitiesManager.NullActivity;
                    break;
                case QrtrsMrkngMode.Reporting:
                    _quarters[qrtNumber].RealActivity = ActivitiesManager.NullActivity;
                    break;
                default:
                    _quarters[qrtNumber].PlannedActivity = ActivitiesManager.NullActivity;
                    break;
            }
            
        }
    }
}
