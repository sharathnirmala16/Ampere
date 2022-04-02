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

namespace Ampere
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string uniColor = "";
        public Window1(string color)
        {
            InitializeComponent();
            uniColor = color;
            SetColor();
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
