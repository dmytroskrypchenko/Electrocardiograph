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

        private int _currentIteration = 0;
        private ObservableCollection<Point> _points;
        private bool _isRun;

        private SfChart _cardiogramChart;
        private Button _startStopButton;

        private Timer _timer;

        Random random = new Random();

        public MainPage()
        {
            _timer = new Timer(TimeSpan.FromMilliseconds(10), AddPoint);

            _points = new ObservableCollection<Point>();

            _cardiogramChart = new SfChart();
            _cardiogramChart.HorizontalOptions = LayoutOptions.FillAndExpand;
            _cardiogramChart.VerticalOptions = LayoutOptions.FillAndExpand;

            var mainStackLayout = new StackLayout();
            mainStackLayout.Orientation = StackOrientation.Horizontal;
            mainStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            mainStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            var xAxis = new NumericalAxis();
            xAxis.AutoScrollingDelta = 10;
            xAxis.Title.Text = "Time, s";
            _cardiogramChart.PrimaryAxis = xAxis;

            var yAxis = new NumericalAxis();
            yAxis.Minimum = 200;
            yAxis.Maximum = 550;
            _cardiogramChart.SecondaryAxis = yAxis;

            var series = new LineSeries();
            series.ItemsSource = _points;
            series.Color = Color.Red;
            series.XBindingPath = nameof(Point.X);
            series.YBindingPath = nameof(Point.Y);
            _cardiogramChart.Series.Add(series);
            _cardiogramChart.ChartBehaviors.Add(new ChartZoomPanBehavior { EnablePanning = true, EnableZooming = false, EnableDoubleTap = false });

            _startStopButton = new Button();
            _startStopButton.Text = "Start";
            _startStopButton.Clicked += OnStartStopButtonClicked;

            var saveButton = new Button();
            saveButton.Text = "Save as JPG";
            saveButton.Clicked += OnSaveButtonClicked;

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

            var heartRateEntry = new Entry();
            heartRateEntry.Text = "88";
            heartRateEntry.VerticalOptions = LayoutOptions.Start;
            heartRateEntry.IsEnabled = false;

            var heartRateLayout = new StackLayout();
            heartRateLayout.Orientation = StackOrientation.Horizontal;
            heartRateLayout.Children.Add(heartRateLabel);
            heartRateLayout.Children.Add(heartRateEntry);
            heartRateLayout.Children.Add(bpmLabel);

            var buttonsLayout = new StackLayout();
            buttonsLayout.Orientation = StackOrientation.Vertical;
            buttonsLayout.Children.Add(_startStopButton);
            buttonsLayout.Children.Add(saveButton);

            var controlFrame = new Frame();
            controlFrame.OutlineColor = Color.Accent;
            controlFrame.Content = buttonsLayout;

            var resultsFrame = new Frame();
            resultsFrame.OutlineColor = Color.Accent;
            resultsFrame.Content = heartRateLayout;

            var rightPanelLayout = new StackLayout();
            rightPanelLayout.Orientation = StackOrientation.Vertical;
            rightPanelLayout.Children.Add(controlLabel);
            rightPanelLayout.Children.Add(controlFrame);
            rightPanelLayout.Children.Add(resultsLabel);
            rightPanelLayout.Children.Add(resultsFrame);

            mainStackLayout.Children.Add(_cardiogramChart);
            mainStackLayout.Children.Add(rightPanelLayout);

            Content = mainStackLayout;
            Padding = new Thickness(15);

        }

        private void OnStartStopButtonClicked(object sender, EventArgs e)
        {
            if (_isRun)
            {
                _isRun = false;
                _timer.Stop();
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    Task.Delay(4000).Wait();
                //    _startStopButton.Text = "Start";
                //});

                return;
            }
            _isRun = true;
            _points.Clear();
            _currentIteration = 0;
            Device.BeginInvokeOnMainThread(() => _startStopButton.Text = "Stop");

            _timer.Start();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _cardiogramChart.SaveAsImage("ChartSample.jpg");
        }

        private void AddPoint()
        {

            _points.Add(new Point(_currentIteration, random.Next(200, 550)));
            _currentIteration++;

        }
    }
}
