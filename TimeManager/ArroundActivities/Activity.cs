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

        public event PropertyChangedEventHandler PropertyChanged;

        public Activity(String name, Color color)
        {
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
    }
}
