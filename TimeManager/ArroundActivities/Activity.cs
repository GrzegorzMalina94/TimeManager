using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeManager
{
    [Serializable] public class Activity :INotifyPropertyChanged
    {
        string _name;
        byte R, G, B;
        List<QuarterIdentifier> _planQrtsIds;
        List<QuarterIdentifier> _reportQrtsIds;


        public event PropertyChangedEventHandler PropertyChanged;

        public Activity(String name, Color color)
        {
            _planQrtsIds = new List<QuarterIdentifier>();
            _reportQrtsIds = new List<QuarterIdentifier>();
            _name = name;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
                _name = value;
            }
        }

        public SolidColorBrush SquareColor
        {
            get
            {
                Color color = Color.FromRgb(R, G, B);
                return new SolidColorBrush(color);
            }
            set
            {
                if(value != SquareColor)
                {
                    R = value.Color.R;
                    G = value.Color.G;
                    B = value.Color.B;
                    NotifyPropertyChanged();
                }                
            }
        }



        public void AssignQuarterToPlan(QuarterIdentifier qIdentifier)
        {
            _planQrtsIds.Add(qIdentifier);
        }



        public void AssignQuarterToReport(QuarterIdentifier qIdentifier)
        {
            _reportQrtsIds.Add(qIdentifier);
        }



        public void DeleteQuarterFromPlan(QuarterIdentifier qIdentifier)
        {
            _planQrtsIds.RemoveAll(qId => qId == qIdentifier);
        }



        public void DeleteQuarterFromReport(QuarterIdentifier qIdentifier)
        {
            _reportQrtsIds.RemoveAll(qId => qId == qIdentifier);
        }



        public List<QuarterIdentifier> GetIdsToPlanAssignedQuarters()
        {
            return _planQrtsIds;
        }



        public List<QuarterIdentifier> GetIdsToReportAssignedQuarters()
        {
            return _reportQrtsIds;
        }



        public int GetNumberToPlanAssignedQuarters()
        {
            return _planQrtsIds.Count();
        }



        public int GetNumberToReportAssignedQuarters()
        {
            return _reportQrtsIds.Count();
        }
    }
}
