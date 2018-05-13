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
        public event EventHandler<BluetoothDataReceivedEventArgs> DataReceived = delegate { };
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

            try
            {
                ConnectionStatus = BluetoothConnectionStatus.Connecting;
                BluetoothSocket.Connect();
            }
            catch (Java.IO.IOException connectException)
            {
                errorMessage = "Connection Failed:" + connectException.Message;
                return false;
            }
            catch (Exception connectException)
            {
                errorMessage = "Connection Failed:" + connectException.Message;
                return false;
            }
            ConnectionStatus = BluetoothConnectionStatus.Connected;

            StartListeningForDataFromBluetoothDevice();
            ConnectionStatus = BluetoothConnectionStatus.ConnectedAndListening;

            return true;
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
                new Thread(() =>
                {
                    byte[] buffer;

                    string previousEnding = "";
                    string currentMessage = "";

                    while (true)
                    {
                        if (BluetoothSocket.InputStream.IsDataAvailable())
                        {
                            buffer = new byte[1024];

                            int i = 0;
                            for (i = 0; i < 1024; i++)
                            {
                                // ordinarily, we'd just check to see of cur != 0, but that'll raise an exception
                                // here for some reason, so we have to call the IsDataAvaiable each time. :(
                                if (BluetoothSocket.InputStream.IsDataAvailable())
                                {
                                    byte cur = (byte)BluetoothSocket.InputStream.ReadByte();
                                    buffer[i] = cur;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // copy the buffer to a new array
                            byte[] message = new byte[i];
                            message = buffer.Take(i).ToArray();

                            var wholeMessage = Encoding.UTF8.GetString(message);
                            var lastIndexOfNewLine = wholeMessage.LastIndexOf("\r\n");
                            currentMessage = previousEnding + wholeMessage.Substring(0, lastIndexOfNewLine);
                            previousEnding = wholeMessage.Substring(lastIndexOfNewLine, wholeMessage.Length - lastIndexOfNewLine);

                            var points = currentMessage.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            DataReceived(this, new BluetoothDataReceivedEventArgs(points));
                        }
                        Thread.Sleep(250);
                    }
                }).Start();
            }
        }

        private BluetoothDevice FindDevice(string deviceAddress)
        {
            return BluetoothAdapter.BondedDevices.FirstOrDefault(device => device.Address == deviceAddress);
        }
    }
}