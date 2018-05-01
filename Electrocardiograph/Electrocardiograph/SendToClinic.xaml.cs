namespace Electrocardiograph
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendToClinic : ContentPage
    {
        public SendToClinic()
        {
            InitializeComponent();
        }

        private void OnSendToClinicButtonClicked(object sender, EventArgs e)
        {
        }
    }
}