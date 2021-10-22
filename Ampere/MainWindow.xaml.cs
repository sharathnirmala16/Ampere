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
using System.Windows.Threading;

namespace Ampere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.PowerStatus pwr;
        float i = 100;
        public MainWindow()
        {
            InitializeComponent();
            pwr = System.Windows.Forms.SystemInformation.PowerStatus;
            batteryBar.ProgressBarValue(GetBatteryPerc());
            DispatcherTimer mainTimer = new DispatcherTimer();
            mainTimer.Interval = TimeSpan.FromMilliseconds(50);
            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Start();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            batteryBar.ProgressBarValue(GetBatteryPerc());
        }

        private float GetBatteryPerc()
        {
            return pwr.BatteryLifePercent * 100;
        }

        private void clButtonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
        }

        private void clButtonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonM_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
        }

        private void clButtonM_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonP_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
        }

        private void clButtonP_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
