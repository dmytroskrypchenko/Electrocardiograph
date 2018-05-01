namespace Electrocardiograph.Pages
{
    using Plugin.Messaging;
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
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                    .To("dim_skrip@mail.ru")
                    .Subject("Xamarin Messaging Plugin")
                    .Body("Hello from your friends at Xamarin!")
                    .Build();

                emailMessenger.SendEmail(email);
            }
        }
    }
}