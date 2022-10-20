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
    /// Logika interakcji dla klasy FillingWindow.xaml
    /// </summary>
    public partial class FillingWindow : Window
    {
        public FillingWindow()
        {
            InitializeComponent();
        }

        private void CbTimeBeginning_Loaded(object sender, RoutedEventArgs e)
        {
            var lQrtDscrpts = new List<string>();

            for (int i = 1; i <= 96; i++)
                lQrtDscrpts.Add(VariousTools.GiveQuarterDescription(i));

            cbTimeBeginning.ItemsSource = lQrtDscrpts;
        }
    }
}
