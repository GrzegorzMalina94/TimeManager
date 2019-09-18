using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeManager
{
    public class Quarter
    {
        Rectangle _quarterRectangle;        
        ActionHandler _actionHandler;
        Activity _assignedActivity;
        ActivitiesManager _activitiesManager;

        public Activity AssignedActivity
        {
            get
            {
                return _assignedActivity;
            }

            set
            {
                _assignedActivity = value;
                Binding binding = new Binding("SquareColor");
                binding.Source = _assignedActivity;
                _quarterRectangle.SetBinding(Shape.FillProperty, binding);
            }
        }
        
        

        public Quarter(Rectangle quarterRectangle, string activityName)
        {
            _quarterRectangle = quarterRectangle;
            _activitiesManager = ActivitiesManager.GetInstance();
            AssignedActivity = _activitiesManager.GetActivity(activityName);
            _actionHandler = ActionHandler.GetInstance();
            
            _quarterRectangle.MouseLeftButtonDown += _quarterRectangle_MouseLeftButtonDown;
            _quarterRectangle.MouseEnter += _quarterRectangle_MouseEnter;
            _quarterRectangle.MouseLeftButtonUp += _quarterRectangle_MouseLeftButtonUp;
        }
        


        private void _quarterRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _actionHandler.QuarterRectangle_MouseLeftButtonDown(this);
        }

        private void _quarterRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            _actionHandler.QuarterRectangle_MouseEnter(this);
        }

        private void _quarterRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _actionHandler.QuarterRectangle_MouseLeftButtonUp();
        }



        public void AddFrame()
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
            _quarterRectangle.Stroke = blackBrush;
            _quarterRectangle.StrokeThickness = 2;
        }

        public void RemoveFrame()
        {
            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            _quarterRectangle.Stroke = whiteBrush;
            _quarterRectangle.StrokeThickness = 1;
        }

        public void Colour(SolidColorBrush color)
        {
            _quarterRectangle.Fill = color;
        }
    }
}
