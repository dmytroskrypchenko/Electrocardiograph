namespace Electrocardiograph.Droid
{
    using System.Text;

    public class MessageReceivedEventArgs : System.EventArgs
    {
        public MessageReceivedEventArgs(byte[] message)
        {
            MessageBytes = message;
        }

        public byte[] MessageBytes { get; set; }
        public string MessageString
        {
            get
            {
                return Encoding.UTF8.GetString(this.MessageBytes);
            }
        }
    }
}