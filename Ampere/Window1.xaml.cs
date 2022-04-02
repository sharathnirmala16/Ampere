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
using System.Windows.Threading;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace Ampere
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DispatcherTimer mainTimer = new DispatcherTimer();

        string uniColor = "";
        public Window1(string color)
        {
            InitializeComponent();
            uniColor = color;
            SetColor();

            //Values that stay constant i.e Name, Designed Capacity, Full Charge Capacity, Wear Level
            GetConstantValues();

            //For values that change, like current voltage, and plotting charge and discharge rate
            mainTimer.Interval = TimeSpan.FromMilliseconds(1000);
            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Start();
        }

        private void GetConstantValues()
        {
            //BatteryStaticData->Designed Capacity & DeviceName, BatteryFullChargedCapacity->FullChargedCapacity 
            //Wear level calculated
            string data = RunScript("gwmi -Name root\\wmi BatteryStaticData DesignedCapacity, DeviceName | Out-String");
            nameLabel.Content = ObtainStringValues(data, "DeviceName:", "PSComputerName");
            designedCapLabel.Content = Convert.ToInt32(ObtainStringValues(data, "DesignedCapacity:", "DeviceName")).ToString() + " mWh";

            data = RunScript("gwmi -Name root\\wmi BatteryFullChargedCapacity FullChargedCapacity | Out-String");
            fullChargeCapLabel.Content = Convert.ToInt32(ObtainStringValues(data, "FullChargedCapacity:", "PSComputerName")).ToString() + " mWh";
            wearLabel.Content = Math.Ceiling((1 - (Convert.ToDouble(fullChargeCapLabel.Content.ToString().Replace(" mWh", "")) / Convert.ToDouble(designedCapLabel.Content.ToString().Replace(" mWh", "")))) * 100).ToString() + " %";
        }

        private string ObtainStringValues(string data, string specificName1, string specificName2)
        {
            data = data.Replace("\n", "");
            data = data.Replace(" ", "");
            int len = data.IndexOf(specificName2) - data.IndexOf(specificName1) - specificName1.Length;
            string res = "";
            try
            {
                res = data.Substring(data.IndexOf(specificName1) + specificName1.Length, len);
            }
            catch 
            {
                MessageBox.Show(specificName1 + " " + specificName2);
            }
            res = res.Replace(" ", "");
            return res;   
        }

        private string RunScript(string script)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();
            string res = "";
            foreach (PSObject psobject in results)
            {
                res += psobject.ToString();
            }
            return res;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            bool discharging;
            double voltage, chargeRate, dischargeRate;

            string data = RunScript("gwmi -Name root\\wmi BatteryStatus Voltage, Charging, ChargeRate, Discharging, DischargeRate | Out-String");
            discharging = Convert.ToBoolean(ObtainStringValues(data, "Charging:", "DischargeRate"));
            chargeRate = Convert.ToDouble(ObtainStringValues(data, "ChargeRate:", "Charging"));
            dischargeRate = Convert.ToDouble(ObtainStringValues(data, "DischargeRate:", "Discharging"));
            voltage = Convert.ToDouble(ObtainStringValues(data, "Voltage:", "PSComputerName")) / 1000;

            voltageLabel.Content = voltage.ToString() + " V";
        }

        private void SetColor()
        {
            switch (uniColor)
            {
                case "#FF0DEB00":
                    //logoBox.Source = new BitmapImage(new Uri("Logo v2G.png", UriKind.Relative));
                    Icon = new BitmapImage(new Uri("Logo v2G.png", UriKind.Relative));
                    break;
                case "#FF35A0F6":
                    //logoBox.Source = new BitmapImage(new Uri("Logo v2B.png", UriKind.Relative));
                    Icon = new BitmapImage(new Uri("Logo v2B.png", UriKind.Relative));
                    break;
                case "#FFFFD40E":
                    //logoBox.Source = new BitmapImage(new Uri("Logo v2Y.png", UriKind.Relative));
                    Icon = new BitmapImage(new Uri("Logo v2Y.png", UriKind.Relative));
                    break;
                case "#FFFF1000":
                    //logoBox.Source = new BitmapImage(new Uri("Logo v2R.png", UriKind.Relative));
                    Icon = new BitmapImage(new Uri("Logo v2R.png", UriKind.Relative));
                    break;
            }
            topBar.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            topBar.Stroke = topBar.Fill;
        }

        private void clButtonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
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
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
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
            clButtonM.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            clButtonP.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
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
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void minButtonRec_MouseLeave(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom(uniColor));
        }

        private void minButtonRec_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void minButtonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom(uniColor));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
        }

        private void minButtonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            minButtonRec.Fill = (Brush)(new BrushConverter().ConvertFrom("#FF303030"));
            minButtonGrid.Background = (Brush)(new BrushConverter().ConvertFrom(uniColor));
        }
    }
}
