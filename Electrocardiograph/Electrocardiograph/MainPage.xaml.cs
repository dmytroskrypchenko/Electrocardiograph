namespace Electrocardiograph
{
    using System;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public partial class MainPage : ContentPage
    {
        private int _currentIteration = 0;
        private ObservableCollection<Point> _points = new ObservableCollection<Point>();
        private bool _isRun;

        private Timer _timer;

        Random random = new Random();

        public MainPage()
        {
            InitializeComponent();

            _timer = new Timer(TimeSpan.FromMilliseconds(10), AddPoint);

            series.ItemsSource = _points;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "setLandScape");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
        }

        private void OnStartStopButtonClicked(object sender, EventArgs e)
        {
            if (_isRun)
            {
                _isRun = false;
                _timer.Stop();
                Device.BeginInvokeOnMainThread(() => startStopButton.Text = "Start");
                return;
            }
            _isRun = true;
            _points.Clear();
            _currentIteration = 0;
            //Device.BeginInvokeOnMainThread(() => startStopButton.Text = "Stop");

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
