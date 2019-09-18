using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeManager
{
    class Day
    {
        Quarter[] _quarters = new Quarter[96];
        StackPanel _dayStackPanel { get; set; }
        string _name;

        public Day(StackPanel dayStackPanel, string name, string[] activities)
        {
            _dayStackPanel = dayStackPanel;            
            _name = name;

            //Creating header.
            Label label = new Label();
            label.Content = _name;
            label.Height = 25;
            label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            _dayStackPanel.Children.Add(label);

            //Creating all quarters.
            for (int i = 0; i < 96; i++)
            {
                Rectangle rectangle = new Rectangle();
                SolidColorBrush whiteBrush = new SolidColorBrush();
                whiteBrush.Color = Colors.White;
                rectangle.Height = 20;
                rectangle.Stroke = whiteBrush;
                rectangle.StrokeThickness = 1;
                _dayStackPanel.Children.Add(rectangle);

                if(activities != null) //Execute when data from database are available.
                    _quarters[i] = new Quarter(rectangle, activities[i]);
                else                   //Execute when data from are NOT available.
                    _quarters[i] = new Quarter(rectangle, "Nothing");
            }            
        }

        public string[] GetDayPlan()
        {
            string[] dayPlan = new string [96];

            for (int i = 0; i < 96; i++)
                dayPlan[i] = _quarters[i].AssignedActivity.Name;

            return dayPlan;

        }
    }
}
