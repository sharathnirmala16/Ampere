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
using System.Speech.Synthesis;
using System.Diagnostics;
using System.IO;

namespace Ampere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.PowerStatus pwr;
        SpeechSynthesizer sys = new SpeechSynthesizer();

        bool offlineTrigger = false;
        bool onlineTrigger = false;
        bool prevPwrLineStatus = false;

        DispatcherTimer mainTimer = new DispatcherTimer();
        DispatcherTimer secondTimer = new DispatcherTimer();

        PerformanceCounter cpuUsageCounter;
        PerformanceCounter ramUsageCounter;

        int time10Count = 0;

        public MainWindow()
        {
            InitializeComponent();

            pwr = System.Windows.Forms.SystemInformation.PowerStatus;
            batteryBar.ProgressBarValue(GetBatteryPerc());  
            
            mainTimer.Interval = TimeSpan.FromMilliseconds(1000);
            secondTimer.Interval = TimeSpan.FromMilliseconds(1000);

            mainTimer.Tick += MainTimer_Tick;
            secondTimer.Tick += SecondTimer_Tick;

            mainTimer.Start();

            cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            ramUsageCounter = new PerformanceCounter("Memory", "% Committed Bytes in Use");
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            float batteryPerc = GetBatteryPerc();
            batteryBar.ProgressBarValue(batteryPerc);
            percLabel.Content = ((int)batteryPerc).ToString() + "%";
            string batteryPluggedStatus = pwr.PowerLineStatus.ToString();

            int batteryTimeDischargeSec = pwr.BatteryLifeRemaining; //THIS NEEDS TO BE ADDED IN AN UPDATE (ML FEATURES)
            //int batteryTimeToCharge = THIS NEEDS TO BE ADDED IN AN UPDATE (ML FEATURES)
            string hours = (batteryTimeDischargeSec / 3600).ToString();
            string minutes = ((batteryTimeDischargeSec / 60) % 60).ToString();
            remainingBatteryLife(hours, minutes, batteryPluggedStatus);

            //Code for warning the user
            string alertText = optionsBar.CurrentPosText();
            double maxValue = maxBatterySlider.getSliderRawValue();
            double minValue = minBatterySlider.getSliderRawValue();
            maxValue = ConvertToSliderVal(maxValue) + 80;
            minValue = ConvertToSliderVal(minValue) + 10;
            int tempIndex = percLabel.Content.ToString().IndexOf("%");
            int currentValue = Convert.ToInt32(percLabel.Content.ToString().Remove(tempIndex, 1));

            switch (alertText)
            {
                case "No Alerts":
                    break;
                case "Visual Alert":
                    if (currentValue >= (int)maxValue && batteryPluggedStatus == "Online")
                    {
                        this.Show();
                        ActivateAlertTimer(true);
                    }
                    else if (currentValue <= (int)minValue && batteryPluggedStatus == "Offline")
                    {
                        this.Show();
                        ActivateAlertTimer(false);
                    }
                    break;
                case "Aural Alert":
                    if (currentValue >= (int)maxValue && batteryPluggedStatus == "Online")
                    {
                        sys.Speak("Your battery has charged up to " + currentValue.ToString() + " percent.");
                        ActivateAlertTimer(true);
                    }
                    else if (currentValue <= (int)minValue && batteryPluggedStatus == "Offline")
                    {
                        sys.Speak("Your battery has charged up to " + currentValue.ToString() + " percent.");
                        ActivateAlertTimer(false);
                    }
                    break;
                case "Visual & Aural Alerts":
                    if (currentValue >= (int)maxValue && batteryPluggedStatus == "Online")
                    {
                        this.Show();
                        sys.Speak("Your battery has charged up to " + currentValue.ToString() + " percent.");
                        ActivateAlertTimer(true);
                    }
                    else if (currentValue <= (int)minValue && batteryPluggedStatus == "Offline")
                    {
                        this.Show();
                        sys.Speak("Your battery has charged up to " + currentValue.ToString() + " percent.");
                        ActivateAlertTimer(false);
                    }
                    break;
            }

            string curDateTime = DateTime.Now.ToString();
            LogDataPoint(batteryPerc, cpuUsageCounter.NextValue(), ramUsageCounter.NextValue(), batteryPluggedStatus, batteryTimeDischargeSec, curDateTime);
        }

        private void ActivateAlertTimer(bool prevPLS)
        {
            prevPwrLineStatus = prevPLS;
            mainTimer.Stop();
            secondTimer.Start();
        }

        private void LogDataPoint(float batteryPerc, float cpuUsageP, float ramUsageP, string batteryPluggedStatus, int windowsBatteryTimePred, string curDateTime)
        {
            //LOGS THE DATA EVERY 10 SECONDS
            if (time10Count < 10) time10Count++;
            else if (time10Count == 10)
            {
                time10Count = 0;
                //Filepath must change when installed automatically
                string fileName = "D:\\Computer Programming\\Programs\\C#\\Ampere Battery Indicator\\Ampere\\Data\\system_usage_data.csv";
                string dataLog = batteryPerc.ToString() + "," + cpuUsageP.ToString() + "," + ramUsageP.ToString() + "," + batteryPluggedStatus + "," +
                                 windowsBatteryTimePred.ToString() + "," + curDateTime + "\n";

                if (File.Exists(fileName) == false)
                {
                    string dataHead = "Battery %,CPU %, RAM %, PLS, WBTP, DateTime" + Environment.NewLine;
                    File.WriteAllText(fileName, dataHead);
                }

                File.AppendAllText(fileName, dataLog);
            }
        }

        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            string batteryPluggedStatus = pwr.PowerLineStatus.ToString();
            bool curPluggedStatus = false;
            if (batteryPluggedStatus == "Online") curPluggedStatus = true;

            if (curPluggedStatus != prevPwrLineStatus)
            {
                secondTimer.Stop();
                mainTimer.Start();
                prevPwrLineStatus = curPluggedStatus;
            }
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
            //this.Hide();
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

        private double ConvertToSliderVal(double rawValue)
        {
            return rawValue/320;
        }

        private void maxBatterySlider_MouseEnter(object sender, MouseEventArgs e)
        {
            double rawValue = maxBatterySlider.getSliderRawValue();
            rawValue = ConvertToSliderVal(rawValue);

            maxChargeLabel.Content = "Maximum Charge Alert: " + (int)(80 + (20 * rawValue)) + "%";
        }

        private void minBatterySlider_MouseEnter(object sender, MouseEventArgs e)
        {
            double rawValue = minBatterySlider.getSliderRawValue();
            rawValue = ConvertToSliderVal(rawValue);

            minChargeLabel.Content = "Minimum Charge Alert: " + (int)(10 + (20 * rawValue)) + "%";
        }

        private void maxBatterySlider_MouseLeave(object sender, MouseEventArgs e)
        {
            double rawValue = maxBatterySlider.getSliderRawValue();
            rawValue = ConvertToSliderVal(rawValue);

            maxChargeLabel.Content = "Maximum Charge Alert: " + (int)(80 + (20 * rawValue)) + "%";
        }

        private void minBatterySlider_MouseLeave(object sender, MouseEventArgs e)
        {
            double rawValue = minBatterySlider.getSliderRawValue();
            rawValue = ConvertToSliderVal(rawValue);

            minChargeLabel.Content = "Minimum Charge Alert: " + (int)(10 + (20 * rawValue)) + "%";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (testButton)
        }

        private void optionsBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void logoRec_MouseEnter(object sender, MouseEventArgs e)
        {
            logoLabel.Content = "nalytics";
        }

        private void logoRec_MouseLeave(object sender, MouseEventArgs e)
        {
            logoLabel.Content = "mpere";
        }
    }
}
