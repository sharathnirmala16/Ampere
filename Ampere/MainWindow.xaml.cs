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
            float batteryPerc = GetBatteryPerc();
            batteryBar.ProgressBarValue(batteryPerc);
            percLabel.Content = ((int)batteryPerc).ToString() + "%";
            string batteryPluggedStatus = pwr.PowerLineStatus.ToString();

            int batteryTimeDischargeSec = pwr.BatteryLifeRemaining;
            string hours = (batteryTimeDischargeSec / 3600).ToString();
            string minutes = ((batteryTimeDischargeSec / 60) % 60).ToString();
            remainingBatteryLife(hours, minutes, batteryPluggedStatus);
        }

        private void remainingBatteryLife(string hours, string minutes, string chargingStatus)
        {
            if (chargingStatus == "Offline")
            {
                batteryLifeLabel.Content = hours + "hr, " + minutes + "min";
            }

            /*else if (chargingStatus == "Online")
            {
                batteryLifeLabel.Content = hours + "hr, " + minutes + "min";
            }*/
        }

        private float GetBatteryPerc()
        {
            return pwr.BatteryLifePercent * 100;
        }

        private void clButtonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("Transparent"));
        }

        private void clButtonM_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonM_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("Transparent"));
        }

        private void clButtonP_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void clButtonP_MouseLeave(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            clButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("Transparent"));
        }

        private void clButtonGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void minButtonGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void minButtonRec_MouseEnter(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void minButtonRec_MouseLeave(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
        }

        private void minButtonRec_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void minButtonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void minButtonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#ffd40e"));
        }
    }
}
