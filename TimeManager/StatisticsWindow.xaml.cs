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
        DBAccess _dbAccess;

        byte CurrSlctdMonth
        {
            get
            {
                return (byte)(MonthComboBox.SelectedIndex + 1);
            }
        }

        public StatisticsWindow()
        {
            InitializeComponent();
            _dbAccess = DBAccess.GetInstance();            

            MonthComboBox.SelectedIndex = DateTime.Today.Month - 1;
            Week.PrepareWeekComboBox(StatWindWeekComboBox, Period.Month, (byte)DateTime.Today.Month);
            DayComboBox.SelectedIndex = (int)DateTime.Today.DayOfWeek - 1;
            //Setting selection to "Week":
            PeriodComboBox.SelectedIndex = 1;
            DayComboBox.IsEnabled = false;

            DayComboBox.SelectionChanged += ComboBox_SelectionChanged_DefaultHandler;
            MonthComboBox.SelectionChanged += MonthComboBox_SelectionChanged;
            PeriodComboBox.SelectionChanged += PeriodComboBox_SelectionChanged;
            StatWindWeekComboBox.SelectionChanged += ComboBox_SelectionChanged_DefaultHandler;

            UpdateStatisticsVisualisation();
        }
        
        //--------------------------------------------- COMBOBOXES HANDLERS --------------------------------------------
        
        private void ComboBox_SelectionChanged_DefaultHandler(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatisticsVisualisation();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Week.PrepareWeekComboBox(StatWindWeekComboBox, Period.Month, CurrSlctdMonth);
            UpdateStatisticsVisualisation();
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Period selectedPeriod = (Period)(sender as ComboBox).SelectedIndex;
            switch (selectedPeriod)
            {
                case Period.Day:
                    {
                        DayComboBox.IsEnabled = true;
                        StatWindWeekComboBox.IsEnabled = true;
                        break;
                    }
                case Period.Week:
                    {
                        DayComboBox.IsEnabled = false;
                        StatWindWeekComboBox.IsEnabled = true;
                        break;
                    }
                case Period.Month:
                    {
                        DayComboBox.IsEnabled = false;
                        StatWindWeekComboBox.IsEnabled = false;
                        break;
                    }
                default:
                    {
                        DayComboBox.IsEnabled = false;
                        StatWindWeekComboBox.IsEnabled = true;
                        break;
                    }
            }
            UpdateStatisticsVisualisation();
        }

        //----------------------------------------------- OTHER METHODS ------------------------------------------------

        private void ClearStatisticsVisualisation()
        {
            while (ActivityName.Children.Count > 1)
            {
                ActivityName.Children.RemoveAt(1);
                QrtsQnttyPlnnd.Children.RemoveAt(1);
                QrtsQnttyRlty.Children.RemoveAt(1);
                RltyToPlanSP.Children.RemoveAt(1);
            }
        }

        /// <summary>
        /// Prepare new label for Statistics field of Statistic window.
        /// </summary>
        /// <param name="content">Content of label. Its type should be Activity or integer.</param>
        /// <param name="firstActivity">Determine if label should be label concerning first activity.</param>
        /// <returns></returns>
        private object PrepareLabel(object content, bool firstActivity)
        {
            if (content is Activity)
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
        /// <summary>
        /// Updates part of content of StatisticsWindow according to chosen values of ComboBoxes.
        /// </summary>
        public void UpdateStatisticsVisualisation()
        {
            int firstQuarter = 0;
            int lastQuarter = 1;
            Period selectedPeriod = (Period)PeriodComboBox.SelectedIndex;

            switch (selectedPeriod)
            {
                case Period.Day:
                    {
                        firstQuarter = VariousTools.NumberOfFirstQuarterOfFirstWeekInMonth(CurrSlctdMonth);
                        firstQuarter += StatWindWeekComboBox.SelectedIndex * 7 * 96 + DayComboBox.SelectedIndex * 96;
                        lastQuarter = firstQuarter + 96 - 1;
                        break;
                    }
                case Period.Week:
                    {
                        firstQuarter = VariousTools.NumberOfFirstQuarterOfFirstWeekInMonth(CurrSlctdMonth);
                        firstQuarter += StatWindWeekComboBox.SelectedIndex * 7 * 96;
                        lastQuarter = firstQuarter + 96 * 7 - 1;
                        break;
                    }
                case Period.Month:
                    {
                        firstQuarter = VariousTools.NumberOfFirstQuarterOfFirstWeekInMonth(CurrSlctdMonth);
                        lastQuarter = firstQuarter + 96 * 7 * StatWindWeekComboBox.Items.Count - 1;
                        break;
                    }
                default:
                    {                        
                        break;
                    }
            }

            List<Activity> activities = ActivitiesManager.GetInstance().Activities;

            ClearStatisticsVisualisation();

            foreach (Activity activity in activities)
            {
                bool firstActivity = activity == activities.First();
                short numOfQrtsAccToPlan = (short)_dbAccess.CountActivityAppearences(activity, QrtrsMrkngMode.Planning,
                    firstQuarter, lastQuarter);
                short numOfQrtsAccToReport = (short)_dbAccess.CountActivityAppearences(activity, QrtrsMrkngMode.Reporting,
                    firstQuarter, lastQuarter);
                double rltyToPlan = (double)numOfQrtsAccToReport / numOfQrtsAccToPlan;

                ActivityControl activityLabel = PrepareLabel(activity, firstActivity) as ActivityControl;
                Label QrtQntAccToPln = (Label)PrepareLabel(numOfQrtsAccToPlan, firstActivity);
                Label QrtsQnttInRlty = (Label)PrepareLabel(numOfQrtsAccToReport, firstActivity);
                Label RltyToPlan = (Label)PrepareLabel(Double.IsNaN(rltyToPlan) || Double.IsInfinity(rltyToPlan) ? 
                    "Not planned" : Math.Round(rltyToPlan * 100, 2).ToString(), firstActivity);
                ActivityName.Children.Add(activityLabel);                
                QrtsQnttyPlnnd.Children.Add(QrtQntAccToPln);
                QrtsQnttyRlty.Children.Add(QrtsQnttInRlty);
                RltyToPlanSP.Children.Add(RltyToPlan);
            }
        }
    }
}
