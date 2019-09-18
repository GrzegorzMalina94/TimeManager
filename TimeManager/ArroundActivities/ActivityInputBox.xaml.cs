using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeManager
{
    /// <summary>
    /// Logika interakcji dla klasy ActivityInputBox.xaml
    /// </summary>
    public partial class ActivityInputBox : Window
    {
        Rectangle _chosenSquare;
        
        SolidColorBrush[] SquareBrushes = new SolidColorBrush[18]
        {
            new SolidColorBrush(Colors.GreenYellow),
            new SolidColorBrush(Colors.Olive),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.Aqua),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.DarkBlue),

            new SolidColorBrush(Colors.Yellow),
            new SolidColorBrush(Colors.Orange),
            new SolidColorBrush(Colors.Brown),
            new SolidColorBrush(Colors.DarkSalmon),
            new SolidColorBrush(Colors.OrangeRed),
            new SolidColorBrush(Colors.Red),

            new SolidColorBrush(Colors.Bisque),
            new SolidColorBrush(Colors.Tan),
            new SolidColorBrush(Colors.RosyBrown),
            new SolidColorBrush(Colors.LightGray),
            new SolidColorBrush(Colors.Gray),
            new SolidColorBrush(Colors.DarkSlateGray)
        };

        public SolidColorBrush ChosenColor
        {
            get { return (SolidColorBrush)_chosenSquare.Fill; }
        }

        public string GivenName
        {
            get { return ActivityName.Text; }
        }

        public ActivityInputBox()
        {
            InitializeComponent();
            Rectangle currentRectangle;
            int columnsAmount = ColorsGrid.ColumnDefinitions.Count;
            int rowsAmount = ColorsGrid.RowDefinitions.Count;
            for (int row = 0; row < rowsAmount; row++)
            {
                for(int col = 0; col < columnsAmount; col++)
                {
                    currentRectangle = new Rectangle();
                    currentRectangle.Width = 20;
                    currentRectangle.Height = 20;
                    currentRectangle.Fill = SquareBrushes[row * columnsAmount + col];
                    currentRectangle.Stroke = new SolidColorBrush(Colors.Black);
                    currentRectangle.StrokeThickness = 1;
                    if(row == 0 && col == 0)
                    {
                        currentRectangle.StrokeThickness = 3;
                        _chosenSquare = currentRectangle;
                    }
                    currentRectangle.Margin = new Thickness(0,0,0,10);
                    currentRectangle.MouseLeftButtonDown += Square_Click;
                    ColorsGrid.Children.Add(currentRectangle);
                    Grid.SetColumn(currentRectangle, col);
                    Grid.SetRow(currentRectangle, row);
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ActivityName.SelectAll();
            ActivityName.Focus();
        }

        public void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void Square_Click(object sender, RoutedEventArgs e)
        {
            if (_chosenSquare != null) _chosenSquare.StrokeThickness = 1;
            (sender as Rectangle).StrokeThickness = 3;
            _chosenSquare = (sender as Rectangle);
        }
    }
}
