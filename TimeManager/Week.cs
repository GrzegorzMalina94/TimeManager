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
        private static Week _instance;
        public static int QnttOfQrtsInWeek = 672;
        Grid _weekGrid;
        Day[] _days = new Day[7];
        string[] _daysNames = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        ActionHandler _actionHandler;
        DB.Access _dbAccess;
        
        public static Week GetInstance()
        {
            if (_instance == null)
            {
                return new Week();
            }
            else return _instance;
        }

        private Week()
        {
                  
        }

        public void ActualInitialisation(Grid centralGrid)
        {
            _weekGrid = centralGrid;
            _actionHandler = ActionHandler.GetInstance();
            _weekGrid.MouseLeave += _actionHandler.WeekGrid_MouseLeave;
            _dbAccess = DB.Access.GetInstance();

            //Creating "Day" variables and theirs representation in GUI. 
            string[] plan = new string[QnttOfQrtsInWeek];
            string[] report = new string[QnttOfQrtsInWeek];
            _dbAccess.ReadData(plan, report);
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
            string[] plan = _days[0].GetDayPlan();
            string[] report = _days[0].GetDayReport();
            for (int i = 1; i < 7; i++)
            {
                plan = plan.Concat(_days[i].GetDayPlan()).ToArray();
                report = report.Concat(_days[i].GetDayReport()).ToArray();
            }
            _dbAccess.SaveData(plan, report);
        }

        Quarter GetQuarter(byte day, byte quarter)
        {
            return _days[day].GetQuarter(quarter);
        }

        public void NullAssigment(QrtrsMrkngMode markingMode, List<QuarterIdentifier> qrtsIds)
        {
            foreach(QuarterIdentifier qrtId in qrtsIds)
            {
                _days[qrtId.DayNumber].NullAssigment(markingMode, qrtId.QuarterNumber);
            }
        }
    }
}
