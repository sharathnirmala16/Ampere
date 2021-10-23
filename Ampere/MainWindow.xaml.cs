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
        bool offlineTrigger = false;
        bool onlineTrigger = false;
        public MainWindow()
        {
            InitializeComponent();
            pwr = System.Windows.Forms.SystemInformation.PowerStatus;
            batteryBar.ProgressBarValue(GetBatteryPerc());
            DispatcherTimer mainTimer = new DispatcherTimer();
            mainTimer.Interval = TimeSpan.FromMilliseconds(1000);
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
            //int batteryTimeToCharge =
            string hours = (batteryTimeDischargeSec / 3600).ToString();
            string minutes = ((batteryTimeDischargeSec / 60) % 60).ToString();
            remainingBatteryLife(hours, minutes, batteryPluggedStatus);
        }

        private void remainingBatteryLife(string hours, string minutes, string chargingStatus)
        {
            if (chargingStatus == "Offline")
            {
                if (hours == "0" && minutes == "0")
                {
                    switch (batteryLifeLabel.Content)
                    {
                        case "Updating lifetime":
                            batteryLifeLabel.Content = "Updating lifetime.";
                            break;
                        case "Updating lifetime.":
                            batteryLifeLabel.Content = "Updating lifetime..";
                            break;
                        case "Updating lifetime..":
                            batteryLifeLabel.Content = "Updating lifetime...";
                            break;
                        case "Updating lifetime...":
                            batteryLifeLabel.Content = "Updating lifetime";
                            break;
                        default:
                            batteryLifeLabel.Content = "Updating lifetime";
                            break;
                    }
                }
                else if (hours == "0" && minutes != "0")
                {
                    if (Convert.ToInt32(minutes) > 1) batteryLifeLabel.Content = minutes + " minutes";
                    else batteryLifeLabel.Content = minutes + " minute";
                }
                else
                {
                    if (Convert.ToInt32(hours) > 1 && Convert.ToInt32(minutes) > 1) batteryLifeLabel.Content = hours + " hours, " + minutes + " minutes";
                    else if (Convert.ToInt32(hours) > 1 && Convert.ToInt32(minutes) == 1) batteryLifeLabel.Content = hours + " hours, " + minutes + " minute";
                    else if (Convert.ToInt32(hours) == 1 && Convert.ToInt32(minutes) > 1) batteryLifeLabel.Content = hours + " hour, " + minutes + " minutes";
                    else if (Convert.ToInt32(hours) == 1 && Convert.ToInt32(minutes) == 1) batteryLifeLabel.Content = hours + " hour, " + minutes + " minute";
                }
            }

            else if (chargingStatus == "Online")
            {
                if (onlineTrigger == false)
                {
                    batteryLifeLabel.Content = "Updating lifetime";
                    onlineTrigger = true;
                    offlineTrigger = false;
                }
                switch (batteryLifeLabel.Content)
                {
                    case "Charging":
                        batteryLifeLabel.Content = "Charging.";
                        break;
                    case "Charging.":
                        batteryLifeLabel.Content = "Charging..";
                        break;
                    case "Charging..":
                        batteryLifeLabel.Content = "Charging...";
                        break;
                    case "Charging...":
                        batteryLifeLabel.Content = "Charging";
                        break;
                    default:
                        batteryLifeLabel.Content = "Charging";
                        break;
                }
            }
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

        private void HideWhenMinimized()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }
    }
}
