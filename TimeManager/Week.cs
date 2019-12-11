using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimeManager
{
    public class Week
    {
        static Week _instance;        
        ActionHandler _actionHandler;
        byte _currentWeek;
        ComboBox _weekComboBox;
        Day[] _days = new Day[7];
        DBAccess _dbAccess;
        Grid _weekGrid;
        string[] _daysNames = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public static int QnttOfQrtsInWeek = 672;

        public byte CurrentWeek
        {
            get { return _currentWeek; }
        }

        public static String PrepareWeekString(DateTime weekStart, DateTime weekEnd)
        {
            string leftSide = weekStart.ToShortDateString().Remove(0, 5);
            string rightSide = weekEnd.ToShortDateString().Remove(0, 5);
            leftSide = leftSide.Replace('-', '.');
            rightSide = rightSide.Replace('-', '.');
            return leftSide + " - " + rightSide;
        }

        private Week()
        {

        }

        public static Week GetInstance()
        {
            if (_instance == null)
            {
                return _instance = new Week();
            }
            else return _instance;
        }

        private void PrepareWeekComboBox()
        {
            DateTime firstJanuary = DateTime.Today.AddDays(-DateTime.Today.DayOfYear + 1);
            int pastYearDays;

            switch (firstJanuary.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    pastYearDays = 0;
                    break;
                case DayOfWeek.Tuesday:
                    pastYearDays = 1;
                    break;
                case DayOfWeek.Wednesday:
                    pastYearDays = 2;
                    break;
                case DayOfWeek.Thursday:
                    pastYearDays = 3;
                    break;
                case DayOfWeek.Friday:
                    pastYearDays = 4;
                    break;
                case DayOfWeek.Saturday:
                    pastYearDays = 5;
                    break;
                case DayOfWeek.Sunday:
                    pastYearDays = 6;
                    break;
                default:
                    pastYearDays = 0;
                    break;
            }

            DateTime weekStart = firstJanuary.AddDays(-pastYearDays);
            DateTime weekEnd = weekStart.AddDays(6);
            int currentWeekIndex = 0;
            for (int i = 1; weekStart.Year < DateTime.Today.Year + 1; i++)
            {
                _weekComboBox.Items.Add(Week.PrepareWeekString(weekStart, weekEnd) + " (" + i + ")");
                if (weekStart <= DateTime.Today && DateTime.Today <= weekEnd) currentWeekIndex = i - 1;
                weekStart = weekStart.AddDays(7);
                weekEnd = weekEnd.AddDays(7);
            }
            _weekComboBox.SelectedIndex = currentWeekIndex;
            _currentWeek = (byte)(currentWeekIndex + 1);
        }

        Quarter GetQuarter(byte day, byte quarter)
        {
            return _days[day].GetQuarter(quarter);
        }
        
        void TakeSelectedWeekDataFromDB(string[] plan, string[] report)
        {
            if (!_dbAccess.CheckWeekInDB(_currentWeek))
                _dbAccess.PrepareDefaultWeekContnent(_currentWeek);
            _dbAccess.ReadData(_currentWeek, plan, report);
        }

        public void ActualInitialisation(Grid centralGrid, ComboBox weekComboBox)
        {
            _weekGrid = centralGrid;
            _weekComboBox = weekComboBox;
            _actionHandler = ActionHandler.GetInstance();            
            _dbAccess = DBAccess.GetInstance();

            _weekGrid.MouseLeave += _actionHandler.WeekGrid_MouseLeave;

            PrepareWeekComboBox();

            //Creating "Day" variables and theirs representation in GUI. 
            string[] plan = new string[QnttOfQrtsInWeek];
            string[] report = new string[QnttOfQrtsInWeek];
            TakeSelectedWeekDataFromDB(plan, report);
            for (byte i = 0; i < 7; i++)
            {
                StackPanel dayStackPanel = new StackPanel();
                _weekGrid.Children.Add(dayStackPanel);
                Grid.SetColumn(dayStackPanel, i);
                _days[i] = new Day(dayStackPanel, _daysNames[i], plan.Skip(i * 96).Take(96).ToArray(),
                   report.Skip(i * 96).Take(96).ToArray(), i);                
            }
        }

        public void SaveData()
        {
            string[] plan = _days[0].DayPlan;
            string[] report = _days[0].DayReport;
            for (int i = 1; i < 7; i++)
            {
                plan = plan.Concat(_days[i].DayPlan).ToArray();
                report = report.Concat(_days[i].DayReport).ToArray();
            }
            _dbAccess.SaveData(_currentWeek, plan, report);
        }

        public void Update()
        {
            _currentWeek = (byte)(_weekComboBox.SelectedIndex + 1);
            string[] plan = new string[QnttOfQrtsInWeek];
            string[] report = new string[QnttOfQrtsInWeek];
            TakeSelectedWeekDataFromDB(plan, report);
            for (byte i = 0; i < 7; i++)
            {
                _days[i].DayPlan = plan.Skip(i * 96).Take(96).ToArray();
                _days[i].DayReport = report.Skip(i * 96).Take(96).ToArray();
            }
        }
    }
}
