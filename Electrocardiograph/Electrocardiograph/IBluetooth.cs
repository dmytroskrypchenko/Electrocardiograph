namespace Electrocardiograph
{
    using System.Collections.Generic;

    public interface IBluetooth
    {
        List<PairedDevice> PairedDevices { get; set; }
        bool ConnectToDevice(PairedDevice device, out string errorMessage);
    }
}
