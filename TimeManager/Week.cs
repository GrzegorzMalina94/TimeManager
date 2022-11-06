using System;
using System.Linq;
using System.Windows.Controls;

namespace TimeManager
{
    #region Enums

    public enum Period
    {
        Day = 0,
        Week = 1,
        Month = 2,
        Year = 3
    }

    #endregion

    public class Week
    {
        #region Fields

        static Week _instance;
        ActionHandler _actionHandler;
        byte _crrntlChosenWeek;
        ComboBox _weekComboBox;
        Day[] _days = new Day[7];
        DBAccess _dbAccess;
        Grid _weekGrid;
        string[] _daysNames = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public static int QnttOfQrtsInWeek = 672;

        #endregion

        #region Properties

        public byte CurrentlyDisplayedWeek
        {
            get { return _crrntlChosenWeek; }
        }

        #endregion

        #region Static methods (except GetInstance)

        public static void PrepareWeekComboBox(ComboBox weekComboBox, Period period, byte month = 0)
        {
            if (period != Period.Year && period != Period.Month)
            {
                throw new ArgumentException("Period argument must be Period.Month or Period.Year.", "period");
            }

            DateTime firstDay;
            DateTime endLoopDate;
            int pastPeriodDays = VariousTools.FirstWeekDaysInPastPeriod(period, month);

            if (period == Period.Year)
            {
                firstDay = new DateTime(DateTime.Today.Year, 1, 1);
                endLoopDate = new DateTime(DateTime.Today.Year + 1, 1, 1);
            }
            else
            {
                firstDay = new DateTime(DateTime.Today.Year, month, 1);
                endLoopDate = new DateTime(DateTime.Today.Year, month + 1, 1);
            }
            
            DateTime weekStart = firstDay.AddDays(-pastPeriodDays);
            DateTime weekEnd = weekStart.AddDays(6);            
            int currentWeekIndex = 0;
            weekComboBox.Items.Clear();

            for (byte i = 1; weekStart < endLoopDate; i++)
            {
                weekComboBox.Items.Add(Week.PrepareWeekString(weekStart, weekEnd, i));
                if (weekStart <= DateTime.Today && DateTime.Today <= weekEnd) currentWeekIndex = i - 1;
                weekStart = weekStart.AddDays(7);
                weekEnd = weekEnd.AddDays(7);
            }
            weekComboBox.SelectedIndex = currentWeekIndex;
        }

        public static String PrepareWeekString(DateTime weekStart, DateTime weekEnd, byte weekNumber)
        {
            string weekStartStr = weekStart.ToShortDateString().Remove(0, 5);
            string weekEndStr = weekEnd.ToShortDateString().Remove(0, 5);
            string weekNumberStr = "(" + weekNumber + ")";
            weekStartStr = weekStartStr.Replace('-', '.');
            weekEndStr = weekEndStr.Replace('-', '.');            
            return weekStartStr + " - " + weekEndStr + " " + weekNumberStr;
        }

        #endregion

        #region ctor + GetInstance

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

        #endregion

        #region Methods

        void TakeSelectedWeekDataFromDB(out string[] plan, out string[] report)
        {
            TakeDataFromDbOfWeekIndicatedByParameter(_crrntlChosenWeek, out plan, out report);
        }

        public void TakeDataFromDbOfWeekIndicatedByParameter(int indicatedWeek, out string[] plan, out string[] report)
        {
            plan = new string[QnttOfQrtsInWeek];
            report = new string[QnttOfQrtsInWeek];
            if (!_dbAccess.CheckWeekInDB(indicatedWeek))
                _dbAccess.PrepareDefaultWeekContnent(indicatedWeek);
            _dbAccess.ReadData(indicatedWeek, plan, report);
        }

        public void ActualInitialisation(Grid centralGrid, ComboBox weekComboBox)
        {
            _weekGrid = centralGrid;
            _weekComboBox = weekComboBox;
            _actionHandler = ActionHandler.GetInstance();
            _dbAccess = DBAccess.GetInstance();

            _weekGrid.MouseLeave += _actionHandler.WeekGrid_MouseLeave;

            PrepareWeekComboBox(_weekComboBox, Period.Year);
            _crrntlChosenWeek = (byte)(_weekComboBox.SelectedIndex + 1);

            //Creating "Day" variables and theirs representation in GUI.
            string[] plan, report;
            TakeSelectedWeekDataFromDB(out plan, out report);
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
            _dbAccess.SaveData(_crrntlChosenWeek, plan, report);
        }

        public void UpdateView()
        {
            _crrntlChosenWeek = (byte)(_weekComboBox.SelectedIndex + 1);
            string[] plan, report;
            TakeSelectedWeekDataFromDB(out plan,out report);
            for (byte i = 0; i < 7; i++)
            {
                _days[i].DayPlan = plan.Skip(i * 96).Take(96).ToArray();
                _days[i].DayReport = report.Skip(i * 96).Take(96).ToArray();
            }
        }

        #endregion
    }
}
