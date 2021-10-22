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
    /// Interaction logic for BatteryBar.xaml
    /// </summary>
    public partial class BatteryBar : UserControl
    {
        System.Windows.Forms.PowerStatus pwr;
        public BatteryBar()
        {
            InitializeComponent();
            pwr = System.Windows.Forms.SystemInformation.PowerStatus;
        }

        public void ProgressBarValue(float value)
        {
            ProgressValue.Height = 243 * (value / 100);

            if (value > 50)
            {
                ProgressValue.Fill = (Brush)(new BrushConverter().ConvertFrom("#35a0f6"));
                blurEffect.Color = Color.FromRgb(53, 160, 246); 
            }
            else if (value <= 50 && value >= 15)
            {
                ProgressValue.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
                blurEffect.Color = Color.FromRgb(255, 212, 14);
            }
            else if (value <= 15 && value >= 0)
            {
                ProgressValue.Fill = (Brush)(new BrushConverter().ConvertFrom("#ff1000"));
                blurEffect.Color = Color.FromRgb(255, 16, 0);
            }

            if (pwr.PowerLineStatus.ToString() == "Online")
            {
                chargingSymbol.Visibility = Visibility.Visible;
            }
            else if (pwr.PowerLineStatus.ToString() == "Offline")
            {
                chargingSymbol.Visibility = Visibility.Hidden;
            }
        }
    }
}
