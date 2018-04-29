﻿namespace Electrocardiograph
{
    using Xamarin.Forms;

    public partial class App : Application
    {
        static IBluetooth _bluetooth;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new DeviceList(_bluetooth));
        }

        public static void SetBluetooth(IBluetooth bluetooth)
        {
            _bluetooth = bluetooth;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
