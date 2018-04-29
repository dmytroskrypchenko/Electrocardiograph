namespace Electrocardiograph.Droid
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Android.Bluetooth;

    public class BluetoothService : IBluetooth
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived = delegate { };

        public BluetoothConnectionStatus ConnectionStatus { get; set; }
        public BluetoothAdapter BluetoothAdapter { get; set; }
        public BluetoothSocket BluetoothSocket { get; set; }
        public BluetoothDevice CurrentBluetoothDevice { get; set; }
        public List<PairedDevice> PairedDevices { get; set; }

        public BluetoothService()
        {
            BluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (BluetoothAdapter == null)
            {
                ConnectionStatus = BluetoothConnectionStatus.NotAvailable;
                return;
            }
            ConnectionStatus = BluetoothConnectionStatus.None;

            RefreshBondedDeviceNames();
        }

        protected void RefreshBondedDeviceNames()
        {
            PairedDevices = new List<PairedDevice>();

            foreach (BluetoothDevice device in BluetoothAdapter.BondedDevices)
            {
                PairedDevices.Add(new PairedDevice(device.Name, device.Address));
            }
            PairedDevices.Add(new PairedDevice("Test", "rererer"));
            PairedDevices.Add(new PairedDevice("Test2", "dsdsdsd"));
        }

        public bool ConnectToDevice(PairedDevice device, out string errorMessage)
        {
            ConnectionStatus = BluetoothConnectionStatus.None;
            errorMessage = "";

            // try and get the device by name
            CurrentBluetoothDevice = FindDevice(device.Address);
            if (CurrentBluetoothDevice == null)
            {
                errorMessage = "Could not locate device: " + device.Name;
                return false;
            }

            BluetoothSocket = CurrentBluetoothDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));

            // cancel discovery because it slows down the connection and may cause it to fail altogether
            BluetoothAdapter.CancelDiscovery();

            bool connected = false;
            try
            {
                ConnectionStatus = BluetoothConnectionStatus.Connecting;
                BluetoothSocket.Connect();
                connected = true;
            }
            catch (Java.IO.IOException connectException)
            {
                errorMessage = "Connection Failed:" + connectException;
                return false;
            }
            catch (Exception connectException)
            {
                errorMessage = "Connection Failed:" + connectException;
                return false;
            }
            ConnectionStatus = BluetoothConnectionStatus.Connected;

            StartListeningForDataFromBluetoothDevice();
            ConnectionStatus = BluetoothConnectionStatus.ConnectedAndListening;

            return connected;
        }

        protected void StartListeningForDataFromBluetoothDevice()
        {
            if (ConnectionStatus != BluetoothConnectionStatus.Connected && ConnectionStatus != BluetoothConnectionStatus.ConnectedAndListening)
            {
                Console.WriteLine("Can't listen to bluetooth device, not connected");
                return;
            }

            if (BluetoothSocket.InputStream.CanRead)
            {
                new Thread(new ThreadStart(() =>
                {
                    //todo:implement read
                })).Start();
            }
        }

        private BluetoothDevice FindDevice(string deviceAddress)
        {
            foreach (BluetoothDevice device in BluetoothAdapter.BondedDevices)
            {
                if (device.Address == deviceAddress)
                {
                    return device;
                }
            }
            return null;
        }
    }
}