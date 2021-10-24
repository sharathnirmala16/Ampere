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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ampere
{
    /// <summary>
    /// Interaction logic for OptionBar.xaml
    /// </summary>
    public partial class OptionBar : UserControl
    {
        private string[] arr = new string[4];
        public OptionBar()
        {
            InitializeComponent();
            arr[0] = "No Alerts";
            arr[1] = "Visual Alert";
            arr[2] = "Aural Alert";
            arr[3] = "Visual & Aural Alerts";
        }

        private void leftButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nextItem(false);
        }

        private void rightButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nextItem(true);
        }

        private void nextItem(bool right)
        {
            int curPos = CurrentPos();
            if (right == true)
            {
                if (curPos == 3) curPos = 0;
                else curPos++;
                alertLabel.Content = arr[curPos];
            }

            else
            {
                if (curPos == 0) curPos = 3;
                else curPos--;
                alertLabel.Content = arr[curPos];
            }
        }

        private int CurrentPos()
        {
            string temp = alertLabel.Content.ToString();
            int res = -1;
            for (int i = 0; i < 4; i++)
            {
                if (arr[i] == temp) res = i;
            }

            return res;
        }

        public string CurrentPosText()
        {
            return alertLabel.Content.ToString();
        }
    }
}
