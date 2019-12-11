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
    /// Logika interakcji dla klasy StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            List<Activity> activities = ActivitiesManager.GetInstance().Activities;
            PrepareMainContent(DBAccess.GetInstance(), activities, Week.GetInstance().CurrentWeek);
        }

        /// <summary>
        /// Prepare new label for Statistics field of Statistic window.
        /// </summary>
        /// <param name="content">Content of label. Its type should be Activity or integer.</param>
        /// <param name="firstActivity">Determine if label should be label concerning first activity.</param>
        /// <returns></returns>
        object PrepareLabel(object content, bool firstActivity)
        {
            if(content is Activity)
            {
                Activity givenActivity = content as Activity;
                ActivityControl activityLabel = new ActivityControl();
                activityLabel.ActivityName = givenActivity.Name;
                activityLabel.ActivityColor = givenActivity.SquareColor;

                Thickness borderThickness;
                if (firstActivity)
                {
                    borderThickness = new Thickness(0, 2, 0, 1);                    
                }
                else
                {
                    borderThickness = new Thickness(0, 0, 0, 1);
                }
                activityLabel.BorderThickness = borderThickness;
                return activityLabel;
            }
            else
            {
                Label newLabel = new Label();
                newLabel.Content = content;
                newLabel.Width = Double.NaN;
                newLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                newLabel.BorderBrush = new SolidColorBrush(Colors.Black);

                Thickness borderThickness;
                if (firstActivity)
                {
                    borderThickness = new Thickness(0, 2, 0, 1);
                    newLabel.Height = 31;
                }
                else
                {
                    borderThickness = new Thickness(0, 0, 0, 1);
                    newLabel.Height = 29;
                }
                newLabel.BorderThickness = borderThickness;
                return newLabel;
            }
        }

        void PrepareMainContent(DBAccess dbAccess, List<Activity> activities, byte week)
        {
            foreach (Activity activity in activities)
            {
                bool firstActivity = activity == activities.First();

                ActivityControl activityLabel = PrepareLabel(activity, firstActivity) as ActivityControl;
                Label QrtQntAccToPln = (Label)PrepareLabel(dbAccess.CountActivityAppearences(activity, QrtrsMrkngMode.Planning, week), firstActivity);
                Label QrtsQnttInRlty = (Label)PrepareLabel(dbAccess.CountActivityAppearences(activity, QrtrsMrkngMode.Reporting, week), firstActivity);

                ActivityName.Children.Add(activityLabel);
                QrtsQnttyPlnnd.Children.Add(QrtQntAccToPln);
                QrtsQnttyRlty.Children.Add(QrtsQnttInRlty);
            }
        }
    }
}
