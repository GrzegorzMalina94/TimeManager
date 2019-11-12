using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager
{
    [Serializable] public class QuarterIdentifier
    {
        public byte DayNumber { get; set; }
        public byte QuarterNumber { get; set; }

        public QuarterIdentifier(byte dayNumber, byte quarterNumber)
        {
            DayNumber = dayNumber;
            QuarterNumber = quarterNumber;
        }
    }
}
