﻿namespace Electrocardiograph.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public partial class ChartElectrocardiograph : ContentPage
    {
        private const int Threshold = 440;
        private const int ArduinoDelay = 10;//msec

        private readonly IBluetooth _bluetooth;
        private bool _isRun;
        private int _currentIteration = 0;
        private readonly ObservableCollection<Point> _points = new ObservableCollection<Point>();

        public ChartElectrocardiograph(IBluetooth bluetooth)
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

                _bluetooth.DataReceived -= OnBluetoothDataReceived;
                Device.BeginInvokeOnMainThread(() =>
                {
                    startStopButton.Text = "Start";
                    heartRateEntry.Text = CalculateHeartRate().ToString();
                    saveButton.IsEnabled = true;
                    sendButton.IsEnabled = true;
                });

                return;
            }
            _isRun = true;
            _points.Clear();
            _currentIteration = 0;
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    startStopButton.Text = "Stop";
            //    heartRateEntry.Text = "0";
            //    saveButton.IsEnabled = false;
            //    sendButton.IsEnabled = false;
            //});
            _bluetooth.DataReceived += OnBluetoothDataReceived;
        }

        private void OnBluetoothDataReceived(object sender, BluetoothDataReceivedEventArgs e)
        {
            foreach (var value in e.Values)
            {
                if (!string.IsNullOrEmpty(value) && value != "!")
                {
                    int.TryParse(value, out int parsedValue);

                    var point = new Point(_currentIteration * ArduinoDelay / 1000.0, parsedValue);
                    Device.BeginInvokeOnMainThread(() => _points.Add(point));

                }
                _currentIteration++;
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _cardiogramChart.SaveAsImage(GenerateFileName());
        }

        private void OnSendToClinickButtonClicked(object sender, EventArgs e)
        {
            var fileName = GenerateFileName();
            _cardiogramChart.SaveAsImage(fileName);
            var sendToClinicPage = new SendToClinic(fileName);
            Navigation.PushAsync(sendToClinicPage);
        }

        private int CalculateHeartRate()
        {
            if (_points.Count == 0)
                return 0;

            int countQRS = 0;
            for (int i = 2; i < _points.Count - 2; i++)
            {
                if (_points[i].Y >= Threshold &&
                    _points[i].Y > _points[i - 1].Y && _points[i].Y > _points[i - 2].Y &&
                    _points[i].Y > _points[i + 1].Y && _points[i].Y > _points[i + 2].Y)
                {
                    countQRS++;
                }
            }
            return (int)((60 * countQRS) / _points.Last().X);
        }

        private string GenerateFileName()
        {
            return "Electrocardiogram_" + DateTime.Now.ToString("ddMMMyyyy_HH:mm") + ".jpg";
        }
    }
}
