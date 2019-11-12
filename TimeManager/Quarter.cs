using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeManager
{
    public class Quarter
    {
        Rectangle _plnnActvtRctng;
        Rectangle _realActvtRctng;
        StackPanel _quarterArea;
        ActionHandler _actionHandler;
        Activity _plannedActivity;
        Activity _realActivity;
        public QuarterIdentifier Identifier { get; set; }

        public Activity PlannedActivity
        {
            get
            {
                return _plannedActivity;
            }
            set
            {
                _plannedActivity = value;
                Binding binding = new Binding("SquareColor");
                binding.Source = _plannedActivity;
                _plnnActvtRctng.SetBinding(Shape.FillProperty, binding);
            }
        }

        public Activity RealActivity
        {
            get
            {
                return _realActivity;
            }
            set
            {
                _realActivity = value;
                Binding binding = new Binding("SquareColor");
                binding.Source = _realActivity;
                _realActvtRctng.SetBinding(Shape.FillProperty, binding);
            }
        }



        public Quarter(StackPanel quarterArea, Activity plannedActivity, Activity realActivity, QuarterIdentifier identifier)
        {
            _quarterArea = quarterArea;

            _plnnActvtRctng = new Rectangle();
            _realActvtRctng = new Rectangle();
            _realActvtRctng.Width = 30;
            PlannedActivity = plannedActivity;
            RealActivity = realActivity;
            _quarterArea.Children.Add(_plnnActvtRctng);
            _quarterArea.Children.Add(_realActvtRctng);

            Identifier = identifier;

            _actionHandler = ActionHandler.GetInstance();
            

            SetStandardFrame();
            _plnnActvtRctng.MouseLeftButtonDown += _quarterRectangle_MouseLeftButtonDown;
            _plnnActvtRctng.MouseEnter += _quarterRectangle_MouseEnter;
            _plnnActvtRctng.MouseLeftButtonUp += _quarterRectangle_MouseLeftButtonUp;
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



        public void SetSelectionFrame()
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
            Border border = (_quarterArea.Parent as Border);
            border.BorderBrush = blackBrush;
            border.BorderThickness = new Thickness(2);
            _plnnActvtRctng.Height = 16;// (borders thickness) * 2 + rectangle height = 20

            _realActvtRctng.Height = 12;
            _realActvtRctng.Margin = new Thickness(90, -14, 0, 0);
        }

        public void SetStandardFrame()
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
            Border border = (_quarterArea.Parent as Border);
            border.BorderBrush = blackBrush;
            _plnnActvtRctng.Height = 18.75;//Height of hour = 80 = rectHeight * 4 + borders = 75 + 1 + 1 + 1 + 1 + 1 => rectHeight = 75 / 4 = 18.75

            _realActvtRctng.Height = 13;
            _realActvtRctng.Margin = new Thickness(90, -15.88, 0, 0);

            switch (Identifier.QuarterNumber % 4 )
            {
                case 0:
                    border.BorderThickness = new Thickness(0.5, 1, 0.5, 0.5);
                    break;
                case 1:
                    border.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                    break;
                case 2:
                    border.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                    break;
                case 3:
                    border.BorderThickness = new Thickness(0.5, 0.5, 0.5, 1);
                    break;
                default:
                    border.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                    break;
            }
        }

        public void Colour(SolidColorBrush color)
        {
            _plnnActvtRctng.Fill = color;
        }
    }
}
