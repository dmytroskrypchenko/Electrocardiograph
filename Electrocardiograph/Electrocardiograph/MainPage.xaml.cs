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
            cardiogramChart.HorizontalOptions = LayoutOptions.FillAndExpand;
            cardiogramChart.VerticalOptions = LayoutOptions.FillAndExpand;

            var mainStackLayout = new StackLayout();
            mainStackLayout.Orientation = StackOrientation.Horizontal;
            mainStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            mainStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            var xAxis = new NumericalAxis();
            xAxis.AutoScrollingDelta = 10;
            cardiogramChart.PrimaryAxis = xAxis;

            var yAxis = new NumericalAxis();
            yAxis.Minimum = 200;
            yAxis.Maximum = 550;
            cardiogramChart.SecondaryAxis = yAxis;

            var series = new LineSeries();
            series.ItemsSource = data;
            series.Color = Color.Red;
            series.XBindingPath = nameof(Point.X);
            series.YBindingPath = nameof(Point.Y);
            cardiogramChart.Series.Add(series);
            cardiogramChart.ChartBehaviors.Add(new ChartZoomPanBehavior { EnablePanning = true, EnableZooming = false, EnableDoubleTap = false });
            LoadData();

            var startStopButton = new Button();
            startStopButton.Text = "Start";

            var controlLabel = new Label();
            controlLabel.Text = "Control:";

            var resultsLabel = new Label();
            resultsLabel.Text = "Results:";

            var heartRateLabel = new Label();
            heartRateLabel.Text = "Heart rate:";
            heartRateLabel.VerticalOptions = LayoutOptions.Center;

            var bpmLabel = new Label();
            bpmLabel.Text = "BPM";
            bpmLabel.VerticalOptions = LayoutOptions.Center;

            var heartRateEntry= new Entry();
            heartRateEntry.Text = "88";
            heartRateEntry.VerticalOptions = LayoutOptions.Start;
            heartRateEntry.IsEnabled = false;

            var heartRateLayout = new StackLayout();
            heartRateLayout.Orientation = StackOrientation.Horizontal;
            heartRateLayout.Children.Add(heartRateLabel);
            heartRateLayout.Children.Add(heartRateEntry);
            heartRateLayout.Children.Add(bpmLabel);

            var controlFrame = new Frame();
            controlFrame.OutlineColor = Color.Accent;
            controlFrame.Content = startStopButton;
           
            var resultsFrame = new Frame();
            resultsFrame.OutlineColor = Color.Accent;
            resultsFrame.Content = heartRateLayout;
            
            var rightPanelLayout = new StackLayout();
            rightPanelLayout.Orientation = StackOrientation.Vertical;
            rightPanelLayout.Children.Add(controlLabel);
            rightPanelLayout.Children.Add(controlFrame);
            rightPanelLayout.Children.Add(resultsLabel);
            rightPanelLayout.Children.Add(resultsFrame);

            mainStackLayout.Children.Add(cardiogramChart);
            mainStackLayout.Children.Add(rightPanelLayout);

            Content = mainStackLayout;
            Padding = new Thickness(15);
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
