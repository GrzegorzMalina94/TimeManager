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
        Grid _weekGrid;
        Day[] _days = new Day[7];
        string[] _daysNames = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        ActionHandler _actionHandler;
        DB.Access _dbAccess;
                
        /// <summary>
        /// Initialize reference to the grid, which contains plan for all week. Fullfill week with days.
        /// </summary>
        /// <param name="centralGrid">Must be one-row, seven-columns grid (one column per one day).</param>
        public Week(Grid centralGrid)
        {
            _weekGrid = centralGrid;
            _actionHandler = ActionHandler.GetInstance();
            _weekGrid.MouseLeave += _actionHandler.WeekGrid_MouseLeave;
            _dbAccess = DB.Access.GetInstance();

            //Creating "Day" variables and theirs representation in GUI. 
            string[] loadedData = _dbAccess.ReadData();
            for (int i = 0; i < 7; i++)
            {
                StackPanel dayStackPanel = new StackPanel();
                _weekGrid.Children.Add(dayStackPanel);
                Grid.SetColumn(dayStackPanel, i);
                if(loadedData.Length == 672)  //Execute when CORRECT number of records was taken from database.
                    _days[i] = new Day(dayStackPanel, _daysNames[i], loadedData.Skip(i*96).Take(96).ToArray());
                else                          //Execute  when INCORRECT number of records was taken from database.
                {
                    _days[i] = new Day(dayStackPanel, _daysNames[i], null);
                }
            }
            
            
        }
        
        public void SaveData()
        {
            string[] dataToSave = _days[0].GetDayPlan();
            for(int i = 1; i < 7; i++)
            {
                dataToSave = dataToSave.Concat(_days[i].GetDayPlan()).ToArray();
            }
            _dbAccess.SaveData(dataToSave);
        }
    }
}
