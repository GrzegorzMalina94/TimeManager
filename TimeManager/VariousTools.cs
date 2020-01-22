using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager
{
    class VariousTools
    {
        private VariousTools()
        {

        }

        public static short CalculateNumberOfDaysInMonth(short month)
        {
            switch(month)
            {
                case 1:
                    {
                        return 31;                        
                    }
                case 2:
                    {
                        if ((DateTime.Today.Year % 4) == 0)
                            return 29;
                        else
                            return 28;
                    }
                case 3:
                    {
                        return 31;
                    }
                case 4:
                    {
                        return 30;
                    }
                case 5:
                    {
                        return 31;
                    }
                case 6:
                    {
                        return 30;
                    }
                case 7:
                    {
                        return 31;
                    }
                case 8:
                    {
                        return 31;
                    }
                case 9:
                    {
                        return 30;
                    }
                case 10:
                    {
                        return 31;
                    }
                case 11:
                    {
                        return 30;
                    }
                case 12:
                    {
                        return 31;
                    }
                default:
                    {
                        return 31;
                    }
            }
        }

        /// <summary>
        /// Method calculates number of days of first week of specified month or of current year which belongs to the 
        /// previous period (month or year). If action concern month or year - that depends on the first argument.
        /// Action will always concern current year or months of current year - there is not possibility to calculate
        /// said number of days for another year or for month of another year.
        /// </summary>
        /// <param name="period">Specifies period which action will concern. It can be only month or year.</param>
        /// <param name="month">Specifies the month for which the number (which number - see in main description) will
        /// be calculated.</param>
        /// <returns></returns>
        public static byte FirstWeekDaysInPastPeriod(Period period, byte month = 0)
        {
            if (period != Period.Year && period != Period.Month)
            {
                throw new ArgumentException("Period argument must be Period.Month or Period.Year.", "period");
            }

            DateTime firstDay = DateTime.Today;
            if (period == Period.Year)
                firstDay = firstDay.AddDays(-DateTime.Today.DayOfYear + 1);
            else if (period == Period.Month)
                firstDay = new DateTime(firstDay.Year, month, 1);

            switch (firstDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }

        public static int NumberOfFirstQuarterOfFirstWeekInMonth(byte month)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            short daysOffset = (short)(firstDayOfMonth.DayOfYear - 1 +
                FirstWeekDaysInPastPeriod(Period.Year) -
                FirstWeekDaysInPastPeriod(Period.Month, month));
            return daysOffset * 96;            
        }
    }
}
