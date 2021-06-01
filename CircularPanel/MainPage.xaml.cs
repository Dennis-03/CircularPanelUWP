using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CircularPanel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            SecondHandRotate.Angle = (DateTime.Now.Second * 6) + (DateTime.Now.Millisecond * .006);
            MinuteHandRotate.Angle = (DateTime.Now.Minute * 6) + (DateTime.Now.Second * .10);
            HourHandRotate.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * .2);
        }
    }
    public class Circular : Panel
    {
        int childrenCount, size;
        double cellWidth, cellHeight;
        protected override Size MeasureOverride(Size availableSize)
        {
            childrenCount = Children.Count;
            size = childrenCount / 2;
            cellWidth = availableSize.Width / size;
            cellHeight = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height / size;

            foreach (UIElement child in Children)
            {
                child.Measure(new Size(cellWidth, cellHeight));
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double center, x, y, angle = 0, diffAngle = 360 / childrenCount, radians;
            center = (finalSize.Width > finalSize.Height) ? finalSize.Height / 2 : finalSize.Width / 2;
            foreach (UIElement child in Children)
            {
                angle += diffAngle;
                radians = Math.PI * (angle) / 180;
                x = (center - child.DesiredSize.Width) * Math.Sin(radians) + center;
                y = Math.Abs((center - child.DesiredSize.Height) * Math.Cos(radians) - center);
                Point point = new Point(x, y);
                child.Arrange(new Rect(point, child.DesiredSize));
            }
            finalSize.Height = center * 2 + Children[0].DesiredSize.Height;
            finalSize.Width = center * 2 + Children[0].DesiredSize.Width;
            return finalSize;
        }
    }
}
