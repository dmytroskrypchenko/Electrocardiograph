namespace Electrocardiograph
{
    using System;
    using System.Collections.Generic;

    public interface IBluetooth
    {
        event EventHandler<BluetoothDataReceivedEventArgs> DataReceived;
        List<PairedDevice> PairedDevices { get; set; }
        bool ConnectToDevice(PairedDevice device, out string errorMessage);
    }
}
