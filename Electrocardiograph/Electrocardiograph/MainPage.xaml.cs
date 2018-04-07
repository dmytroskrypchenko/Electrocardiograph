using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Electrocardiograph
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Point> data;
        int i = 0;
        Random random = new Random();

        public MainPage()
        {
            var cardiogramChart = new SfChart();
            data = new ObservableCollection<Point>();
            cardiogramChart.VerticalOptions = LayoutOptions.FillAndExpand;

            var stackLayout = new StackLayout();
            stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            var xAxis = new NumericalAxis();
            xAxis.AutoScrollingDelta = 10;
            cardiogramChart.PrimaryAxis = xAxis;

            var yAxis = new NumericalAxis();
            yAxis.Minimum = 200;
            yAxis.Maximum = 550;
            cardiogramChart.SecondaryAxis = yAxis;

            LineSeries series = new LineSeries();
            series.ItemsSource = data;
            series.Color = Color.Red;
            series.XBindingPath = nameof(Point.X);
            series.YBindingPath = nameof(Point.Y);
            cardiogramChart.Series.Add(series);
            cardiogramChart.ChartBehaviors.Add(new ChartZoomPanBehavior { EnablePanning = true, EnableZooming = false, EnableDoubleTap = false });
            LoadData();
            stackLayout.Children.Add(cardiogramChart);
            Content = stackLayout;
        }

        public void LoadData()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 10), () =>
            {
                data.Add(new Point(i, random.Next(200, 550)));
                i++;
                return true;
            });
        }
    }
}
