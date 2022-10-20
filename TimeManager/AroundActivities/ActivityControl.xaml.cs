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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeManager
{
    /// <summary>
    /// Logika interakcji dla klasy ActivityControl.xaml
    /// </summary>
    public partial class ActivityControl : UserControl
    {
        public ActivityControl()
        {
            InitializeComponent();
        }

        public SolidColorBrush ActivityColor
        {
            get { return (SolidColorBrush)Square.Fill; }
            set { Square.Fill = value; }
        }

        public String ActivityName
        {
            get { return ActivityLabel.Content.ToString(); }
            set { ActivityLabel.Content = value; }
        }
    }
}
