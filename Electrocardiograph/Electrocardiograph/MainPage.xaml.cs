namespace Electrocardiograph
{
    using System;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public partial class MainPage : ContentPage
    {
        private IBluetooth _bluetooth;
        private bool _isRun;
        private int _currentIteration = 0;
        private ObservableCollection<Point> _points = new ObservableCollection<Point>();

        public MainPage(IBluetooth bluetooth)
        {
            _bluetooth = bluetooth;

            InitializeComponent();
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
                //Device.BeginInvokeOnMainThread(() => startStopButton.Text = "Start");
                _bluetooth.DataReceived -= OnBluetoothDataReceived;
                return;
            }
            _isRun = true;
            _points.Clear();
            _currentIteration = 0;
            //Device.BeginInvokeOnMainThread(() => startStopButton.Text = "Stop");
            _bluetooth.DataReceived += OnBluetoothDataReceived;
        }

        private void OnBluetoothDataReceived(object sender, BluetoothDataReceivedEventArgs e)
        {
            foreach (var value in e.Values)
            {
                if (!string.IsNullOrEmpty(value) && value != "!")
                {
                    int.TryParse(value, out int parsedValue);

                    var point = new Point(_currentIteration, parsedValue);
                    Device.BeginInvokeOnMainThread(() => _points.Add(point));

                }
                _currentIteration++;
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _cardiogramChart.SaveAsImage("ChartSample.jpg");
        }
    }
}
