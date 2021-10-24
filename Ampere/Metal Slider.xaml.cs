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
    /// Interaction logic for Metal_Slider.xaml
    /// </summary>
    public partial class Metal_Slider : UserControl
    {
        bool drag = false;
        Point startPoint;
        public Metal_Slider()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag = true;
            startPoint = Mouse.GetPosition(MainRectangle);
        }

        private void slideRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true && slideRectangle.Margin.Left >= -1 && slideRectangle.Margin.Left <= 321)
            {
                Point newPoint = Mouse.GetPosition(MainRectangle);
                double right = newPoint.X - startPoint.X;
                if (right <= 320 && right >= 0) slideRectangle.Margin = new Thickness(right, 0, 0, 0);
            }
        }

        private void slideRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drag = false;
        }

        public void ResetValue()
        {
            slideRectangle.Margin = new Thickness(0, 0, 0, 0);
        }

        public double getSliderRawValue()
        {
            return slideRectangle.Margin.Left;
        }
    }
}
