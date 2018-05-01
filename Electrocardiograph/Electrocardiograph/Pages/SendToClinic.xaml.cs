namespace Electrocardiograph.Pages
{
    using Plugin.Messaging;
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendToClinic : ContentPage
    {
        private string _fileName;

        public SendToClinic(string fileName)
        {
            _fileName = fileName;
            InitializeComponent();
        }

        private void OnSendToClinicButtonClicked(object sender, EventArgs e)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;

            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                    .To(clinicEmailEntry.Text)
                    .Subject(subjectEntry.Text)
                    .Body(bodyEditor.Text)
                    .WithAttachment("/storage/emulated/0/Pictures/" + _fileName, "image/jpeg")
                    .Build();

                emailMessenger.SendEmail(email);
            }
        }
    }
}