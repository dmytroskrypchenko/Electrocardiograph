﻿namespace Electrocardiograph.Pages
{
    using System;
    using Xamarin.Forms;

    public partial class DeviceList : ContentPage
    {
        private readonly IBluetooth _bluetooth;
        private PairedDevice _selectedDevice;

        public DeviceList(IBluetooth bluetooth)
        {
            InitializeComponent();
            _bluetooth = bluetooth;

            pairedDevicesListView.ItemsSource = _bluetooth.PairedDevices;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedDevice = (PairedDevice)e.SelectedItem;
            connectButton.IsEnabled = true;
            connectButton.Text = "Connect to Device";
        }

        private void OnConnectButtonClicked(object sender, EventArgs e)
        {
            if (_bluetooth.ConnectToDevice(_selectedDevice, out string errorMessage))
            {
                var mainPage = new ChartElectrocardiograph(_bluetooth);
                Navigation.PushAsync(mainPage);
            }
            else
            {
                DisplayAlert("Error", errorMessage, "OK");
            }
        }
    }
}
