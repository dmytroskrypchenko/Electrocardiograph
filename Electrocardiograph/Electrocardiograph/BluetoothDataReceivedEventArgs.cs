namespace Electrocardiograph
{
    using System;

    public class BluetoothDataReceivedEventArgs : EventArgs
    {
        public BluetoothDataReceivedEventArgs(string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }
    }
}